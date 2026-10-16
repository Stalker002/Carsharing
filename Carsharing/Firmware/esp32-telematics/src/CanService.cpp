#include "CanService.h"
#include "AppConfig.h"

#if ENABLE_CAN
#include "driver/twai.h"
#endif

namespace Telematics {

namespace {
constexpr uint8_t ObdPidEngineRpm = 0x0C;
constexpr uint8_t ObdPidVehicleSpeed = 0x0D;
constexpr uint8_t ObdPidFuelLevel = 0x2F;

const uint8_t ObdPollPids[] = {
    ObdPidVehicleSpeed,
    ObdPidFuelLevel,
    ObdPidEngineRpm
};
}

void CanService::begin() {
#if ENABLE_CAN
    twai_general_config_t generalConfig = TWAI_GENERAL_CONFIG_DEFAULT(
        static_cast<gpio_num_t>(CAN_TX_PIN),
        static_cast<gpio_num_t>(CAN_RX_PIN),
        CAN_SELF_TEST_ENABLED ? TWAI_MODE_NO_ACK : TWAI_MODE_NORMAL);
    twai_timing_config_t timingConfig = TWAI_TIMING_CONFIG_500KBITS();
    twai_filter_config_t filterConfig = TWAI_FILTER_CONFIG_ACCEPT_ALL();

    started_ = twai_driver_install(&generalConfig, &timingConfig, &filterConfig) == ESP_OK
        && twai_start() == ESP_OK;
    Serial.printf("CAN/TWAI %s on TX=%d RX=%d speed=%d kbps self-test=%s\n",
        started_ ? "started" : "failed",
        CAN_TX_PIN,
        CAN_RX_PIN,
        CAN_SPEED_KBPS,
        CAN_SELF_TEST_ENABLED ? "on" : "off");
#else
    started_ = false;
#endif
}

void CanService::update(VehicleState& state) {
#if ENABLE_CAN
    if (!started_) {
        return;
    }

    maintainBus();
    sendSelfTestFrame();
    sendObdRequest();

    twai_message_t message;
    while (twai_receive(&message, 0) == ESP_OK) {
        processFrame(message.identifier, message.data, message.data_length_code, state);
    }
#else
    (void)state;
#endif
}

bool CanService::started() const {
    return started_;
}

void CanService::processFrame(uint32_t identifier, const uint8_t* data, uint8_t length, VehicleState& state) {
    processObdResponse(identifier, data, length, state);

    if (identifier == CAN_SELF_TEST_FRAME_ID && length >= 3) {
        state.canSpeedKmh = static_cast<float>(data[0]);
        state.fuelPercent = static_cast<float>(data[1]);
        state.ignitionOn = (data[2] & 0x01) == 0x01;
    }
}

void CanService::processObdResponse(uint32_t identifier, const uint8_t* data, uint8_t length, VehicleState& state) {
#if ENABLE_CAN
    if (identifier < CAN_OBD_RESPONSE_ID_MIN || identifier > CAN_OBD_RESPONSE_ID_MAX || length < 4) {
        return;
    }

    if (data[1] != 0x41) {
        return;
    }

    const uint8_t pid = data[2];
    switch (pid) {
        case ObdPidVehicleSpeed:
            state.canSpeedKmh = static_cast<float>(data[3]);
            break;

        case ObdPidFuelLevel:
            state.fuelPercent = (static_cast<float>(data[3]) * 100.0f) / 255.0f;
            break;

        case ObdPidEngineRpm:
            if (length >= 5) {
                state.engineRpm = static_cast<float>((static_cast<uint16_t>(data[3]) << 8) | data[4]) / 4.0f;
                state.ignitionOn = state.engineRpm > 0.0f;
            }
            break;

        default:
            break;
    }
#else
    (void)identifier;
    (void)data;
    (void)length;
    (void)state;
#endif
}

void CanService::sendObdRequest() {
#if ENABLE_CAN
    if (!CAN_OBD_POLL_ENABLED || !started_) {
        return;
    }

    const unsigned long now = millis();
    if (now - lastObdPollAt_ < CAN_OBD_POLL_INTERVAL_MS) {
        return;
    }

    lastObdPollAt_ = now;

    twai_status_info_t status {};
    if (twai_get_status_info(&status) != ESP_OK) {
        logCanDiagnostic("CAN status read failed");
        return;
    }

    if (status.state == TWAI_STATE_BUS_OFF) {
        logCanDiagnostic("CAN bus-off, waiting for recovery");
        return;
    }

    if (status.msgs_to_tx > 0) {
        logCanDiagnostic("CAN TX queue still waiting for ACK, pending frames", status.msgs_to_tx);
        return;
    }

    twai_message_t message {};
    message.identifier = CAN_OBD_REQUEST_ID;
    message.data_length_code = 8;
    message.data[0] = 0x02;
    message.data[1] = 0x01;
    message.data[2] = ObdPollPids[obdPidIndex_];
    message.data[3] = 0x55;
    message.data[4] = 0x55;
    message.data[5] = 0x55;
    message.data[6] = 0x55;
    message.data[7] = 0x55;

    obdPidIndex_ = (obdPidIndex_ + 1) % (sizeof(ObdPollPids) / sizeof(ObdPollPids[0]));

    const esp_err_t result = twai_transmit(&message, pdMS_TO_TICKS(10));
    if (result != ESP_OK) {
        logCanDiagnostic("CAN OBD request failed", static_cast<int>(result));
    }
#endif
}

void CanService::sendSelfTestFrame() {
#if ENABLE_CAN
    if (!CAN_SELF_TEST_ENABLED || !started_) {
        return;
    }

    const unsigned long now = millis();
    if (now - lastSelfTestAt_ < CAN_SELF_TEST_INTERVAL_MS) {
        return;
    }

    lastSelfTestAt_ = now;

    twai_message_t message {};
    message.identifier = CAN_SELF_TEST_FRAME_ID;
    message.data_length_code = 3;
    message.flags = TWAI_MSG_FLAG_SELF;
    message.data[0] = static_cast<uint8_t>((now / 1000UL) % 120UL);
    message.data[1] = 73;
    message.data[2] = 0x01;

    const esp_err_t result = twai_transmit(&message, 0);
    if (result == ESP_OK) {
        Serial.printf("CAN self-test sent id=0x%03X speed=%u fuel=%u ignition=%u\n",
            CAN_SELF_TEST_FRAME_ID,
            message.data[0],
            message.data[1],
            message.data[2] & 0x01);
    } else {
        Serial.printf("CAN self-test transmit failed: %d\n", static_cast<int>(result));
    }
#endif
}

void CanService::maintainBus() {
#if ENABLE_CAN
    twai_status_info_t status {};
    if (twai_get_status_info(&status) != ESP_OK) {
        return;
    }

    if (status.state == TWAI_STATE_BUS_OFF) {
        logCanDiagnostic("CAN bus-off detected, starting recovery");
        twai_initiate_recovery();
        return;
    }

    if (status.state == TWAI_STATE_STOPPED) {
        const esp_err_t result = twai_start();
        if (result == ESP_OK) {
            logCanDiagnostic("CAN restarted");
        } else {
            logCanDiagnostic("CAN restart failed", static_cast<int>(result));
        }
    }
#endif
}

void CanService::logCanDiagnostic(const char* message, int value) {
#if ENABLE_CAN
    const unsigned long now = millis();
    if (now - lastCanDiagnosticAt_ < 5000UL) {
        return;
    }

    lastCanDiagnosticAt_ = now;
    if (value == 0) {
        Serial.println(message);
    } else {
        Serial.printf("%s: %d\n", message, value);
    }
#else
    (void)message;
    (void)value;
#endif
}

}

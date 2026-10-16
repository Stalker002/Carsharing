#include <Arduino.h>
#include <ArduinoJson.h>
#include <Preferences.h>
#include <PubSubClient.h>
#include <TinyGPSPlus.h>
#include <WiFi.h>
#include <WiFiClient.h>
#include <WiFiClientSecure.h>
#include <cstring>
#include <time.h>

#include "Config.h"

#if ENABLE_CAN
#include "driver/twai.h"
#endif

namespace {
constexpr unsigned long MinTelemetryIntervalMs = 1000UL;
constexpr unsigned long MaxTelemetryIntervalMs = 300000UL;
constexpr uint8_t MaxMqttConnectFailuresBeforeWifiReset = 5;

#if MQTT_USE_TLS
WiFiClientSecure netClient;
#else
WiFiClient netClient;
#endif

PubSubClient mqtt(netClient);
Preferences preferences;
TinyGPSPlus gps;
HardwareSerial gpsSerial(2);

String telemetryTopic;
String heartbeatTopic;
String diagnosticsTopic;
String carCommandTopic;
String deviceCommandTopic;
String commandResultTopic;
String availabilityTopic;

String activeTripId;
String lastCommandId;

unsigned long telemetryIntervalMs = TELEMETRY_INTERVAL_MS;
unsigned long lastTelemetryAt = 0;
unsigned long lastHeartbeatAt = 0;
unsigned long lastDiagnosticsAt = 0;
unsigned long lastNetworkRetryAt = 0;
uint32_t telemetrySequence = 0;
uint32_t heartbeatSequence = 0;
uint32_t commandSequence = 0;
uint8_t mqttConnectFailures = 0;

bool doorsLocked = true;
bool engineEnabled = false;
bool ignitionOn = false;
float canSpeedKmh = NAN;
float fuelLevelPercent = NAN;

struct RelayPulse {
    int8_t pin;
    bool active;
    unsigned long stopAt;
};

RelayPulse lockRelay { LOCK_RELAY_PIN, false, 0 };
RelayPulse unlockRelay { UNLOCK_RELAY_PIN, false, 0 };

void setRelay(int8_t pin, bool enabled) {
    if (pin < 0) {
        return;
    }
    digitalWrite(pin, enabled ? HIGH : LOW);
}

void startRelayPulse(RelayPulse& relay) {
    if (lockRelay.pin == relay.pin) {
        lockRelay.active = false;
    }
    if (unlockRelay.pin == relay.pin) {
        unlockRelay.active = false;
    }
    setRelay(relay.pin, true);
    relay.active = true;
    relay.stopAt = millis() + RELAY_PULSE_MS;
}

void updateRelayPulse(RelayPulse& relay) {
    if (!relay.active) {
        return;
    }

    if (static_cast<long>(millis() - relay.stopAt) >= 0) {
        setRelay(relay.pin, false);
        relay.active = false;
    }
}

void setupPins() {
    if (LOCK_RELAY_PIN >= 0) {
        pinMode(LOCK_RELAY_PIN, OUTPUT);
    }
    if (UNLOCK_RELAY_PIN >= 0 && UNLOCK_RELAY_PIN != LOCK_RELAY_PIN) {
        pinMode(UNLOCK_RELAY_PIN, OUTPUT);
    }
    if (ENGINE_RELAY_PIN >= 0) {
        pinMode(ENGINE_RELAY_PIN, OUTPUT);
    }
    if (HAZARD_RELAY_PIN >= 0) {
        pinMode(HAZARD_RELAY_PIN, OUTPUT);
    }
    pinMode(STATUS_LED_PIN, OUTPUT);
    pinMode(BATTERY_ADC_PIN, INPUT);
    analogSetPinAttenuation(BATTERY_ADC_PIN, ADC_11db);

    setRelay(LOCK_RELAY_PIN, false);
    setRelay(UNLOCK_RELAY_PIN, false);
    setRelay(ENGINE_RELAY_PIN, false);
    setRelay(HAZARD_RELAY_PIN, false);
    digitalWrite(STATUS_LED_PIN, LOW);
}

String nowIsoUtc() {
    struct tm timeInfo {};
    if (getLocalTime(&timeInfo, 20)) {
        char buffer[25];
        strftime(buffer, sizeof(buffer), "%Y-%m-%dT%H:%M:%SZ", &timeInfo);
        return String(buffer);
    }

    return String("uptime-ms-") + String(millis());
}

String nextMessageId(const char* prefix, uint32_t sequence) {
    return String(prefix) + "-" + DEVICE_ID + "-" + String(sequence);
}

float readBatteryVoltage() {
    const int raw = analogRead(BATTERY_ADC_PIN);
    return (raw / ADC_MAX_VALUE) * ADC_REFERENCE_VOLTAGE * BATTERY_ADC_MULTIPLIER;
}

int signalLevel() {
    if (WiFi.status() != WL_CONNECTED) {
        return 0;
    }

    return constrain(map(WiFi.RSSI(), -100, -50, 0, 31), 0, 31);
}

void setNullableFloat(JsonDocument& doc, const char* key, float value) {
    if (isnan(value)) {
        doc[key] = nullptr;
        return;
    }

    doc[key] = value;
}

bool publishJson(const String& topic, JsonDocument& doc, bool retained) {
    char payload[MQTT_BUFFER_SIZE];
    const size_t length = serializeJson(doc, payload, sizeof(payload));
    if (length == 0 || length >= sizeof(payload)) {
        Serial.println("JSON payload is too large");
        return false;
    }

    return mqtt.publish(topic.c_str(), reinterpret_cast<const uint8_t*>(payload), length, retained);
}

bool publishJson(const String& topic, JsonDocument& doc) {
    return publishJson(topic, doc, false);
}

void addDeviceFields(JsonDocument& doc) {
    doc["schemaVersion"] = 1;
    doc["deviceId"] = DEVICE_ID;
    doc["carId"] = CAR_ID;
    doc["firmwareVersion"] = FIRMWARE_VERSION;
    doc["timestamp"] = nowIsoUtc();
    doc["uptimeMs"] = millis();
}

void publishCommandResult(
    const char* commandId,
    const char* status,
    const char* errorCode,
    const char* errorMessage) {
    StaticJsonDocument<512> doc;
    addDeviceFields(doc);
    doc["messageId"] = nextMessageId("command-result", ++commandSequence);
    doc["commandId"] = commandId;
    doc["status"] = status;
    doc["errorCode"] = errorCode;
    doc["errorMessage"] = errorMessage;

    publishJson(commandResultTopic, doc);
}

void publishCommandResult(const char* commandId, const char* status) {
    publishCommandResult(commandId, status, nullptr, nullptr);
}

void publishCommandResult(const char* commandId, const char* status, const char* errorCode) {
    publishCommandResult(commandId, status, errorCode, nullptr);
}

void publishTelemetry(const char* source = "Mixed") {
    StaticJsonDocument<MQTT_BUFFER_SIZE> doc;
    addDeviceFields(doc);
    doc["messageId"] = nextMessageId("telemetry", ++telemetrySequence);
    doc["tripId"] = activeTripId.length() == 0 ? nullptr : activeTripId.c_str();

    if (gps.location.isValid()) {
        doc["latitude"] = gps.location.lat();
        doc["longitude"] = gps.location.lng();
        doc["gpsFix"] = true;
    } else {
        doc["latitude"] = nullptr;
        doc["longitude"] = nullptr;
        doc["gpsFix"] = false;
    }

    if (gps.altitude.isValid()) {
        doc["altitudeMeters"] = gps.altitude.meters();
    } else {
        doc["altitudeMeters"] = nullptr;
    }

    if (gps.speed.isValid()) {
        doc["gpsSpeedKmh"] = gps.speed.kmph();
    } else {
        doc["gpsSpeedKmh"] = nullptr;
    }

    setNullableFloat(doc, "canSpeedKmh", canSpeedKmh);
    setNullableFloat(doc, "fuelLevel", fuelLevelPercent);
    doc["engineOn"] = engineEnabled;
    doc["ignitionOn"] = ignitionOn;
    doc["doorsLocked"] = doorsLocked;
    doc["batteryVoltage"] = readBatteryVoltage();
    doc["gsmSignalLevel"] = signalLevel();
    doc["satellites"] = gps.satellites.isValid() ? gps.satellites.value() : 0;
    doc["source"] = source;

    publishJson(telemetryTopic, doc);
}

void publishHeartbeat() {
    StaticJsonDocument<768> doc;
    addDeviceFields(doc);
    doc["messageId"] = nextMessageId("heartbeat", ++heartbeatSequence);
    doc["wifiConnected"] = WiFi.status() == WL_CONNECTED;
    doc["mqttConnected"] = mqtt.connected();
    doc["ipAddress"] = WiFi.status() == WL_CONNECTED ? WiFi.localIP().toString() : "";
    doc["rssi"] = WiFi.status() == WL_CONNECTED ? WiFi.RSSI() : 0;
    doc["freeHeap"] = ESP.getFreeHeap();
    doc["telemetryIntervalMs"] = telemetryIntervalMs;
    doc["doorsLocked"] = doorsLocked;
    doc["engineEnabled"] = engineEnabled;
    doc["activeTripId"] = activeTripId.length() == 0 ? nullptr : activeTripId.c_str();

    publishJson(heartbeatTopic, doc);
}

void publishDiagnostics() {
    StaticJsonDocument<768> doc;
    addDeviceFields(doc);
    doc["messageId"] = nextMessageId("diagnostics", millis());
    doc["chipModel"] = ESP.getChipModel();
    doc["chipRevision"] = ESP.getChipRevision();
    doc["cpuMhz"] = ESP.getCpuFreqMHz();
    doc["flashSize"] = ESP.getFlashChipSize();
    doc["sketchSize"] = ESP.getSketchSize();
    doc["freeSketchSpace"] = ESP.getFreeSketchSpace();
    doc["mqttState"] = mqtt.state();
    doc["mqttConnectFailures"] = mqttConnectFailures;
    doc["gpsCharsProcessed"] = gps.charsProcessed();
    doc["gpsFailedChecksum"] = gps.failedChecksum();
    doc["batteryVoltage"] = readBatteryVoltage();

    publishJson(diagnosticsTopic, doc);
}

bool targetMatches(JsonDocument& doc) {
    const char* targetDeviceId = doc["deviceId"] | "";
    const char* targetCarId = doc["carId"] | "";

    if (strlen(targetDeviceId) > 0 && strcmp(targetDeviceId, DEVICE_ID) != 0) {
        return false;
    }

    if (strlen(targetCarId) > 0 && strcmp(targetCarId, CAR_ID) != 0) {
        return false;
    }

    return true;
}

void handleSetTelemetryInterval(const char* commandId, JsonDocument& doc) {
    const unsigned long requested = doc["payload"]["intervalMs"] | telemetryIntervalMs;
    telemetryIntervalMs = constrain(requested, MinTelemetryIntervalMs, MaxTelemetryIntervalMs);
    preferences.putULong("telInterval", telemetryIntervalMs);
    publishCommandResult(commandId, "Succeeded");
}

void handleSetTripContext(const char* commandId, JsonDocument& doc) {
    const char* tripId = doc["payload"]["tripId"] | "";
    activeTripId = tripId;
    preferences.putString("tripId", activeTripId);
    publishCommandResult(commandId, "Succeeded");
}

void handleCommand(char* topic, byte* payload, unsigned int length) {
    Serial.print("Command received from ");
    Serial.println(topic);

    StaticJsonDocument<MQTT_BUFFER_SIZE> doc;
    const DeserializationError error = deserializeJson(doc, payload, length);
    if (error) {
        publishCommandResult("unknown", "Failed", "InvalidJson", error.c_str());
        return;
    }

    const char* commandId = doc["commandId"] | "unknown";
    const char* commandType = doc["commandType"] | "";

    if (!targetMatches(doc)) {
        publishCommandResult(commandId, "Failed", "WrongTarget", "Command target does not match this device");
        return;
    }

    if (lastCommandId == commandId) {
        publishCommandResult(commandId, "Succeeded", nullptr, "Duplicate command ignored");
        return;
    }

    if (strcmp(commandType, "GetState") == 0) {
        publishTelemetry("Device");
        publishCommandResult(commandId, "Succeeded");
    } else if (strcmp(commandType, "GetDiagnostics") == 0) {
        publishDiagnostics();
        publishCommandResult(commandId, "Succeeded");
    } else if (strcmp(commandType, "LockDoors") == 0) {
        startRelayPulse(lockRelay);
        doorsLocked = true;
        publishCommandResult(commandId, "Succeeded");
    } else if (strcmp(commandType, "UnlockDoors") == 0) {
        startRelayPulse(unlockRelay);
        doorsLocked = false;
        publishCommandResult(commandId, "Succeeded");
    } else if (strcmp(commandType, "EnableEngine") == 0) {
        setRelay(ENGINE_RELAY_PIN, true);
        engineEnabled = ENGINE_RELAY_PIN >= 0;
        publishCommandResult(commandId, "Succeeded");
    } else if (strcmp(commandType, "DisableEngine") == 0) {
        setRelay(ENGINE_RELAY_PIN, false);
        engineEnabled = false;
        publishCommandResult(commandId, "Succeeded");
    } else if (strcmp(commandType, "SetTelemetryInterval") == 0) {
        handleSetTelemetryInterval(commandId, doc);
    } else if (strcmp(commandType, "SetTripContext") == 0) {
        handleSetTripContext(commandId, doc);
    } else if (strcmp(commandType, "ClearTripContext") == 0) {
        activeTripId = "";
        preferences.remove("tripId");
        publishCommandResult(commandId, "Succeeded");
    } else if (strcmp(commandType, "RebootDevice") == 0) {
        publishCommandResult(commandId, "Succeeded");
        delay(300);
        ESP.restart();
    } else {
        publishCommandResult(commandId, "Failed", "UnknownCommand", "Unsupported command type");
        return;
    }

    lastCommandId = commandId;
}

void connectWiFi() {
    if (WiFi.status() == WL_CONNECTED) {
        return;
    }

    Serial.print("Connecting Wi-Fi: ");
    Serial.println(WIFI_SSID);

    WiFi.mode(WIFI_STA);
    WiFi.setSleep(false);
    WiFi.begin(WIFI_SSID, WIFI_PASSWORD);

    const unsigned long startedAt = millis();
    while (WiFi.status() != WL_CONNECTED && millis() - startedAt < 12000UL) {
        delay(250);
        Serial.print(".");
    }

    Serial.println();
    if (WiFi.status() == WL_CONNECTED) {
        Serial.print("Wi-Fi connected, IP: ");
        Serial.println(WiFi.localIP());
        configTime(0, 0, "pool.ntp.org", "time.google.com");
    } else {
        Serial.println("Wi-Fi connection failed");
    }
}

void publishAvailability(bool online) {
    StaticJsonDocument<256> doc;
    doc["deviceId"] = DEVICE_ID;
    doc["carId"] = CAR_ID;
    doc["status"] = online ? "online" : "offline";
    doc["timestamp"] = nowIsoUtc();
    publishJson(availabilityTopic, doc, true);
}

void connectMqtt() {
    if (mqtt.connected() || WiFi.status() != WL_CONNECTED) {
        return;
    }

#if MQTT_USE_TLS
    netClient.setCACert(MQTT_ROOT_CA);
#endif

    Serial.print("Connecting MQTT: ");
    Serial.print(MQTT_HOST);
    Serial.print(":");
    Serial.println(MQTT_PORT);

    StaticJsonDocument<256> willDoc;
    willDoc["deviceId"] = DEVICE_ID;
    willDoc["carId"] = CAR_ID;
    willDoc["status"] = "offline";
    willDoc["timestamp"] = nowIsoUtc();
    char willPayload[256];
    serializeJson(willDoc, willPayload, sizeof(willPayload));

    const bool hasCredentials = strlen(MQTT_USERNAME) > 0;
    const bool connected = hasCredentials
        ? mqtt.connect(DEVICE_ID, MQTT_USERNAME, MQTT_PASSWORD, availabilityTopic.c_str(), 0, true, willPayload)
        : mqtt.connect(DEVICE_ID, availabilityTopic.c_str(), 0, true, willPayload);

    if (connected) {
        mqttConnectFailures = 0;
        mqtt.subscribe(carCommandTopic.c_str());
        mqtt.subscribe(deviceCommandTopic.c_str());
        publishAvailability(true);
        publishHeartbeat();
        publishDiagnostics();
        Serial.println("MQTT connected");
    } else {
        mqttConnectFailures++;
        Serial.print("MQTT failed, rc=");
        Serial.println(mqtt.state());

        if (mqttConnectFailures >= MaxMqttConnectFailuresBeforeWifiReset) {
            mqttConnectFailures = 0;
            WiFi.disconnect();
        }
    }
}

void maintainNetwork() {
    if (WiFi.status() == WL_CONNECTED && mqtt.connected()) {
        digitalWrite(STATUS_LED_PIN, HIGH);
        mqtt.loop();
        return;
    }

    digitalWrite(STATUS_LED_PIN, LOW);
    const unsigned long now = millis();
    if (now - lastNetworkRetryAt < NETWORK_RETRY_INTERVAL_MS) {
        return;
    }

    lastNetworkRetryAt = now;
    connectWiFi();
    connectMqtt();
}

void readGps() {
    while (gpsSerial.available() > 0) {
        gps.encode(gpsSerial.read());
    }
}

#if ENABLE_CAN
void setupCan() {
    twai_general_config_t generalConfig = TWAI_GENERAL_CONFIG_DEFAULT(
        static_cast<gpio_num_t>(CAN_TX_PIN),
        static_cast<gpio_num_t>(CAN_RX_PIN),
        TWAI_MODE_NORMAL);
    twai_timing_config_t timingConfig = TWAI_TIMING_CONFIG_500KBITS();
    twai_filter_config_t filterConfig = TWAI_FILTER_CONFIG_ACCEPT_ALL();

    if (twai_driver_install(&generalConfig, &timingConfig, &filterConfig) == ESP_OK && twai_start() == ESP_OK) {
        Serial.println("CAN/TWAI started");
    } else {
        Serial.println("CAN/TWAI failed");
    }
}

void readCan() {
    twai_message_t message;
    while (twai_receive(&message, 0) == ESP_OK) {
        // Demo test-stand frame: id 0x100, speed/fuel/ignition.
        if (message.identifier == 0x100 && message.data_length_code >= 3) {
            canSpeedKmh = static_cast<float>(message.data[0]);
            fuelLevelPercent = static_cast<float>(message.data[1]);
            ignitionOn = (message.data[2] & 0x01) == 0x01;
        }
    }
}
#else
void setupCan() {}
void readCan() {}
#endif

void loadPersistentState() {
    preferences.begin("telematics", false);
    telemetryIntervalMs = preferences.getULong("telInterval", TELEMETRY_INTERVAL_MS);
    telemetryIntervalMs = constrain(telemetryIntervalMs, MinTelemetryIntervalMs, MaxTelemetryIntervalMs);
    activeTripId = preferences.getString("tripId", "");
}

void buildTopics() {
    telemetryTopic = String("cars/") + CAR_ID + "/telemetry";
    heartbeatTopic = String("cars/") + CAR_ID + "/heartbeat";
    diagnosticsTopic = String("cars/") + CAR_ID + "/diagnostics";
    carCommandTopic = String("cars/") + CAR_ID + "/commands";
    deviceCommandTopic = String("devices/") + DEVICE_ID + "/commands";
    commandResultTopic = String("cars/") + CAR_ID + "/command-results";
    availabilityTopic = String("devices/") + DEVICE_ID + "/availability";
}
}

void setup() {
    Serial.begin(115200);
    delay(500);

    setupPins();
    loadPersistentState();
    buildTopics();

    gpsSerial.begin(GPS_BAUD_RATE, SERIAL_8N1, GPS_RX_PIN, GPS_TX_PIN);
    setupCan();

    mqtt.setServer(MQTT_HOST, MQTT_PORT);
    mqtt.setCallback(handleCommand);
    mqtt.setBufferSize(MQTT_BUFFER_SIZE);
    mqtt.setKeepAlive(30);
    mqtt.setSocketTimeout(10);

    connectWiFi();
    connectMqtt();
}

void loop() {
    readGps();
    readCan();
    updateRelayPulse(lockRelay);
    updateRelayPulse(unlockRelay);
    maintainNetwork();

    const unsigned long now = millis();
    if (mqtt.connected() && now - lastTelemetryAt >= telemetryIntervalMs) {
        lastTelemetryAt = now;
        publishTelemetry();
    }

    if (mqtt.connected() && now - lastHeartbeatAt >= HEARTBEAT_INTERVAL_MS) {
        lastHeartbeatAt = now;
        publishHeartbeat();
    }

    if (mqtt.connected() && now - lastDiagnosticsAt >= DIAGNOSTICS_INTERVAL_MS) {
        lastDiagnosticsAt = now;
        publishDiagnostics();
    }
}

#pragma once

#include "DeviceState.h"

namespace Telematics {

class CanService {
public:
    void begin();
    void update(VehicleState& state);
    bool started() const;

private:
    bool started_ = false;
    unsigned long lastSelfTestAt_ = 0;
    unsigned long lastObdPollAt_ = 0;
    unsigned long lastCanDiagnosticAt_ = 0;
    uint8_t obdPidIndex_ = 0;

    void processFrame(uint32_t identifier, const uint8_t* data, uint8_t length, VehicleState& state);
    void processObdResponse(uint32_t identifier, const uint8_t* data, uint8_t length, VehicleState& state);
    void sendObdRequest();
    void sendSelfTestFrame();
    void maintainBus();
    void logCanDiagnostic(const char* message, int value = 0);
};

}

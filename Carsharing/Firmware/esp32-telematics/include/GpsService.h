#pragma once

#include <Arduino.h>
#include <TinyGPSPlus.h>
#include "DeviceState.h"

namespace Telematics {

class GpsService {
public:
    void begin();
    void update();
    GpsSnapshot snapshot();
    bool enabled() const;
    uint32_t charsProcessed();
    uint32_t failedChecksum();

private:
    TinyGPSPlus gps_;
    HardwareSerial serial_ { 2 };
    double lastLat_ = 0.0;
    double lastLon_ = 0.0;
    bool hasLastLocation_ = false;
    unsigned long lastFixAt_ = 0;
};

}

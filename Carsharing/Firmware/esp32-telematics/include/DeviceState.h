#pragma once

#include <Arduino.h>

namespace Telematics {

enum class DeviceStatus {
    Booting,
    ConnectingNetwork,
    ConnectingMqtt,
    Online,
    OfflineBuffering,
    Fault
};

struct VehicleState {
    bool doorsLocked = true;
    bool engineEnabled = false;
    bool ignitionOn = false;
    float canSpeedKmh = NAN;
    float fuelPercent = NAN;
    float engineRpm = NAN;
};

struct GpsSnapshot {
    bool enabled = false;
    bool fix = false;
    bool stale = false;
    double latitude = 0.0;
    double longitude = 0.0;
    double altitudeMeters = 0.0;
    bool hasAltitude = false;
    double speedKmh = 0.0;
    bool hasSpeed = false;
    double accuracyMeters = 0.0;
    bool hasAccuracy = false;
    uint32_t satellites = 0;
    uint32_t ageMs = 0;
};

struct CommandEnvelope {
    String commandId;
    String correlationId;
    String commandType;
};

const char* toString(DeviceStatus status);

}

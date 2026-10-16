#pragma once

#include <Arduino.h>
#include "DeviceState.h"

namespace Telematics {

class VehicleIo {
public:
    void begin();
    void update();
    void lockDoors(VehicleState& state);
    void unlockDoors(VehicleState& state);
    void enableEngine(VehicleState& state);
    void disableEngine(VehicleState& state);

private:
    struct RelayPulse {
        int8_t pin = -1;
        bool active = false;
        unsigned long stopAt = 0;
    };

    RelayPulse lockRelay_;
    RelayPulse unlockRelay_;

    void setRelay(int8_t pin, bool enabled);
    void startPulse(RelayPulse& relay, RelayPulse& opposite);
    void updatePulse(RelayPulse& relay);
};

}

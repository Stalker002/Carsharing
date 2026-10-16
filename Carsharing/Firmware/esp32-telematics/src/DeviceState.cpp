#include "DeviceState.h"

namespace Telematics {

const char* toString(DeviceStatus status) {
    switch (status) {
        case DeviceStatus::Booting:
            return "Booting";
        case DeviceStatus::ConnectingNetwork:
            return "ConnectingNetwork";
        case DeviceStatus::ConnectingMqtt:
            return "ConnectingMqtt";
        case DeviceStatus::Online:
            return "Online";
        case DeviceStatus::OfflineBuffering:
            return "OfflineBuffering";
        case DeviceStatus::Fault:
            return "Fault";
    }

    return "Fault";
}

}

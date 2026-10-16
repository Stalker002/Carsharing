#include "VehicleIo.h"
#include "AppConfig.h"

namespace Telematics {

void VehicleIo::begin() {
    lockRelay_.pin = LOCK_RELAY_PIN;
    lockRelay_.active = false;
    lockRelay_.stopAt = 0;
    unlockRelay_.pin = UNLOCK_RELAY_PIN;
    unlockRelay_.active = false;
    unlockRelay_.stopAt = 0;

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

void VehicleIo::update() {
    updatePulse(lockRelay_);
    updatePulse(unlockRelay_);
}

void VehicleIo::lockDoors(VehicleState& state) {
    startPulse(lockRelay_, unlockRelay_);
    state.doorsLocked = true;
}

void VehicleIo::unlockDoors(VehicleState& state) {
    startPulse(unlockRelay_, lockRelay_);
    state.doorsLocked = false;
}

void VehicleIo::enableEngine(VehicleState& state) {
    setRelay(ENGINE_RELAY_PIN, true);
    state.engineEnabled = ENGINE_RELAY_PIN >= 0;
}

void VehicleIo::disableEngine(VehicleState& state) {
    setRelay(ENGINE_RELAY_PIN, false);
    state.engineEnabled = false;
}

void VehicleIo::setRelay(int8_t pin, bool enabled) {
    if (pin < 0) {
        return;
    }
    digitalWrite(pin, enabled ? HIGH : LOW);
}

void VehicleIo::startPulse(RelayPulse& relay, RelayPulse& opposite) {
    setRelay(opposite.pin, false);
    opposite.active = false;

    setRelay(relay.pin, true);
    relay.active = true;
    relay.stopAt = millis() + RELAY_PULSE_MS;
}

void VehicleIo::updatePulse(RelayPulse& relay) {
    if (!relay.active) {
        return;
    }

    if (static_cast<long>(millis() - relay.stopAt) >= 0) {
        setRelay(relay.pin, false);
        relay.active = false;
    }
}

}

#include "GpsService.h"
#include "AppConfig.h"

namespace Telematics {

void GpsService::begin() {
#if ENABLE_GPS
    serial_.begin(GPS_BAUD_RATE, SERIAL_8N1, GPS_RX_PIN, GPS_TX_PIN);
#endif
}

void GpsService::update() {
#if ENABLE_GPS
    while (serial_.available() > 0) {
        gps_.encode(serial_.read());
    }
#endif
}

GpsSnapshot GpsService::snapshot() {
    GpsSnapshot result;
    result.enabled = enabled();

#if ENABLE_GPS
    if (gps_.location.isValid()) {
        result.fix = true;
        result.latitude = gps_.location.lat();
        result.longitude = gps_.location.lng();
        result.ageMs = gps_.location.age();
        result.stale = result.ageMs > 15000UL;
        lastLat_ = result.latitude;
        lastLon_ = result.longitude;
        hasLastLocation_ = true;
        lastFixAt_ = millis();
    } else if (hasLastLocation_) {
        result.latitude = lastLat_;
        result.longitude = lastLon_;
        result.ageMs = millis() - lastFixAt_;
        result.stale = true;
    }

    result.hasAltitude = gps_.altitude.isValid();
    result.altitudeMeters = result.hasAltitude ? gps_.altitude.meters() : 0.0;
    result.hasSpeed = gps_.speed.isValid();
    result.speedKmh = result.hasSpeed ? gps_.speed.kmph() : 0.0;
    result.hasAccuracy = gps_.hdop.isValid();
    result.accuracyMeters = result.hasAccuracy ? gps_.hdop.hdop() * 5.0 : 0.0;
    result.satellites = gps_.satellites.isValid() ? gps_.satellites.value() : 0;
#endif

    return result;
}

bool GpsService::enabled() const {
#if ENABLE_GPS
    return true;
#else
    return false;
#endif
}

uint32_t GpsService::charsProcessed() {
#if ENABLE_GPS
    return gps_.charsProcessed();
#else
    return 0;
#endif
}

uint32_t GpsService::failedChecksum() {
#if ENABLE_GPS
    return gps_.failedChecksum();
#else
    return 0;
#endif
}

}

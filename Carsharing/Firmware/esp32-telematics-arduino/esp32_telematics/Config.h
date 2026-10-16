#pragma once

// Arduino IDE project configuration.
// Open esp32_telematics.ino, then change these values before uploading.

#define DEVICE_ID "esp32-car-001"
#define CAR_ID "car-001"
#define FIRMWARE_VERSION "1.0.0"

#define WIFI_SSID "your-wifi-ssid"
#define WIFI_PASSWORD "your-wifi-password"

#define MQTT_HOST "192.168.1.10"
#define MQTT_PORT 1883
#define MQTT_USERNAME ""
#define MQTT_PASSWORD ""

// Set to 1 only after you configure MQTT_ROOT_CA.
#define MQTT_USE_TLS 0
#define MQTT_ROOT_CA ""

#define TELEMETRY_INTERVAL_MS 10000UL
#define HEARTBEAT_INTERVAL_MS 30000UL
#define DIAGNOSTICS_INTERVAL_MS 60000UL
#define NETWORK_RETRY_INTERVAL_MS 5000UL

#define GPS_RX_PIN 17
#define GPS_TX_PIN 16
#define GPS_BAUD_RATE 9600

#define LOCK_RELAY_PIN 18
#define UNLOCK_RELAY_PIN LOCK_RELAY_PIN
#define ENGINE_RELAY_PIN -1
#define HAZARD_RELAY_PIN 19
#define STATUS_LED_PIN 2

#define BATTERY_ADC_PIN 34
#define BATTERY_ADC_MULTIPLIER 11.0f
#define ADC_REFERENCE_VOLTAGE 3.3f
#define ADC_MAX_VALUE 4095.0f

// Enable when SN65HVD230 is connected to ESP32 TWAI/CAN pins.
#define ENABLE_CAN 1
#define CAN_TX_PIN 4
#define CAN_RX_PIN 5

#define SIM800_RX_PIN 27
#define SIM800_TX_PIN 26

// Relay outputs are pulsed for lock/unlock commands.
#define RELAY_PULSE_MS 700UL

// MQTT payload size. Increase if you add many fields.
#define MQTT_BUFFER_SIZE 1536

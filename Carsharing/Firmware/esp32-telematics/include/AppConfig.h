#pragma once

#if __has_include("config.h")
#include "config.h"
#else
#include "config.example.h"
#endif

#ifndef DEVICE_ID
#define DEVICE_ID "esp32-car-001"
#endif

#ifndef CAR_ID
#define CAR_ID "car-001"
#endif

#ifndef FIRMWARE_VERSION
#define FIRMWARE_VERSION "0.1.0"
#endif

#ifndef WIFI_SSID
#define WIFI_SSID "your-wifi-ssid"
#endif

#ifndef WIFI_PASSWORD
#define WIFI_PASSWORD "your-wifi-password"
#endif

#ifndef MQTT_HOST
#define MQTT_HOST "192.168.1.10"
#endif

#ifndef MQTT_PORT
#define MQTT_PORT 1883
#endif

#ifndef MQTT_USERNAME
#define MQTT_USERNAME ""
#endif

#ifndef MQTT_PASSWORD
#define MQTT_PASSWORD ""
#endif

#ifndef MQTT_USE_TLS
#define MQTT_USE_TLS 0
#endif

#ifndef MQTT_ROOT_CA
#define MQTT_ROOT_CA ""
#endif

#ifndef TELEMATICS_TRANSPORT_HTTP
#define TELEMATICS_TRANSPORT_HTTP 1
#endif

#ifndef ENABLE_SIM800_TRANSPORT
#define ENABLE_SIM800_TRANSPORT 0
#endif

#ifndef TELEMATICS_API_BASE_URL
#define TELEMATICS_API_BASE_URL "http://192.168.1.10:5007"
#endif

#ifndef TELEMATICS_API_USE_WIFI_GATEWAY
#define TELEMATICS_API_USE_WIFI_GATEWAY 0
#endif

#ifndef TELEMATICS_API_GATEWAY_PORT
#define TELEMATICS_API_GATEWAY_PORT 5007
#endif

#ifndef TELEMATICS_DEVICE_KEY
#define TELEMATICS_DEVICE_KEY "esp32-test-001"
#endif

#ifndef HTTP_TIMEOUT_MS
#define HTTP_TIMEOUT_MS 5000
#endif

#ifndef COMMAND_POLL_INTERVAL_MS
#define COMMAND_POLL_INTERVAL_MS 5000UL
#endif

#ifndef MQTT_BUFFER_SIZE
#define MQTT_BUFFER_SIZE 1536
#endif

#ifndef TELEMETRY_INTERVAL_MS
#define TELEMETRY_INTERVAL_MS 10000UL
#endif

#ifndef HEARTBEAT_INTERVAL_MS
#define HEARTBEAT_INTERVAL_MS 30000UL
#endif

#ifndef DIAGNOSTICS_INTERVAL_MS
#define DIAGNOSTICS_INTERVAL_MS 60000UL
#endif

#ifndef NETWORK_RETRY_INTERVAL_MS
#define NETWORK_RETRY_INTERVAL_MS 5000UL
#endif

#ifndef RELAY_PULSE_MS
#define RELAY_PULSE_MS 700UL
#endif

#ifndef COMMAND_DEDUP_CACHE_SIZE
#define COMMAND_DEDUP_CACHE_SIZE 8
#endif

#ifndef MIN_TELEMETRY_INTERVAL_MS
#define MIN_TELEMETRY_INTERVAL_MS 1000UL
#endif

#ifndef MAX_TELEMETRY_INTERVAL_MS
#define MAX_TELEMETRY_INTERVAL_MS 300000UL
#endif

#ifndef ENABLE_GPS
#define ENABLE_GPS 1
#endif

#ifndef GPS_RX_PIN
#define GPS_RX_PIN 17
#endif

#ifndef GPS_TX_PIN
#define GPS_TX_PIN 16
#endif

#ifndef GPS_BAUD_RATE
#define GPS_BAUD_RATE 9600
#endif

#ifndef LOCK_RELAY_PIN
#define LOCK_RELAY_PIN 18
#endif

#ifndef UNLOCK_RELAY_PIN
#define UNLOCK_RELAY_PIN LOCK_RELAY_PIN
#endif

#ifndef ENGINE_RELAY_PIN
#define ENGINE_RELAY_PIN -1
#endif

#ifndef HAZARD_RELAY_PIN
#define HAZARD_RELAY_PIN 19
#endif

#ifndef STATUS_LED_PIN
#define STATUS_LED_PIN 2
#endif

#ifndef BATTERY_ADC_PIN
#define BATTERY_ADC_PIN 34
#endif

#ifndef BATTERY_ADC_MULTIPLIER
#define BATTERY_ADC_MULTIPLIER 11.0f
#endif

#ifndef ADC_REFERENCE_VOLTAGE
#define ADC_REFERENCE_VOLTAGE 3.3f
#endif

#ifndef ADC_MAX_VALUE
#define ADC_MAX_VALUE 4095.0f
#endif

#ifndef ENABLE_CAN
#define ENABLE_CAN 1
#endif

#ifndef CAN_TX_PIN
#define CAN_TX_PIN 4
#endif

#ifndef CAN_RX_PIN
#define CAN_RX_PIN 5
#endif

#ifndef SIM800_RX_PIN
#define SIM800_RX_PIN 27
#endif

#ifndef SIM800_TX_PIN
#define SIM800_TX_PIN 26
#endif

#ifndef SIM800_BAUD_RATE
#define SIM800_BAUD_RATE 9600
#endif

#ifndef SIM800_APN
#define SIM800_APN "internet.life.com.by"
#endif

#ifndef SIM800_GPRS_USER
#define SIM800_GPRS_USER ""
#endif

#ifndef SIM800_GPRS_PASSWORD
#define SIM800_GPRS_PASSWORD ""
#endif

#ifndef CAN_SPEED_KBPS
#define CAN_SPEED_KBPS 500
#endif

#ifndef CAN_SELF_TEST_ENABLED
#define CAN_SELF_TEST_ENABLED 0
#endif

#ifndef CAN_SELF_TEST_INTERVAL_MS
#define CAN_SELF_TEST_INTERVAL_MS 2000UL
#endif

#ifndef CAN_SELF_TEST_FRAME_ID
#define CAN_SELF_TEST_FRAME_ID 0x100
#endif

#ifndef CAN_OBD_POLL_ENABLED
#define CAN_OBD_POLL_ENABLED 1
#endif

#ifndef CAN_OBD_POLL_INTERVAL_MS
#define CAN_OBD_POLL_INTERVAL_MS 1000UL
#endif

#ifndef CAN_OBD_REQUEST_ID
#define CAN_OBD_REQUEST_ID 0x7DF
#endif

#ifndef CAN_OBD_RESPONSE_ID_MIN
#define CAN_OBD_RESPONSE_ID_MIN 0x7E8
#endif

#ifndef CAN_OBD_RESPONSE_ID_MAX
#define CAN_OBD_RESPONSE_ID_MAX 0x7EF
#endif

#ifndef ENABLE_OFFLINE_BUFFER
#define ENABLE_OFFLINE_BUFFER 0
#endif

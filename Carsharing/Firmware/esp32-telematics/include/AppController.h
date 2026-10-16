#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>
#include <HTTPClient.h>
#include <Preferences.h>
#include <PubSubClient.h>
#include <WiFi.h>
#include <WiFiClient.h>
#include <WiFiClientSecure.h>
#include "AppConfig.h"

#if ENABLE_SIM800_TRANSPORT
#define TINY_GSM_MODEM_SIM800
#include <TinyGsmClient.h>
#endif

#include "CanService.h"
#include "DeviceState.h"
#include "GpsService.h"
#include "VehicleIo.h"

namespace Telematics {

class AppController {
public:
    void setup();
    void loop();
    void onMqttMessage(char* topic, byte* payload, unsigned int length);

private:
#if MQTT_USE_TLS
    WiFiClientSecure networkClient_;
#else
    WiFiClient networkClient_;
#endif
    PubSubClient mqtt_ { networkClient_ };
#if ENABLE_SIM800_TRANSPORT
    HardwareSerial simSerial_ { 1 };
    TinyGsm modem_ { simSerial_ };
    TinyGsmClient gsmClient_ { modem_ };
    bool sim800Started_ = false;
#endif
    Preferences preferences_;
    VehicleIo vehicleIo_;
    GpsService gps_;
    CanService can_;
    VehicleState vehicle_;
    DeviceStatus status_ = DeviceStatus::Booting;

    String telemetryTopic_;
    String heartbeatTopic_;
    String commandTopic_;
    String commandResultTopic_;
    String diagnosticsTopic_;

    String dedupCache_[COMMAND_DEDUP_CACHE_SIZE];
    uint8_t dedupCursor_ = 0;
    unsigned long telemetryIntervalMs_ = TELEMETRY_INTERVAL_MS;
    unsigned long lastTelemetryAt_ = 0;
    unsigned long lastHeartbeatAt_ = 0;
    unsigned long lastDiagnosticsAt_ = 0;
    unsigned long lastCommandPollAt_ = 0;
    unsigned long lastNetworkRetryAt_ = 0;
    unsigned long lastWiFiStatusLogAt_ = 0;
    unsigned long reconnectDelayMs_ = NETWORK_RETRY_INTERVAL_MS;
    uint32_t telemetrySequence_ = 0;
    uint32_t heartbeatSequence_ = 0;
    uint32_t diagnosticsSequence_ = 0;
    uint32_t commandResultSequence_ = 0;
    bool lastMqttConnected_ = false;
    bool timeConfigured_ = false;
    bool bootDiagnosticsPending_ = true;
    bool connectivityLostDiagnosticsPending_ = false;

    void buildTopics();
    void loadPersistentState();
    void maintainNetwork();
    void connectWiFi();
    void logWiFiStatus();
    void connectSim800();
    void connectMqtt();
    void configureTimeIfNeeded();
    bool httpTransportConnected();
    int networkSignalLevel();
    String networkTransportName() const;
    void publishPendingDiagnostics();
    bool publishJson(const String& topic, JsonDocument& doc, bool retained = false);
    void publishTelemetry(const char* source = "Mixed");
    void publishHeartbeat();
    void publishDiagnostics(const char* eventType = "Periodic", const char* message = nullptr);
    void publishCommandResult(const CommandEnvelope& command, const char* status);
    void publishCommandError(const CommandEnvelope& command, const char* code, const char* message);
    bool postHttpJson(const String& path, JsonDocument& doc, String* response = nullptr);
    String apiBaseUrl() const;
#if ENABLE_SIM800_TRANSPORT
    bool sendGsmHttpRequest(const char* method, const String& path, const String& body, String* response = nullptr);
    bool parseHttpBaseUrl(String& host, uint16_t& port, String& pathPrefix) const;
#endif
    bool sendHttpTelemetry(const char* source = "Mixed");
    bool sendHttpHeartbeat();
    void pollHttpCommands();
    void handleHttpCommand(JsonObject command);
    void acknowledgeHttpCommand(const String& commandId, bool success, const char* message = nullptr);
    bool applyCommand(const CommandEnvelope& command, JsonVariantConst payload);
    void handleCommand(JsonDocument& doc);
    bool targetMatches(JsonDocument& doc) const;
    bool isDuplicateCommand(const String& commandId) const;
    void rememberCommand(const String& commandId);
    bool commandExpired(JsonDocument& doc) const;
    bool parseIsoUtc(const char* value, time_t& parsed) const;
    bool timeSynchronized() const;
    String nowIsoUtc() const;
    String nextMessageId(const char* prefix, uint32_t sequence) const;
    float readBatteryVoltage() const;
    int wifiRssi() const;
    void setStatus(DeviceStatus status);
};

}

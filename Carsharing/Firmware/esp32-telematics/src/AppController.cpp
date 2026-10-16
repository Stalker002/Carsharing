#include "AppController.h"
#include "AppConfig.h"
#include <cstring>
#include <cstdio>
#include <esp_system.h>
#include <time.h>

namespace {
Telematics::AppController* activeApp = nullptr;
constexpr bool UseHttpTransport = TELEMATICS_TRANSPORT_HTTP == 1;
constexpr bool UseSim800Transport = ENABLE_SIM800_TRANSPORT == 1;

void mqttCallback(char* topic, byte* payload, unsigned int length) {
    if (activeApp != nullptr) {
        activeApp->onMqttMessage(topic, payload, length);
    }
}
}

namespace Telematics {

void AppController::setup() {
    activeApp = this;
    Serial.begin(115200);
    delay(300);
    Serial.println("ESP32 telematics boot");
    Serial.printf("Device=%s car=%s firmware=%s\n", DEVICE_ID, CAR_ID, FIRMWARE_VERSION);
    Serial.printf("Transport=%s API=%s deviceKey=%s\n",
        UseHttpTransport ? "HTTP" : "MQTT",
        TELEMATICS_API_USE_WIFI_GATEWAY ? "wifi-gateway" : TELEMATICS_API_BASE_URL,
        TELEMATICS_DEVICE_KEY);

    setStatus(DeviceStatus::Booting);
    vehicleIo_.begin();
    loadPersistentState();
    buildTopics();
    gps_.begin();
    can_.begin();

    if (!UseHttpTransport) {
        mqtt_.setServer(MQTT_HOST, MQTT_PORT);
        mqtt_.setCallback(mqttCallback);
        mqtt_.setBufferSize(MQTT_BUFFER_SIZE);
        mqtt_.setKeepAlive(30);
        mqtt_.setSocketTimeout(10);
    }

    maintainNetwork();
}

void AppController::loop() {
    gps_.update();
    can_.update(vehicle_);
    vehicleIo_.update();
    maintainNetwork();

    const unsigned long now = millis();
    if (UseHttpTransport && httpTransportConnected() && now - lastTelemetryAt_ >= telemetryIntervalMs_) {
        lastTelemetryAt_ = now;
        sendHttpTelemetry();
    } else if (!UseHttpTransport && mqtt_.connected() && now - lastTelemetryAt_ >= telemetryIntervalMs_) {
        lastTelemetryAt_ = now;
        publishTelemetry();
    }

    if (UseHttpTransport && httpTransportConnected() && now - lastHeartbeatAt_ >= HEARTBEAT_INTERVAL_MS) {
        lastHeartbeatAt_ = now;
        sendHttpHeartbeat();
    } else if (!UseHttpTransport && mqtt_.connected() && now - lastHeartbeatAt_ >= HEARTBEAT_INTERVAL_MS) {
        lastHeartbeatAt_ = now;
        publishHeartbeat();
    }

    if (UseHttpTransport && httpTransportConnected() && now - lastCommandPollAt_ >= COMMAND_POLL_INTERVAL_MS) {
        lastCommandPollAt_ = now;
        pollHttpCommands();
    }

    if (!UseHttpTransport && mqtt_.connected() && now - lastDiagnosticsAt_ >= DIAGNOSTICS_INTERVAL_MS) {
        lastDiagnosticsAt_ = now;
        publishDiagnostics();
    }
}

void AppController::onMqttMessage(char* topic, byte* payload, unsigned int length) {
    (void)topic;

    if (length == 0 || length >= MQTT_BUFFER_SIZE) {
        CommandEnvelope command;
        command.commandId = "unknown";
        publishCommandError(command, "PayloadTooLarge", "Command payload is empty or too large");
        return;
    }

    StaticJsonDocument<MQTT_BUFFER_SIZE> doc;
    const DeserializationError error = deserializeJson(doc, payload, length);
    if (error) {
        CommandEnvelope command;
        command.commandId = "unknown";
        publishCommandError(command, "InvalidJson", error.c_str());
        return;
    }

    handleCommand(doc);
}

void AppController::buildTopics() {
    telemetryTopic_ = String("cars/") + CAR_ID + "/telemetry";
    heartbeatTopic_ = String("cars/") + CAR_ID + "/heartbeat";
    commandTopic_ = String("cars/") + CAR_ID + "/commands";
    commandResultTopic_ = String("cars/") + CAR_ID + "/command-results";
    diagnosticsTopic_ = String("cars/") + CAR_ID + "/diagnostics";
}

void AppController::loadPersistentState() {
    preferences_.begin("telematics", false);
    telemetryIntervalMs_ = preferences_.getULong("telInterval", TELEMETRY_INTERVAL_MS);
    telemetryIntervalMs_ = constrain(telemetryIntervalMs_, MIN_TELEMETRY_INTERVAL_MS, MAX_TELEMETRY_INTERVAL_MS);
}

void AppController::maintainNetwork() {
    if (UseHttpTransport && httpTransportConnected()) {
        logWiFiStatus();
        if (!UseSim800Transport) {
            configureTimeIfNeeded();
        }
        reconnectDelayMs_ = NETWORK_RETRY_INTERVAL_MS;
        setStatus(DeviceStatus::Online);
        digitalWrite(STATUS_LED_PIN, HIGH);
        return;
    }

    if (!UseHttpTransport && WiFi.status() == WL_CONNECTED && mqtt_.connected()) {
        if (!lastMqttConnected_) {
            publishDiagnostics("ConnectivityRestored", "MQTT connection restored");
        }
        lastMqttConnected_ = true;
        reconnectDelayMs_ = NETWORK_RETRY_INTERVAL_MS;
        setStatus(DeviceStatus::Online);
        digitalWrite(STATUS_LED_PIN, HIGH);
        mqtt_.loop();
        return;
    }

    configureTimeIfNeeded();

    if (lastMqttConnected_) {
        connectivityLostDiagnosticsPending_ = true;
    }
    lastMqttConnected_ = false;
    setStatus(DeviceStatus::OfflineBuffering);
    digitalWrite(STATUS_LED_PIN, LOW);

    const unsigned long now = millis();
    if (now - lastNetworkRetryAt_ < reconnectDelayMs_) {
        return;
    }

    lastNetworkRetryAt_ = now;
    if (UseHttpTransport && UseSim800Transport) {
        connectSim800();
    } else {
        connectWiFi();
    }
    if (!UseHttpTransport) {
        connectMqtt();
    }
    reconnectDelayMs_ = min(reconnectDelayMs_ * 2UL, 60000UL);
}

void AppController::connectWiFi() {
    if (WiFi.status() == WL_CONNECTED) {
        return;
    }

    setStatus(DeviceStatus::ConnectingNetwork);
    Serial.printf("WiFi connecting to SSID '%s'\n", WIFI_SSID);
    WiFi.disconnect(false);
    WiFi.mode(WIFI_STA);
    WiFi.setSleep(false);
    WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
}

void AppController::logWiFiStatus() {
    if (UseSim800Transport || WiFi.status() != WL_CONNECTED) {
        return;
    }

    const unsigned long now = millis();
    if (now - lastWiFiStatusLogAt_ < 30000UL) {
        return;
    }

    lastWiFiStatusLogAt_ = now;
    Serial.printf("WiFi connected ip=%s gateway=%s rssi=%d api=%s\n",
        WiFi.localIP().toString().c_str(),
        WiFi.gatewayIP().toString().c_str(),
        WiFi.RSSI(),
        apiBaseUrl().c_str());
}

void AppController::connectSim800() {
#if ENABLE_SIM800_TRANSPORT
    setStatus(DeviceStatus::ConnectingNetwork);

    if (!sim800Started_) {
        simSerial_.begin(SIM800_BAUD_RATE, SERIAL_8N1, SIM800_RX_PIN, SIM800_TX_PIN);
        delay(300);
        sim800Started_ = true;
        Serial.printf("SIM800L UART started RX=%d TX=%d baud=%d\n", SIM800_RX_PIN, SIM800_TX_PIN, SIM800_BAUD_RATE);

        if (!modem_.init()) {
            Serial.println("SIM800L init failed");
            return;
        }

        const String imei = modem_.getIMEI();
        if (imei.length() > 0) {
            Serial.print("SIM800L IMEI: ");
            Serial.println(imei);
        }
    }

    if (!modem_.isNetworkConnected()) {
        Serial.println("SIM800L waiting for life network...");
        if (!modem_.waitForNetwork(60000L)) {
            Serial.println("SIM800L network registration failed");
            return;
        }
    }

    if (!modem_.isGprsConnected()) {
        Serial.print("SIM800L connecting GPRS APN: ");
        Serial.println(SIM800_APN);
        if (!modem_.gprsConnect(SIM800_APN, SIM800_GPRS_USER, SIM800_GPRS_PASSWORD)) {
            Serial.println("SIM800L GPRS connect failed");
            return;
        }
    }

    Serial.print("SIM800L GPRS connected, signal quality: ");
    Serial.println(modem_.getSignalQuality());
#endif
}

void AppController::connectMqtt() {
    if (UseHttpTransport) {
        return;
    }

    if (mqtt_.connected() || WiFi.status() != WL_CONNECTED) {
        return;
    }

    setStatus(DeviceStatus::ConnectingMqtt);
#if MQTT_USE_TLS
    networkClient_.setCACert(MQTT_ROOT_CA);
#endif

    const bool hasCredentials = strlen(MQTT_USERNAME) > 0;
    const bool connected = hasCredentials
        ? mqtt_.connect(DEVICE_ID, MQTT_USERNAME, MQTT_PASSWORD)
        : mqtt_.connect(DEVICE_ID);

    if (connected) {
        mqtt_.subscribe(commandTopic_.c_str(), 1);
        publishHeartbeat();
        publishPendingDiagnostics();
        publishDiagnostics("MqttConnected", "MQTT connected");
    }
}

void AppController::configureTimeIfNeeded() {
    if (timeConfigured_ || WiFi.status() != WL_CONNECTED) {
        return;
    }

    configTime(0, 0, "pool.ntp.org", "time.google.com");
    timeConfigured_ = true;
}

void AppController::publishPendingDiagnostics() {
    if (bootDiagnosticsPending_) {
        publishDiagnostics("Boot", "Device booted");
        bootDiagnosticsPending_ = false;
    }

    if (connectivityLostDiagnosticsPending_) {
        publishDiagnostics("ConnectivityLost", "MQTT connection was lost");
        connectivityLostDiagnosticsPending_ = false;
    }
}

bool AppController::publishJson(const String& topic, JsonDocument& doc, bool retained) {
    if (!mqtt_.connected()) {
        return false;
    }

    char payload[MQTT_BUFFER_SIZE];
    const size_t length = serializeJson(doc, payload, sizeof(payload));
    if (length == 0 || length >= sizeof(payload)) {
        return false;
    }

    return mqtt_.publish(topic.c_str(), reinterpret_cast<const uint8_t*>(payload), length, retained);
}

bool AppController::postHttpJson(const String& path, JsonDocument& doc, String* response) {
    if (!httpTransportConnected()) {
        return false;
    }

    String payload;
    serializeJson(doc, payload);

#if ENABLE_SIM800_TRANSPORT
    if (UseSim800Transport) {
        return sendGsmHttpRequest("POST", path, payload, response);
    }
#endif

    HTTPClient http;
    http.setTimeout(HTTP_TIMEOUT_MS);
    const String url = apiBaseUrl() + path;
    http.begin(url);
    http.addHeader("Content-Type", "application/json");
    http.addHeader("X-Device-Key", TELEMATICS_DEVICE_KEY);

    const int statusCode = http.POST(payload);
    if (response != nullptr) {
        *response = http.getString();
    }
    http.end();

    Serial.printf("HTTP POST %s -> %d\n", url.c_str(), statusCode);
    return statusCode >= 200 && statusCode < 300;
}

String AppController::apiBaseUrl() const {
#if TELEMATICS_API_USE_WIFI_GATEWAY
    if (WiFi.status() == WL_CONNECTED) {
        return String("http://") + WiFi.gatewayIP().toString() + ":" + String(TELEMATICS_API_GATEWAY_PORT);
    }
#endif

    return String(TELEMATICS_API_BASE_URL);
}

#if ENABLE_SIM800_TRANSPORT
bool AppController::sendGsmHttpRequest(const char* method, const String& path, const String& body, String* response) {
    String host;
    String pathPrefix;
    uint16_t port = 80;
    if (!parseHttpBaseUrl(host, port, pathPrefix)) {
        Serial.println("SIM800L HTTP base URL must be plain http://host[:port][/prefix]");
        return false;
    }

    const String fullPath = pathPrefix + path;
    if (!gsmClient_.connect(host.c_str(), port)) {
        Serial.print("SIM800L TCP connect failed: ");
        Serial.print(host);
        Serial.print(":");
        Serial.println(port);
        return false;
    }

    gsmClient_.print(method);
    gsmClient_.print(" ");
    gsmClient_.print(fullPath.length() > 0 ? fullPath : "/");
    gsmClient_.println(" HTTP/1.1");
    gsmClient_.print("Host: ");
    gsmClient_.println(host);
    gsmClient_.println("User-Agent: esp32-telematics-sim800l");
    gsmClient_.println("Connection: close");
    gsmClient_.print("X-Device-Key: ");
    gsmClient_.println(TELEMATICS_DEVICE_KEY);

    if (strcmp(method, "POST") == 0) {
        gsmClient_.println("Content-Type: application/json");
        gsmClient_.print("Content-Length: ");
        gsmClient_.println(body.length());
        gsmClient_.println();
        gsmClient_.print(body);
    } else {
        gsmClient_.println();
    }

    String raw;
    const unsigned long timeoutAt = millis() + HTTP_TIMEOUT_MS;
    while (gsmClient_.connected() || gsmClient_.available()) {
        while (gsmClient_.available()) {
            raw += static_cast<char>(gsmClient_.read());
        }

        if (static_cast<long>(millis() - timeoutAt) >= 0) {
            break;
        }

        delay(5);
    }
    gsmClient_.stop();

    const int statusLineEnd = raw.indexOf("\r\n");
    if (statusLineEnd < 0) {
        Serial.println("SIM800L HTTP response missing status line");
        return false;
    }

    const String statusLine = raw.substring(0, statusLineEnd);
    const int firstSpace = statusLine.indexOf(' ');
    const int secondSpace = firstSpace >= 0 ? statusLine.indexOf(' ', firstSpace + 1) : -1;
    const int statusCode = firstSpace >= 0
        ? statusLine.substring(firstSpace + 1, secondSpace >= 0 ? secondSpace : statusLine.length()).toInt()
        : 0;

    const int bodyStart = raw.indexOf("\r\n\r\n");
    if (response != nullptr) {
        *response = bodyStart >= 0 ? raw.substring(bodyStart + 4) : "";
    }

    Serial.print("SIM800L HTTP ");
    Serial.print(method);
    Serial.print(" ");
    Serial.print(fullPath);
    Serial.print(" -> ");
    Serial.println(statusCode);

    return statusCode >= 200 && statusCode < 300;
}

bool AppController::parseHttpBaseUrl(String& host, uint16_t& port, String& pathPrefix) const {
    String url = apiBaseUrl();
    if (!url.startsWith("http://")) {
        return false;
    }

    url.remove(0, 7);
    const int slash = url.indexOf('/');
    String authority = slash >= 0 ? url.substring(0, slash) : url;
    pathPrefix = slash >= 0 ? url.substring(slash) : "";
    if (pathPrefix.endsWith("/")) {
        pathPrefix.remove(pathPrefix.length() - 1);
    }

    const int colon = authority.lastIndexOf(':');
    if (colon >= 0) {
        host = authority.substring(0, colon);
        port = static_cast<uint16_t>(authority.substring(colon + 1).toInt());
    } else {
        host = authority;
        port = 80;
    }

    return host.length() > 0 && port > 0;
}
#endif

bool AppController::sendHttpTelemetry(const char* source) {
    StaticJsonDocument<768> doc;
    if (timeSynchronized()) {
        doc["deviceTimestampUtc"] = nowIsoUtc();
    } else {
        doc["deviceTimestampUtc"] = nullptr;
    }

    const GpsSnapshot gps = gps_.snapshot();
    if (gps.fix || gps.stale) {
        doc["latitude"] = gps.latitude;
        doc["longitude"] = gps.longitude;
    } else {
        doc["latitude"] = nullptr;
        doc["longitude"] = nullptr;
    }

    if (gps.hasSpeed) {
        doc["gpsSpeedKmh"] = gps.speedKmh;
    } else {
        doc["gpsSpeedKmh"] = nullptr;
    }
    doc["headingDegrees"] = nullptr;
    if (gps.hasAltitude) {
        doc["altitudeMeters"] = gps.altitudeMeters;
    } else {
        doc["altitudeMeters"] = nullptr;
    }
    doc["satellites"] = gps.satellites;
    doc["hdop"] = nullptr;
    doc["gpsFix"] = gps.fix;
    doc["batteryVoltage"] = readBatteryVoltage();
    if (isnan(vehicle_.fuelPercent)) {
        doc["fuelLevelPercent"] = nullptr;
    } else {
        doc["fuelLevelPercent"] = vehicle_.fuelPercent;
    }
    doc["odometerKm"] = nullptr;
    doc["gsmSignalLevel"] = networkSignalLevel();
    if (isnan(vehicle_.canSpeedKmh)) {
        doc["canSpeedKmh"] = nullptr;
    } else {
        doc["canSpeedKmh"] = vehicle_.canSpeedKmh;
    }
    doc["ignitionOn"] = vehicle_.ignitionOn;
    doc["source"] = source;

    return postHttpJson("/api/telematics/ingest/http", doc);
}

bool AppController::sendHttpHeartbeat() {
    StaticJsonDocument<512> doc;
    if (timeSynchronized()) {
        doc["deviceTimestampUtc"] = nowIsoUtc();
    } else {
        doc["deviceTimestampUtc"] = nullptr;
    }

    doc["firmwareVersion"] = FIRMWARE_VERSION;
    doc["uptimeSeconds"] = millis() / 1000UL;
    doc["freeHeapBytes"] = ESP.getFreeHeap();
    doc["gsmSignalLevel"] = networkSignalLevel();

    return postHttpJson("/api/telematics/heartbeat", doc);
}

void AppController::pollHttpCommands() {
    if (!httpTransportConnected()) {
        return;
    }

#if ENABLE_SIM800_TRANSPORT
    if (UseSim800Transport) {
        String payload;
        if (!sendGsmHttpRequest("GET", "/api/telematics/device/commands/pending", "", &payload)) {
            return;
        }

        StaticJsonDocument<MQTT_BUFFER_SIZE> doc;
        const DeserializationError error = deserializeJson(doc, payload);
        JsonArray commands = doc.as<JsonArray>();
        if (error || commands.isNull()) {
            return;
        }

        for (JsonObject command : commands) {
            handleHttpCommand(command);
        }
        return;
    }
#endif

    HTTPClient http;
    http.setTimeout(HTTP_TIMEOUT_MS);
    const String url = apiBaseUrl() + "/api/telematics/device/commands/pending";
    http.begin(url);
    http.addHeader("X-Device-Key", TELEMATICS_DEVICE_KEY);

    const int statusCode = http.GET();
    if (statusCode < 200 || statusCode >= 300) {
        http.end();
        Serial.printf("HTTP GET %s -> %d\n", url.c_str(), statusCode);
        return;
    }

    const String payload = http.getString();
    http.end();
    Serial.printf("HTTP GET %s -> %d\n", url.c_str(), statusCode);

    StaticJsonDocument<MQTT_BUFFER_SIZE> doc;
    const DeserializationError error = deserializeJson(doc, payload);
    JsonArray commands = doc.as<JsonArray>();
    if (error || commands.isNull()) {
        return;
    }

    for (JsonObject command : commands) {
        handleHttpCommand(command);
    }
}

void AppController::handleHttpCommand(JsonObject doc) {
    CommandEnvelope command;
    const char* commandId = doc["id"] | "";
    if (strlen(commandId) == 0) {
        commandId = doc["Id"] | "";
    }

    const char* commandType = doc["type"] | "";
    if (strlen(commandType) == 0) {
        commandType = doc["Type"] | "";
    }

    command.commandId = commandId;
    command.commandType = commandType;

    if (command.commandId.length() == 0 || command.commandType.length() == 0) {
        return;
    }

    JsonVariantConst payload = doc["payload"];
    const bool success = applyCommand(command, payload);
    acknowledgeHttpCommand(command.commandId, success, success ? nullptr : "Unsupported command type");

    if (success && command.commandType == "RebootDevice") {
        delay(250);
        ESP.restart();
    }
}

void AppController::acknowledgeHttpCommand(const String& commandId, bool success, const char* message) {
    StaticJsonDocument<256> doc;
    doc["success"] = success;
    doc["message"] = message;
    postHttpJson(String("/api/telematics/device/commands/") + commandId + "/ack", doc);
}

void AppController::publishTelemetry(const char* source) {
    StaticJsonDocument<MQTT_BUFFER_SIZE> doc;
    doc["schemaVersion"] = 1;
    doc["messageId"] = nextMessageId("tel", ++telemetrySequence_);
    doc["deviceId"] = DEVICE_ID;
    doc["carId"] = CAR_ID;
    doc["sentAt"] = nowIsoUtc();
    doc["timeSynchronized"] = timeSynchronized();

    const GpsSnapshot gps = gps_.snapshot();
    JsonObject location = doc.createNestedObject("location");
    if (gps.fix || gps.stale) {
        location["lat"] = gps.latitude;
        location["lon"] = gps.longitude;
    } else {
        location["lat"] = nullptr;
        location["lon"] = nullptr;
    }
    location["fix"] = gps.fix;
    location["stale"] = gps.stale;
    location["ageMs"] = gps.ageMs;
    location["accuracyMeters"] = gps.hasAccuracy ? gps.accuracyMeters : 0;
    location["satellites"] = gps.satellites;

    JsonObject vehicle = doc.createNestedObject("vehicle");
    if (gps.hasSpeed) {
        vehicle["speedKmh"] = gps.speedKmh;
    } else if (!isnan(vehicle_.canSpeedKmh)) {
        vehicle["speedKmh"] = vehicle_.canSpeedKmh;
    } else {
        vehicle["speedKmh"] = nullptr;
    }

    if (!isnan(vehicle_.fuelPercent)) {
        vehicle["fuelPercent"] = vehicle_.fuelPercent;
    } else {
        vehicle["fuelPercent"] = nullptr;
    }
    vehicle["ignitionOn"] = vehicle_.ignitionOn;
    vehicle["doorsLocked"] = vehicle_.doorsLocked;
    vehicle["engineBlocked"] = !vehicle_.engineEnabled;

    JsonObject device = doc.createNestedObject("device");
    device["batteryVoltage"] = readBatteryVoltage();
    device["rssi"] = wifiRssi();
    device["uptimeSeconds"] = millis() / 1000UL;
    device["freeHeapBytes"] = ESP.getFreeHeap();
    device["firmwareVersion"] = FIRMWARE_VERSION;
    device["status"] = toString(status_);
    device["source"] = source;

    publishJson(telemetryTopic_, doc);
}

void AppController::publishHeartbeat() {
    StaticJsonDocument<768> doc;
    doc["schemaVersion"] = 1;
    doc["messageId"] = nextMessageId("hb", ++heartbeatSequence_);
    doc["deviceId"] = DEVICE_ID;
    doc["carId"] = CAR_ID;
    doc["sentAt"] = nowIsoUtc();
    doc["timeSynchronized"] = timeSynchronized();
    doc["status"] = toString(status_);
    doc["firmwareVersion"] = FIRMWARE_VERSION;

    JsonObject network = doc.createNestedObject("network");
    network["transport"] = networkTransportName();
    network["rssi"] = UseSim800Transport ? networkSignalLevel() : wifiRssi();
    network["mqttConnected"] = mqtt_.connected();
    network["ipAddress"] = !UseSim800Transport && WiFi.status() == WL_CONNECTED ? WiFi.localIP().toString() : "";

    publishJson(heartbeatTopic_, doc);
}

void AppController::publishDiagnostics(const char* eventType, const char* message) {
    StaticJsonDocument<1024> doc;
    doc["schemaVersion"] = 1;
    doc["messageId"] = nextMessageId("diag", ++diagnosticsSequence_);
    doc["deviceId"] = DEVICE_ID;
    doc["carId"] = CAR_ID;
    doc["sentAt"] = nowIsoUtc();
    doc["timeSynchronized"] = timeSynchronized();
    doc["eventType"] = eventType;
    doc["message"] = message;
    doc["status"] = toString(status_);
    doc["firmwareVersion"] = FIRMWARE_VERSION;
    doc["uptimeSeconds"] = millis() / 1000UL;
    doc["freeHeapBytes"] = ESP.getFreeHeap();
    doc["resetReason"] = static_cast<int>(esp_reset_reason());
    doc["wifiConnected"] = WiFi.status() == WL_CONNECTED;
#if ENABLE_SIM800_TRANSPORT
    doc["sim800Enabled"] = UseSim800Transport;
    doc["sim800NetworkConnected"] = UseSim800Transport ? modem_.isNetworkConnected() : false;
    doc["sim800GprsConnected"] = UseSim800Transport ? modem_.isGprsConnected() : false;
#else
    doc["sim800Enabled"] = false;
    doc["sim800NetworkConnected"] = false;
    doc["sim800GprsConnected"] = false;
#endif
    doc["mqttConnected"] = mqtt_.connected();
    doc["mqttState"] = mqtt_.state();
    doc["gpsEnabled"] = gps_.enabled();
    doc["gpsCharsProcessed"] = gps_.charsProcessed();
    doc["gpsFailedChecksum"] = gps_.failedChecksum();
    doc["canEnabled"] = ENABLE_CAN == 1;
    doc["canStarted"] = can_.started();
    doc["batteryVoltage"] = readBatteryVoltage();

    publishJson(diagnosticsTopic_, doc);
}

void AppController::publishCommandResult(const CommandEnvelope& command, const char* status) {
    StaticJsonDocument<768> doc;
    doc["schemaVersion"] = 1;
    doc["messageId"] = nextMessageId("res", ++commandResultSequence_);
    doc["commandId"] = command.commandId;
    doc["correlationId"] = command.correlationId;
    doc["deviceId"] = DEVICE_ID;
    doc["carId"] = CAR_ID;
    doc["sentAt"] = nowIsoUtc();
    doc["timeSynchronized"] = timeSynchronized();
    doc["status"] = status;

    JsonObject result = doc.createNestedObject("result");
    result["doorsLocked"] = vehicle_.doorsLocked;
    result["engineBlocked"] = !vehicle_.engineEnabled;
    result["telemetryIntervalMs"] = telemetryIntervalMs_;

    publishJson(commandResultTopic_, doc);
}

void AppController::publishCommandError(const CommandEnvelope& command, const char* code, const char* message) {
    StaticJsonDocument<768> doc;
    doc["schemaVersion"] = 1;
    doc["messageId"] = nextMessageId("res", ++commandResultSequence_);
    doc["commandId"] = command.commandId.length() == 0 ? "unknown" : command.commandId;
    doc["correlationId"] = command.correlationId;
    doc["deviceId"] = DEVICE_ID;
    doc["carId"] = CAR_ID;
    doc["sentAt"] = nowIsoUtc();
    doc["timeSynchronized"] = timeSynchronized();
    doc["status"] = "Failed";

    JsonObject error = doc.createNestedObject("error");
    error["code"] = code;
    error["message"] = message;

    publishJson(commandResultTopic_, doc);
    publishDiagnostics("CommandFailed", code);
}

bool AppController::applyCommand(const CommandEnvelope& command, JsonVariantConst payload) {
    if (command.commandType == "Ping") {
        if (UseHttpTransport) {
            sendHttpHeartbeat();
        } else {
            publishHeartbeat();
        }
    } else if (command.commandType == "GetState") {
        if (UseHttpTransport) {
            sendHttpTelemetry("Device");
        } else {
            publishTelemetry("Device");
        }
    } else if (command.commandType == "OpenLock" || command.commandType == "UnlockDoors") {
        vehicleIo_.unlockDoors(vehicle_);
    } else if (command.commandType == "CloseLock" || command.commandType == "LockDoors") {
        vehicleIo_.lockDoors(vehicle_);
    } else if (command.commandType == "EnableEngine") {
        vehicleIo_.enableEngine(vehicle_);
    } else if (command.commandType == "DisableEngine") {
        vehicleIo_.disableEngine(vehicle_);
    } else if (command.commandType == "SetTelemetryInterval") {
        unsigned long requested = telemetryIntervalMs_;
        if (!payload.isNull()) {
            if (payload["intervalMs"].is<unsigned long>()) {
                requested = payload["intervalMs"].as<unsigned long>();
            } else if (payload["intervalSeconds"].is<unsigned long>()) {
                requested = payload["intervalSeconds"].as<unsigned long>() * 1000UL;
            }
        }

        telemetryIntervalMs_ = constrain(requested, MIN_TELEMETRY_INTERVAL_MS, MAX_TELEMETRY_INTERVAL_MS);
        preferences_.putULong("telInterval", telemetryIntervalMs_);
    } else if (command.commandType == "RequestDiagnostics") {
        if (UseHttpTransport) {
            sendHttpHeartbeat();
            sendHttpTelemetry("Diagnostics");
        } else {
            publishDiagnostics("Requested", "Diagnostics requested by backend");
        }
    } else if (command.commandType == "RebootDevice") {
        return true;
    } else {
        return false;
    }

    return true;
}

void AppController::handleCommand(JsonDocument& doc) {
    CommandEnvelope command;
    command.commandId = doc["commandId"] | "unknown";
    command.correlationId = doc["correlationId"] | "";
    command.commandType = doc["commandType"] | "";

    if (!targetMatches(doc)) {
        publishCommandError(command, "WrongTarget", "Command target does not match this device");
        return;
    }

    if (command.commandId == "unknown" || command.commandType.length() == 0) {
        publishCommandError(command, "InvalidCommand", "commandId and commandType are required");
        return;
    }

    if (isDuplicateCommand(command.commandId)) {
        publishCommandResult(command, "Succeeded");
        return;
    }

    if (commandExpired(doc)) {
        publishCommandError(command, "CommandExpired", "Command expiration time has passed");
        rememberCommand(command.commandId);
        return;
    }

    JsonVariantConst payload = doc["payload"];
    if (applyCommand(command, payload)) {
        publishCommandResult(command, "Succeeded");
        if (command.commandType == "RebootDevice") {
            rememberCommand(command.commandId);
            delay(250);
            ESP.restart();
        }
    } else {
        publishCommandError(command, "UnknownCommand", "Command type is not supported");
        rememberCommand(command.commandId);
        return;
    }

    rememberCommand(command.commandId);
}

bool AppController::targetMatches(JsonDocument& doc) const {
    const char* targetDeviceId = doc["deviceId"] | "";
    const char* targetCarId = doc["carId"] | "";

    if (strlen(targetDeviceId) > 0 && strcmp(targetDeviceId, DEVICE_ID) != 0) {
        return false;
    }

    if (strlen(targetCarId) > 0 && strcmp(targetCarId, CAR_ID) != 0) {
        return false;
    }

    return true;
}

bool AppController::isDuplicateCommand(const String& commandId) const {
    for (uint8_t i = 0; i < COMMAND_DEDUP_CACHE_SIZE; i++) {
        if (dedupCache_[i] == commandId) {
            return true;
        }
    }

    return false;
}

void AppController::rememberCommand(const String& commandId) {
    dedupCache_[dedupCursor_] = commandId;
    dedupCursor_ = (dedupCursor_ + 1) % COMMAND_DEDUP_CACHE_SIZE;
}

bool AppController::commandExpired(JsonDocument& doc) const {
    const char* expiresAt = doc["expiresAt"] | "";
    if (strlen(expiresAt) == 0 || !timeSynchronized()) {
        return false;
    }

    time_t expiration = 0;
    if (!parseIsoUtc(expiresAt, expiration)) {
        return true;
    }

    return time(nullptr) > expiration;
}

bool AppController::parseIsoUtc(const char* value, time_t& parsed) const {
    int year = 0;
    int month = 0;
    int day = 0;
    int hour = 0;
    int minute = 0;
    int second = 0;

    if (sscanf(value, "%d-%d-%dT%d:%d:%dZ", &year, &month, &day, &hour, &minute, &second) != 6) {
        return false;
    }

    tm timeInfo {};
    timeInfo.tm_year = year - 1900;
    timeInfo.tm_mon = month - 1;
    timeInfo.tm_mday = day;
    timeInfo.tm_hour = hour;
    timeInfo.tm_min = minute;
    timeInfo.tm_sec = second;
    parsed = mktime(&timeInfo);
    return parsed > 0;
}

bool AppController::timeSynchronized() const {
    return time(nullptr) > 1700000000;
}

String AppController::nowIsoUtc() const {
    if (timeSynchronized()) {
        time_t now = time(nullptr);
        tm timeInfo {};
        gmtime_r(&now, &timeInfo);
        char buffer[25];
        strftime(buffer, sizeof(buffer), "%Y-%m-%dT%H:%M:%SZ", &timeInfo);
        return String(buffer);
    }

    return String("uptime-ms-") + String(millis());
}

String AppController::nextMessageId(const char* prefix, uint32_t sequence) const {
    return String(prefix) + "-" + DEVICE_ID + "-" + String(sequence);
}

float AppController::readBatteryVoltage() const {
    const int raw = analogRead(BATTERY_ADC_PIN);
    return (raw / ADC_MAX_VALUE) * ADC_REFERENCE_VOLTAGE * BATTERY_ADC_MULTIPLIER;
}

int AppController::wifiRssi() const {
    return WiFi.status() == WL_CONNECTED ? WiFi.RSSI() : 0;
}

bool AppController::httpTransportConnected() {
#if ENABLE_SIM800_TRANSPORT
    if (UseSim800Transport) {
        return sim800Started_ && modem_.isNetworkConnected() && modem_.isGprsConnected();
    }
#endif

    return WiFi.status() == WL_CONNECTED;
}

int AppController::networkSignalLevel() {
#if ENABLE_SIM800_TRANSPORT
    if (UseSim800Transport) {
        const int quality = modem_.getSignalQuality();
        return quality >= 0 ? constrain(quality, 0, 31) : 0;
    }
#endif

    return constrain(map(wifiRssi(), -100, -50, 0, 31), 0, 31);
}

String AppController::networkTransportName() const {
    return UseSim800Transport ? "SIM800L" : "WiFi";
}

void AppController::setStatus(DeviceStatus status) {
    status_ = status;
}

}

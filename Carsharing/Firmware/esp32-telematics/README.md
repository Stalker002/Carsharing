# ESP32 Telematics Firmware

Прошивка для ESP32 DevKit V4 под будущий `Telematics/IoT Service` в микросервисной версии Carsharing.

## Что умеет

- подключается к Wi-Fi;
- может отправлять HTTP-телеметрию через SIM800L/GPRS;
- работает с текущим `TelematicsService` по HTTP или с MQTT broker через переключатель конфигурации;
- в HTTP-режиме отправляет telemetry packet в `POST /api/telematics/ingest/http`;
- в HTTP-режиме отправляет heartbeat в `POST /api/telematics/heartbeat`;
- в HTTP-режиме забирает pending commands из `GET /api/telematics/device/commands/pending`;
- в MQTT-режиме отправляет telemetry packet в topic `cars/{carId}/telemetry`;
- в MQTT-режиме отправляет heartbeat в topic `cars/{carId}/heartbeat`;
- в MQTT-режиме принимает команды из `cars/{carId}/commands`;
- в MQTT-режиме публикует результат команд в `cars/{carId}/command-results`;
- в MQTT-режиме публикует диагностику в `cars/{carId}/diagnostics`;
- читает GPS через NEO-6M по UART;
- читает CAN/TWAI через SN65HVD230 и опрашивает стандартные OBD-II PID'ы;
- управляет GPIO центрального замка по схеме питания ESP32;
- проверяет целевой `deviceId`/`carId`, `expiresAt` и дубли `commandId`;
- сохраняет измененный интервал телеметрии в NVS.

## HTTP-режим для текущего TelematicsService

По умолчанию прошивка настроена на HTTP-режим, потому что текущий backend MVP принимает телеметрию через REST API.

В `include/config.h` нужно задать:

```cpp
#define TELEMATICS_TRANSPORT_HTTP 1
#define TELEMATICS_API_BASE_URL "http://192.168.1.10:5007"
#define TELEMATICS_API_USE_WIFI_GATEWAY 0
#define TELEMATICS_API_GATEWAY_PORT 5007
#define TELEMATICS_DEVICE_KEY "esp32-test-001"
```

`TELEMATICS_DEVICE_KEY` должен совпадать с `deviceKey`, с которым устройство зарегистрировано в `TelematicsService`.

Если Docker с `TelematicsService` доступен через IP Wi-Fi роутера, можно включить автоматическую отправку на gateway адрес сети:

```cpp
#define TELEMATICS_API_USE_WIFI_GATEWAY 1
#define TELEMATICS_API_GATEWAY_PORT 5007
```

В этом режиме ESP32 отправляет HTTP-запросы на `http://{WiFi.gatewayIP()}:5007`. Если Docker запущен на компьютере без проброса порта на роутере, вместо этого оставьте `TELEMATICS_API_USE_WIFI_GATEWAY 0` и укажите IP компьютера в `TELEMATICS_API_BASE_URL`.

Чтобы отправлять те же HTTP-запросы через SIM800L вместо Wi-Fi, включите:

```cpp
#define TELEMATICS_TRANSPORT_HTTP 1
#define ENABLE_SIM800_TRANSPORT 1
#define SIM800_RX_PIN 27
#define SIM800_TX_PIN 26
#define SIM800_APN "internet.life.com.by"
```

Подмена IMEI в прошивке не выполняется. Текущий IMEI SIM800L выводится в Serial Monitor при старте модема.

HTTP endpoints:

```text
POST /api/telematics/ingest/http
POST /api/telematics/heartbeat
GET  /api/telematics/device/commands/pending
POST /api/telematics/device/commands/{commandId}/ack
```

Все запросы устройства отправляют заголовок:

```text
X-Device-Key: {TELEMATICS_DEVICE_KEY}
```

Поддерживаемый маппинг команд HTTP API:

| Backend command | Действие на ESP32 |
| --- | --- |
| `Ping` | Отправить heartbeat |
| `OpenLock` | Открыть двери |
| `CloseLock` | Закрыть двери |
| `SetTelemetryInterval` | Изменить интервал телеметрии |
| `RequestDiagnostics` | Отправить heartbeat и telemetry |
| `RebootDevice` | Перезагрузить ESP32 после ack |

Для `SetTelemetryInterval` прошивка принимает `payload.intervalSeconds` от backend и также поддерживает `payload.intervalMs` для совместимости с MQTT-контрактом.

## Контракт MQTT

Чтобы использовать MQTT вместо HTTP, установите:

```cpp
#define TELEMATICS_TRANSPORT_HTTP 0
```

Topics:

```text
cars/{carId}/telemetry
cars/{carId}/heartbeat
cars/{carId}/commands
cars/{carId}/command-results
cars/{carId}/diagnostics
```

Пример команды:

```json
{
  "schemaVersion": 1,
  "commandId": "cmd-001",
  "correlationId": "corr-001",
  "deviceId": "esp32-car-001",
  "carId": "car-001",
  "commandType": "UnlockDoors",
  "payload": {},
  "createdAt": "2026-05-23T12:30:00Z",
  "expiresAt": "2026-05-23T12:31:00Z"
}
```

Поддерживаемые `commandType`:

- `GetState`
- `LockDoors`
- `UnlockDoors`
- `EnableEngine`
- `DisableEngine`
- `SetTelemetryInterval`
- `RebootDevice`

Для `SetTelemetryInterval` payload:

```json
{
  "schemaVersion": 1,
  "commandId": "cmd-interval-001",
  "correlationId": "corr-interval-001",
  "deviceId": "esp32-car-001",
  "carId": "car-001",
  "commandType": "SetTelemetryInterval",
  "expiresAt": "2026-05-23T12:31:00Z",
  "payload": {
    "intervalMs": 5000
  }
}
```

## Настройка

1. Установить PlatformIO.
2. Скопировать `include/config.example.h` в `include/config.h`.
3. Заполнить Wi-Fi, `DEVICE_ID`, `CAR_ID` и параметры выбранного транспорта.
4. Подключить ESP32.
5. Выполнить:

```powershell
pio run -t upload
pio device monitor
```

`include/config.h` не должен попадать в git, потому что там могут быть адреса и пароли.

Если `pio` не найден в PowerShell, установите PlatformIO CLI или откройте проект через PlatformIO extension в VS Code.

## Формат telemetry

Telemetry публикуется как JSON со схемой верхнего уровня:

```json
{
  "schemaVersion": 1,
  "messageId": "tel-esp32-car-001-1",
  "deviceId": "esp32-car-001",
  "carId": "car-001",
  "sentAt": "2026-05-23T12:30:00Z",
  "timeSynchronized": true,
  "location": {
    "lat": 53.9023,
    "lon": 27.5619,
    "fix": true,
    "stale": false,
    "ageMs": 120,
    "accuracyMeters": 8.5,
    "satellites": 9
  },
  "vehicle": {
    "speedKmh": 42.5,
    "fuelPercent": 63,
    "ignitionOn": true,
    "doorsLocked": true,
    "engineBlocked": false
  },
  "device": {
    "batteryVoltage": 12.4,
    "rssi": -58,
    "uptimeSeconds": 3600,
    "freeHeapBytes": 120000,
    "firmwareVersion": "0.1.0",
    "status": "Online",
    "source": "Mixed"
  }
}
```

## Пины по умолчанию

| Назначение | GPIO |
| --- | --- |
| GPS RX | 17 |
| GPS TX | 16 |
| SIM800 RX | 27 |
| SIM800 TX | 26 |
| Lock relay / central lock IN1 | 18 |
| Hazard relay IN2 | 19 |
| Engine relay | disabled |
| Status LED | 2 |
| Battery ADC | 34 |
| CAN TX | 4 |
| CAN RX | 5 |

## GPS и CAN/TWAI

Прошивка автоматически вызывает `gps_.update()` и `can_.update(vehicle_)` в основном loop. В HTTP-режиме каждые `TELEMETRY_INTERVAL_MS` миллисекунд она отправляет на `POST /api/telematics/ingest/http`:

- координаты GPS: `latitude`, `longitude`, `gpsSpeedKmh`, `altitudeMeters`, `satellites`, `gpsFix`;
- CAN/OBD-II данные: `canSpeedKmh`, `fuelLevelPercent`, `ignitionOn`;
- состояние устройства: напряжение батареи, уровень сигнала, uptime.

По умолчанию CAN включен под SN65HVD230 из аппаратной схемы:

```cpp
#define ENABLE_CAN 1
```

Чтобы временно отключить SN65HVD230 на стенде без CAN, в `include/config.h` нужно поставить:

```cpp
#define ENABLE_CAN 0
```

Для реального автомобиля включен OBD-II polling:

```cpp
#define CAN_OBD_POLL_ENABLED 1
#define CAN_OBD_POLL_INTERVAL_MS 1000UL
#define CAN_OBD_REQUEST_ID 0x7DF
#define CAN_OBD_RESPONSE_ID_MIN 0x7E8
#define CAN_OBD_RESPONSE_ID_MAX 0x7EF
```

Прошивка опрашивает:

- PID `0x0D` - скорость автомобиля, `canSpeedKmh`;
- PID `0x2F` - уровень топлива, `fuelLevelPercent`;
- PID `0x0C` - обороты двигателя, используется для `ignitionOn`.

Для стенда без автомобиля можно включить демо-расшифровку CAN-фрейма:

- frame id `0x100`;
- byte `0`: скорость, км/ч;
- byte `1`: уровень топлива, проценты;
- byte `2`, bit `0`: зажигание включено.

```cpp
#define CAN_SELF_TEST_ENABLED 1
```

Для реального автомобиля `CAN_SELF_TEST_ENABLED` должен быть `0`, иначе в телеметрию будут попадать искусственные тестовые значения. Если конкретная машина не отвечает на стандартные OBD-II PID'ы, mapping нужно заменить под ее CAN ID.

## Роль в микросервисной архитектуре

ESP32 не должен напрямую вызывать Fleet, Trip, Billing или Fine Service. Он отправляет техническую телеметрию в `Telematics/IoT Service`, а уже сервис публикует события:

- `TelemetryReceived`;
- `DeviceHeartbeatReceived`;
- `VehicleCommandSucceeded`;
- `VehicleCommandFailed`;
- `VehicleSpeedLimitExceeded`;
- `HarshAccelerationDetected`;
- `HarshBrakingDetected`.

Так граница устройства остается простой: устройство передает факты и исполняет команды, а бизнес-решения остаются на стороне backend.

## Что добавить позже

- TLS и device certificates для MQTT;
- TLS/HTTPS для SIM800L-транспорта;
- OTA firmware update;
- подпись сообщений или HMAC;
- persistent offline buffer для телеметрии.

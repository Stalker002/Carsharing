# ESP32 Telematics Arduino Sketch

Это Arduino IDE версия прошивки для ESP32 DevKit V4. Открывать нужно файл:

```text
Firmware/esp32-telematics-arduino/esp32_telematics/esp32_telematics.ino
```

## Библиотеки Arduino IDE

Установить через `Sketch -> Include Library -> Manage Libraries`:

- `PubSubClient` by Nick O'Leary;
- `ArduinoJson` by Benoit Blanchon;
- `TinyGPSPlus` by Mikal Hart.

Плата:

- `Tools -> Board -> ESP32 Arduino -> ESP32 Dev Module`.

## Настройка

В `Config.h` изменить:

- `DEVICE_ID`;
- `CAR_ID`;
- `WIFI_SSID`;
- `WIFI_PASSWORD`;
- `MQTT_HOST`;
- `MQTT_PORT`;
- `MQTT_USERNAME` и `MQTT_PASSWORD`, если broker требует авторизацию.

После этого нажать `Upload`.

## MQTT topics

Устройство публикует:

```text
cars/{carId}/telemetry
cars/{carId}/heartbeat
cars/{carId}/diagnostics
cars/{carId}/command-results
devices/{deviceId}/availability
```

Устройство слушает:

```text
cars/{carId}/commands
devices/{deviceId}/commands
```

## Команды

```json
{
  "commandId": "cmd-001",
  "deviceId": "esp32-car-001",
  "carId": "car-001",
  "commandType": "UnlockDoors",
  "payload": {},
  "createdAt": "2026-05-22T18:00:00Z"
}
```

Поддерживаются:

- `GetState`
- `GetDiagnostics`
- `LockDoors`
- `UnlockDoors`
- `EnableEngine`
- `DisableEngine`
- `SetTelemetryInterval`
- `SetTripContext`
- `ClearTripContext`
- `RebootDevice`

`SetTelemetryInterval`:

```json
{
  "commandId": "cmd-interval-001",
  "deviceId": "esp32-car-001",
  "commandType": "SetTelemetryInterval",
  "payload": {
    "intervalMs": 5000
  }
}
```

`SetTripContext`:

```json
{
  "commandId": "cmd-trip-001",
  "deviceId": "esp32-car-001",
  "commandType": "SetTripContext",
  "payload": {
    "tripId": "trip-123"
  }
}
```

## Аппаратные пины

| Назначение | GPIO |
| --- | --- |
| GPS RX | 17 |
| GPS TX | 16 |
| SIM800 RX | 27 |
| SIM800 TX | 26 |
| Lock relay / central lock IN1 | 18 |
| Unlock relay | 18 |
| Hazard relay IN2 | 19 |
| Engine relay | disabled |
| Status LED | 2 |
| Battery ADC | 34 |
| CAN TX | 4 |
| CAN RX | 5 |

## CAN

CAN включен по умолчанию под SN65HVD230 из аппаратной схемы. Для стенда без CAN поставить в `Config.h`:

```cpp
#define ENABLE_CAN 0
```

Демо-маппинг CAN:

- id `0x100`;
- byte `0`: скорость км/ч;
- byte `1`: топливо в процентах;
- byte `2`, bit `0`: зажигание.

Для реального автомобиля CAN ID нужно заменить под конкретную модель или тестовый стенд.

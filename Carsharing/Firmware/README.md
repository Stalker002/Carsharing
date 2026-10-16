# Firmware

В этой папке лежат варианты прошивки для ESP32 телематики.

## PlatformIO

Основной вариант для разработки и проверки:

```text
esp32-telematics/
```

Откройте папку в PlatformIO, скопируйте `include/config.example.h` в
`include/config.h`, настройте Wi-Fi/MQTT и выполните сборку или загрузку.

## Arduino IDE

Вариант для открытия через Arduino IDE оставлен как reference/legacy:

```text
esp32-telematics-arduino/esp32_telematics/esp32_telematics.ino
```

Откройте этот `.ino` файл, настройте `Config.h`, выберите плату `ESP32 Dev Module` и загрузите прошивку.

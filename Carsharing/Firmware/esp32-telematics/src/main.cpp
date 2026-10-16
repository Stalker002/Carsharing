#include <Arduino.h>
#include "AppController.h"

namespace {
Telematics::AppController app;
}

void setup() {
    app.setup();
}

void loop() {
    app.loop();
}

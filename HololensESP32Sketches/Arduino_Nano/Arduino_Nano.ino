#include "DFRobot_PH.h"
#include <EEPROM.h>

#define PH_PIN A1
#define LED_PIN 1
float voltage, phValue, temperature = 25;
DFRobot_PH ph;

void setup() {
  Serial.begin(115200);
  pinMode(LED_PIN, OUTPUT);
  ph.begin();
}

void loop() {
  voltage = analogRead(PH_PIN) / 1024.0 * 5000;
  phValue = ph.readPH(voltage, temperature);

  Serial.println(phValue);
  digitalWrite(LED_PIN, HIGH);
  ph.calibration(voltage,temperature);
  
  delay(2500);
}

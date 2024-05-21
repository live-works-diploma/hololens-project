#include "MoistureSensor.h"
#include <Arduino.h>

MoistureSensor::MoistureSensor(int pin) {
  _pin = pin;
  pinMode(_pin, INPUT);
}

int MoistureSensor::readMoisturePercentage() {
  int soilMoistureValue = analogRead(_pin);

  //Debug check value
  Serial.print("Analog Read Value: ");
  Serial.println(soilMoistureValue);

  int moisturePercentage = (100.00 - (soilMoistureValue / 1023.00) * 100.00);
  
  if (moisturePercentage < 0) {
    moisturePercentage = 0;
  }
  
  return moisturePercentage;
}
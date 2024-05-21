#ifndef MOISTURESENSOR_H
#define MOISTURESENSOR_H

#include <Arduino.h>

class MoistureSensor {
public:
  MoistureSensor(int pin);
  int readMoisturePercentage();

private:
  int _pin;
};

#endif
#ifndef FLOWMETER_H
#define FLOWMETER_H

#include <Arduino.h>

class FlowMeter {
  public:
    FlowMeter(int pin, float pulsesPerLiter);
    void update();
    float getFlowRate();
    bool isUnderThreshold(float threshold);

  private:
    int _flowMeterPin;
    float _pulsesPerLiter;
    volatile unsigned long _pulseCount;
    unsigned long _lastPrintTime;
    int _flowState;
    int _lastFlowState;
};

#endif

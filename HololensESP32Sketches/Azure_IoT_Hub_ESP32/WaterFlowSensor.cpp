#include "flowMeter.h"

FlowMeter::FlowMeter(int pin, float pulsesPerLiter) {
  _flowMeterPin = pin;
  _pulsesPerLiter = pulsesPerLiter;
  _pulseCount = 0;
  _lastPrintTime = 0;
  _flowState = 0;
  _lastFlowState = 0;
}

void FlowMeter::update() {
  unsigned long currentTime = millis();

  _flowState = digitalRead(_flowMeterPin);

  // compare the flowState to its previous state
  if (_flowState != _lastFlowState) {
    // if the state has changed, increment the counter
    _pulseCount++;
  }

  // Print flow rate every 2 seconds
  if (currentTime - _lastPrintTime >= 2000) {
    unsigned long elapsedTime = currentTime - _lastPrintTime;
    _lastPrintTime = currentTime;

    // Prevent division by zero
    if (elapsedTime == 0) {
      elapsedTime = 1; // Set to a small non-zero value to avoid division by zero
    }

    // save the current state as the last state, for next time through the loop
    _lastFlowState = _flowState;

    // Calculate flow rate in liters per minute
    float flowRate = 0.0;
    if (_pulseCount > 0) {
      flowRate = ((_pulseCount / _pulsesPerLiter) / (elapsedTime / 1000));
      flowRate = flowRate / 60; // Convert from L/s to L/m
    }

    _pulseCount = 0; // Reset pulse count for the next interval
  }
}

float FlowMeter::getFlowRate() {
  // Calculate flow rate in liters per minute
  float flowRate = 0.0;
  if (_pulseCount > 0) {
    flowRate = ((_pulseCount / _pulsesPerLiter) / (2000 / 1000));
    flowRate = flowRate / 60; // Convert from L/s to L/m
  }
  return flowRate;
}

bool FlowMeter::isUnderThreshold(float threshold) {
  return (getFlowRate() < threshold);
}

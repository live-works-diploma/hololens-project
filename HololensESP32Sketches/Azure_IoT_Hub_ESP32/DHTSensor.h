// DHTSensor.h

#ifndef DHTSENSOR_H
#define DHTSENSOR_H

#include <Arduino.h>
#include <DHT.h>

class DHTSensor {
public:
    DHTSensor(int pin, int type);
    void begin();
    String fetchTempAndHumidityData();
    String fetchTemperatureData();
    String fetchHumidityData();

  
private:
    DHT dht;
};

#endif // DHTSENSOR_H
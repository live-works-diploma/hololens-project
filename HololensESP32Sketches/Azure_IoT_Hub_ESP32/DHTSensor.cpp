// DHTSensor.cpp

#include "DHTSensor.h"

DHTSensor::DHTSensor(int pin, int type) : dht(pin, type) {}

void DHTSensor::begin() {
    dht.begin();
}

String DHTSensor::fetchTempAndHumidityData() {
    String tempAndHumidity = "";
    float h = dht.readHumidity(); // Get Humidity
    float t = dht.readTemperature(); // Get Temperature (Celsius)
    
    // Check for any failure to read Sensor and Exit
    if (isnan(h) || isnan(t)) {
        Serial.println("Failed to read from DHT sensor!");
        return tempAndHumidity;
    }
    tempAndHumidity = " Humidity: " + String(h, 1) + " Temperature: " + String(t, 1);
    
    return tempAndHumidity;
}
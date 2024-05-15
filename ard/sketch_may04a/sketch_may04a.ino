#include "ph_pump.h"
#include "logger.h"

// Define the logger globally
Logger logger;

int phSensorPin = A0; // Use A0 for analog input on Arduino

float wantedSpeed = 10; // if 10 will make clockwise speed 100 and anticlock wise speed 80 since 90 is stop

// time is in miliseconds
const float maxTime = 3;
const float maxIncrease = 1; 
const float defaultTime = 1;

PhSensor phSensor(phSensorPin, logger, wantedSpeed, maxTime, maxIncrease, defaultTime);

void setup() {
    Serial.begin(9600); // Initialize serial communication
    logger.setLogLevel(LogLevel::INFO);
    logger.enableLogging(true);

    for (int i = 0; i < 3; ++i) {
        float value = (float)i + 1;
        phSensor.levelOutPhLevel(value);
        delay(1000);
    }
}

void loop() {

}

float findPhLevelRandom() {
    // Use analogRead to simulate a pH sensor reading
    int sensorValue = analogRead(A0); // Read from analog pin A0
    float phLevel = map(sensorValue, 0, 1023, 1, 14); // Map the sensor value to a pH level (1 to 14)
    logger.log(LogLevel::INFO, "Ph Level:", phLevel);
    return phLevel;
}

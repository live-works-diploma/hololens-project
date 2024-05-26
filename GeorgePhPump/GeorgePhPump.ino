#include "ph_pump.h"
#include "logger.h"

// Define the logger globally
Logger logger;
int phSensorPin = 9;
float wantedSpeed = 10;

unsigned long lastRunTime = 0;
unsigned long interval = 15000; // 15 seconds in milliseconds

// time is in miliseconds
const float maxTime = 5;
const float maxIncrease = 0.5; 
const float defaultTime = 1;

GravityPump pump;
bool firstLoop = true;

PhSensor phSensor(phSensorPin, logger, wantedSpeed, maxTime, maxIncrease, defaultTime, pump);

void setup() {
    pump.setPin(9);
    pump.getFlowRateAndSpeed();    
    Serial.begin(9600); // Initialize serial communication    
    
    logger.setLogLevel(LogLevel::INFO);
    logger.enableLogging(true);
}

float ph = 1;
void loop() {
    pump.update();
    unsigned long currentTime = millis(); // Get the current time in milliseconds

    if (!firstLoop && phSensor.firstReading) {
        // stops next if statement from running since ph pump code is finished. firstReading starts off true and switches when 
        // finds ph levels are out of safe zone and then goes back when it gets a reading of it in the safe zone. Can be triggered to put device to sleep when
        // this if statement is entered
    }

    else if (currentTime - lastRunTime >= interval) {
        lastRunTime = currentTime;

        unsigned long value = (unsigned long)phSensor.levelOutPhLevel(ph);
        logger.log(LogLevel::WARNING, "value entered to pump:", value);

        pump.timerPump(value);
        pump.flowPump(100);

        ph += 1;
    }
}

float findPhLevelRandom() {
    // Use analogRead to simulate a pH sensor reading
    int sensorValue = analogRead(A0); // Read from analog pin A0
    float phLevel = map(sensorValue, 0, 1023, 1, 14); // Map the sensor value to a pH level (1 to 14)
    logger.log(LogLevel::INFO, "Ph Level:", phLevel);
    return phLevel;
}

//#include "GravityPump.h"
//
//GravityPump pump;
//bool run = true;
//int debug = 0;
//
//void setup()
//{
//    pump.setPin(9);
//    Serial.begin(9600);
//    pump.getFlowRateAndSpeed();
//
//    Serial.println(pump.timerPump(120000));
//    Serial.println(pump.flowPump(100));
//}
//
//void loop()
//{
//    pump.update();
//}

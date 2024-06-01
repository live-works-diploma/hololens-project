#include "ph_pump.h"
#include "logger.h"

#include "DFRobot_PH.h"
#include <EEPROM.h>

#define PH_PIN A1
#define LED_PIN 1
float voltage, phValue, temperature = 25;
DFRobot_PH phSensorDF;

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
  Serial.begin(9600);

  pinMode(LED_PIN, OUTPUT);
  phSensorDF.begin();

  pump.setPin(9);
  pump.getFlowRateAndSpeed();  

  logger.setLogLevel(LogLevel::INFO);
  logger.enableLogging(true);
}

float ph = 1;
void loop() {
    pump.update();
    unsigned long currentTime = millis(); // Get the current time in milliseconds

    // if (phSensor.phLevelError) {
    //     // was an error with leveling out ph levels
    //     logger.log(LogLevel::ERROR, "Error Leveling out pH levels");
    // }
    if (!firstLoop && phSensor.firstReading) {
        // stops next if statement from running since ph pump code is finished. firstReading starts off true and switches when 
        // finds ph levels are out of safe zone and then goes back when it gets a reading of it in the safe zone. Can be triggered to put device to sleep when
        // this if statement is entered
        // firstLoop = true;
        logger.log(LogLevel::INFO, "Finished with pump", "\n");
        logger.log(LogLevel::INFO, "pH Sensor First Reading:", phSensor.firstReading);
    }   
    else if (currentTime - lastRunTime >= interval) {
        lastRunTime = currentTime;
        voltage = analogRead(PH_PIN) / 1024.0 * 5000;
        phValue = phSensorDF.readPH(voltage, temperature);

        Serial.println(temperature);
        Serial.println(phValue);
        digitalWrite(LED_PIN, HIGH);

        unsigned long value = (unsigned long)phSensor.levelOutPhLevel(phValue);
        logger.log(LogLevel::WARNING, "value entered to pump:", value);

        pump.timerPump(value);
        pump.flowPump(100);

        ph += 1;

        firstLoop = false;
    }
}

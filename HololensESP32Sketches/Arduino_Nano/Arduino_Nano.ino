#include "ph_pump.h"
#include "logger.h"

#include <OneWire.h>

#include "DFRobot_PH.h"
#include <EEPROM.h>

#define PH_PIN A1
float voltage, phValue, temperature = 25;

DFRobot_PH phSensorDF;

// Define the logger globally
Logger logger;
int tempSensorPin = 8;
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
OneWire ds(tempSensorPin);

void setup() {
  Serial.begin(9600);
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

        temperature = 20.0f;
        voltage = analogRead(PH_PIN) / 1024.0 * 5000;
        phValue = phSensorDF.readPH(voltage, temperature);

        Serial.println(temperature);
        Serial.println(phValue);

        unsigned long value = (unsigned long)phSensor.levelOutPhLevel(phValue);
        logger.log(LogLevel::WARNING, "value entered to pump:", value);

        pump.timerPump(value);
        pump.flowPump(100);

        ph += 1;

        firstLoop = false;
    }
}

float getTemp(){
  //returns the temperature from one DS18S20 in DEG Celsius

  byte data[12];
  byte addr[8];

  if ( !ds.search(addr)) {
      //no more sensors on chain, reset search
      ds.reset_search();
      return -1000;
  }

  if ( OneWire::crc8( addr, 7) != addr[7]) {
      Serial.println("CRC is not valid!");
      return -1000;
  }

  if ( addr[0] != 0x10 && addr[0] != 0x28) {
      Serial.print("Device is not recognized");
      return -1000;
  }

  ds.reset();
  ds.select(addr);
  ds.write(0x44,1); // start conversion, with parasite power on at the end

  byte present = ds.reset();
  ds.select(addr);    
  ds.write(0xBE); // Read Scratchpad

  
  for (int i = 0; i < 9; i++) { // we need 9 bytes
    data[i] = ds.read();
  }
  
  ds.reset_search();
  
  byte MSB = data[1];
  byte LSB = data[0];

  float tempRead = ((MSB << 8) | LSB); //using two's compliment
  float TemperatureSum = tempRead / 16;
  
  return TemperatureSum;
  
}

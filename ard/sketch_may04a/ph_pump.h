#ifndef ARDUINO_PH_SENSOR_H
#define ARDUINO_PH_SENSOR_H

#include "logger.h"

class PhSensor {
public:
    const int pin;
    Logger& logger;

    float minPhLevel = 5.8;
    float maxPhLevel = 6.4;

    PhSensor(int pinNumber, Logger& logger, float speed, float maxTime, float maxIncreaseInTime, float defaultTimeToRun)
    : pin(pinNumber), logger(logger), maxTimeToRun(maxTime), maxTimeIncreaseToRun(maxIncreaseInTime), defaultTimeToRun(defaultTimeToRun) {
        if (speed < 0) {
            logger.log(LogLevel::WARNING, "Speed was less than 0, changing to 0");
            speed = 0;
        }

        increaseSpeed = stopSpeed + speed;
        decreaseSpeed = stopSpeed - speed;
    }

    void levelOutPhLevel(float phLevel);

private:
    bool firstReading = true;
    float timeLastRotatedFor = 0;

    float lastReading = 0;

    // speed controls - shouldn't change after run time
    float increaseSpeed;
    float decreaseSpeed;

    const float maxIncreaseSpeed = 180;
    const float maxDecreaseSpeed = 0;
    const float stopSpeed = 90;

    // time controls - shouldn't change after run time
    float maxTimeToRun;
    const float minTimeToRun = 0;

    const float maxTimeIncreaseToRun;
    const float defaultTimeToRun;

    void increasePh(float timeToChange);
    void decreasePh(float timeToChange);
};

#endif //ARDUINO_PH_SENSOR_H

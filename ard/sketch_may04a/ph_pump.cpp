#include "ph_pump.h"

void PhSensor::levelOutPhLevel(float phLevel) {
    logger.log(LogLevel::INFO, "Current ph level:", phLevel);
    logger.log(LogLevel::INFO, "Last ph level:", lastReading);

    // seeing if reading is already in range
    if (phLevel >= minPhLevel && phLevel <= maxPhLevel) {
        logger.log(LogLevel::INFO, "Ph level is at required level", "\n");
        firstReading = true;
        return;
    }

    // sees how much the levels have changed since last reading
    float pastChange;

    // flags if the levels need to be dropped or raised
    bool increase = true;

    // how much the ph levels need to change
    float neededChange;

    // seeing if the ph is too low and finding out by how much to pass into arguments
    if (phLevel < minPhLevel) {
        logger.log(LogLevel::INFO, "Ph Level needs increasing");

        pastChange = phLevel - lastReading;

        if (!firstReading && pastChange <= 0) {
            logger.log(LogLevel::ERROR, "Last pump didn't increase ph levels\n");
            return;
        }

        neededChange = minPhLevel - phLevel;
        increase = true;
    }

    // seeing if the ph is too high and finding out by how much to pass into arguments
    if (phLevel > maxPhLevel) {
        logger.log(LogLevel::INFO, "Ph level needs decreasing");

        pastChange = lastReading - phLevel;

        if (!firstReading && pastChange <= 0) {
            logger.log(LogLevel::ERROR, "Last pump didn't decrease ph levels\n");
            return;
        }

        neededChange = phLevel - maxPhLevel;
        increase = false;
    }

    logger.log(LogLevel::INFO, "Past change:", pastChange);
    logger.log(LogLevel::INFO, "Last Time Rotated for:", timeLastRotatedFor);
    logger.log(LogLevel::INFO, "Needed pH level change:", neededChange, "Increase (Bool):", increase);

    float howMuchPerSecond = pastChange / timeLastRotatedFor;
    logger.log(LogLevel::INFO, "Previous raise per second:", howMuchPerSecond);

    float timeToChange = firstReading ? defaultTimeToRun : neededChange / howMuchPerSecond;

    logger.log(LogLevel::INFO, "How Long wanted to rotate pump:", timeToChange);
    float clampedTime = timeToChange;

    // seeing if ct has increase to max time increase
    if (timeLastRotatedFor + maxTimeIncreaseToRun < clampedTime) {
        logger.log(LogLevel::WARNING, "Needed to clamp time since went over max increase");
        clampedTime = timeLastRotatedFor + maxTimeIncreaseToRun;
    }

    // seeing if ct is over max time
    if (clampedTime > maxTimeToRun) {
        logger.log(LogLevel::WARNING, "Needed to clamp time since went over max time");
        clampedTime = maxTimeToRun;
    }

    // seeing if ct is under min time
    if (clampedTime < minTimeToRun) {
        logger.log(LogLevel::ERROR, "Time to run was under min time. Time to run should never fall below 0 unless at right level unless min time changed is not 0."
                                     "Reassigning to min time to run but may cause errors since ignores other clamps");
        clampedTime = minTimeToRun;
    }

    if (increase) {
        // call method for increasing
        increasePh(clampedTime);
    } else {
        // call method for decreasing
        decreasePh(clampedTime);
    }

    firstReading = false;
    lastReading = phLevel;
}

void PhSensor::increasePh(float timeToChange) {
    logger.log(LogLevel::INFO, "How long to rotate pump (new):", timeToChange, "\n");

    // finding out how fast and long to rotate the pump

    // keeping track of what's happened
    timeLastRotatedFor = timeToChange;
}

void PhSensor::decreasePh(float timeToChange) {
    logger.log(LogLevel::INFO, "How long to rotate pump (new):", timeToChange, "\n");

    // finding out how fast and long to rotate the pump

    // keeping track of what's happened
    timeLastRotatedFor = timeToChange;
}

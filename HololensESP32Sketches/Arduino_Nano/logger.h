#ifndef LOGGER_H
#define LOGGER_H

#include <Arduino.h>

enum class LogLevel { DEBUG, INFO, WARNING, ERROR };

class Logger {
public:
    Logger();

    void setLogLevel(LogLevel level);

    void enableLogging(bool enable);

    template <typename T, typename... Args>
    void log(LogLevel level, T first, Args... args) {
        if (!logEnabled || level < logLevel) {
            return;
        }

        Serial.print("[");
        Serial.print(logLevelToString(level));
        Serial.print("] ");
        Serial.print(first);
        ((Serial.print(" "), Serial.print(args)), ...); // Print remaining arguments
        Serial.println();
    }

private:
    LogLevel logLevel;
    bool logEnabled;

    const char* logLevelToString(LogLevel level);
};

#endif // LOGGER_H

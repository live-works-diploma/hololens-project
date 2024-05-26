#include "Logger.h"

Logger::Logger() : logLevel(LogLevel::DEBUG), logEnabled(true) {}

void Logger::setLogLevel(LogLevel level) {
    logLevel = level;
}

void Logger::enableLogging(bool enable) {
    logEnabled = enable;
}

const char* Logger::logLevelToString(LogLevel level) {
    switch (level) {
        case LogLevel::DEBUG:
            return "DEBUG";
        case LogLevel::INFO:
            return "INFO";
        case LogLevel::WARNING:
            return "WARNING";
        case LogLevel::ERROR:
            return "ERROR";
        default:
            return "UNKNOWN";
    }
}

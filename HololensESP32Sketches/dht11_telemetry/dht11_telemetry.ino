/* 
 * ESP32 NodeMCU DHT11 - Humidity Temperature Sensor Example
 * https://circuits4you.com
 * 
 * References
 * https://circuits4you.com/2017/12/31/nodemcu-pinout/
 * https://techcommunity.microsoft.com/t5/internet-of-things-blog/esp32-with-arduino-ide-your-mqtt-bridge-into-azure-iot-hub/ba-p/3052128
 */
#include <DHT.h>

// // Wi-Fi credentials
// const char* ssid = "dlink-A3E418"; //network ssid
// const char* wifi_password = "gmbwg8w7n79"; //wifi password

// // //set up for wifi over hotspot
// // const char* ssid = "NokiaG21"
// // const char* wifi_password = "Bullet66!"

#define DHTPIN 15    //D15 of ESP32 DevKit
#define DHTTYPE DHT11
//DHTesp dht;
DHT dht(DHTPIN, DHTTYPE);

void setup()
{
  Serial.begin(115200); //Set Baud Rate
  Serial.println();
  Serial.println("Fetching Telemetry...");
  dht.begin();
}

void loop()
{
  float h = dht.readHumidity(); //Get Humidity
  float t = dht.readTemperature(); // Get Temperature (Celcius)

  // Check for any failure to read Sensor and Exit
  if (isnan(h) || isnan(t)) 
  {
    Serial.println("Failed to read from DHT sensor!");
    return;
  }

  //BLINK TO BE ADDED IF REQUIRED
  //blink_led()  # Blink the LED after sending data
  //print()
  // Serial.print("Temperature: {} C ".format(t))
  // Serial.print("Humidity: {} ".format(h))

  //char* = MQTTPayload;
  //Serial.print(temp + " " + hum);
  //String MQTTPayload = String(t, h);

  // Format the MQTT payload string
  String MQTTPayload = "Humidity: " + String(h, 1) + " %   Temperature: " + String(t, 1) + " C";

  // print the result to SerialMonitor - Set Baud to 115200
  // Serial.print("Humidity: ");
  // Serial.print(h);
  // Serial.print(" %\t");
  // Serial.print("Temperature: ");
  // Serial.print(t);
  // Serial.println(" C ");
  Serial.print(MQTTPayload);
  Serial.println(); // Start a new line
  delay(2000);
}

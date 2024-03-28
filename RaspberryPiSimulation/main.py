from azure.iot.device import IoTHubDeviceClient, Message, exceptions
import logging
import json

# Set up logging
logging.basicConfig(level=logging.INFO)

# Define the connection string
connection_string = "HostName=iot-sensor-iot-hub.azure-devices.net;DeviceId=sim-raspberry-pi;SharedAccessKey=TXWwqyQu6Bx3zUl5dvgOIv/5+EJASYz5iAIoTI7Cu/E="

logging.info("\n")
try:
    # Create an instance of the device client using the connection string
    client = IoTHubDeviceClient.create_from_connection_string(connection_string)
    logging.info("Create client")
except exceptions.CredentialError as e:
    logging.error(f"Invalid credentials specified: {e}")
except exceptions.ConnectionFailedError as e:
    logging.error(f"Failed to connect to Azure IoT Hub: {e}")
except Exception as e:
    logging.error(f"Error creating client: {e}")

logging.info("\n")
try:
    # Connect the client
    client.connect()
    logging.info("Connected to client")
except exceptions.ConnectionFailedError as e:
    logging.error(f"Failed to connect to Azure IoT Hub: {e}")
except Exception as e:
    logging.error(f"Error connecting to client: {e}")

# Define the message payload using the wanted_message data
wanted_message = {
    "Sensor": [
        {
            "Name": "Raspberry Pi Sim",
            "WaterLevel": "1",
            "PHLevel": "1"
        }
    ]
}

# Convert the wanted_message dictionary to a JSON string
message_payload = json.dumps(wanted_message)

# Define the message object
message = Message(message_payload)
message.custom_properties["Property1"] = "Value1"
message.custom_properties["Property2"] = "Value2"

logging.info("\n")
try:
    # Send the message
    client.send_message(message)
    logging.info("Sent message to client")
except exceptions.MessageSendFailureError as e:
    logging.error(f"Failed to send message to Azure IoT Hub: {e}")
except Exception as e:
    logging.error(f"Error sending data: {e}")

logging.info("\n")
try:
    # Disconnect the client
    client.disconnect()
    logging.info("Disconnected from client")
except exceptions.ConnectionFailedError as e:
    logging.error(f"Failed to disconnect from Azure IoT Hub: {e}")
except Exception as e:
    logging.error(f"Error disconnecting: {e}")

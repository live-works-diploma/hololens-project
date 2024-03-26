from app.azure_blob_container import AzureBlobContainer
from app.azure_database import AzureFunctions
from app.json import Json
from app.default_data import Default

import logging
from msal import ConfidentialClientApplication
# pip install msal
import time


# Configure logging
logging.basicConfig(level=logging.INFO)


def CreateBlobDefaultData():
    """Connects to azure blob container directly and adds a new json containing data."""

    connection_string = "DefaultEndpointsProtocol=https;AccountName=iotdataretrieval;AccountKey=5rsT9GWrQGLKLlFOMvqUBCA72W74RQqWnBUU6X/WD2zkTOsRB/GeggJG956laWukPZeijH1+ChIw+AStkd7rCg==;EndpointSuffix=core.windows.net"
    container_name = "sensordata"

    azure = AzureBlobContainer(connection_string, container_name, "")
    json = Json()
    default_data = Default()

    created_data = default_data.CreateDefaultData()
    logging.info(f"Created Data: {created_data}")

    json_data = json.ConvertToJson(created_data)
    logging.info(f"Json Data: {json_data}")

    name_of_data = f"test_sensor_data_{time.time()}"
    logging.info(f"Name of Data: {name_of_data}")

    azure.Send(json_data, name_of_data)


def CreateDatabaseDefaultData(key: str):
    """Connects to a function, adds default data then sends that data up and allows the function to add it to the database."""
    
    function_url = "https://iotsensor-funcs.azurewebsites.net/api/DatabaseInteraction"
    function_key = "6edFuUYY4CJIFSuP9M0TuvzWdFew2iJW-UaA2I_pTIiOAzFuWlCZow=="

    client = AzureFunctions(function_url, function_key, key)

    default_data = Default()
    json_converter = Json()

    data = default_data.CreateDefaultData()
    json_data = json_converter.ConvertToJson(data)

    if client.send_data_to_database(json_data):
        print("Sending data Successful.")
    else:
        print("Sending data Unsuccessful.")


def GetDatabaseData(key: str):
    """Retrieves data from database"""

    function_url = "https://iotsensor-funcs.azurewebsites.net/api/DatabaseInteraction"
    function_key = "6edFuUYY4CJIFSuP9M0TuvzWdFew2iJW-UaA2I_pTIiOAzFuWlCZow=="

    client = AzureFunctions(function_url, function_key, key)

    data = client.retrieve_data_from_database()
    print(data)
    

def CreateDatabaseTables(key: str):
    function_url = "https://iotsensor-funcs.azurewebsites.net/api/DatabaseCreateTable"
    function_key = "ANjf-r4xfSJwl-c9-ZSDhmnezN0PHlVCfGgH1bfcG2k3AzFuuPE1vw=="

    client = AzureFunctions(function_url, function_key, key)

    default_data = Default()
    json_converter = Json()

    data = default_data.create_default_instance()
    json_data = json_converter.ConvertToJson(data)

    print(json_data)

    if client.send_data_to_database(json_data):
        print("Create Tables Successful.")
    else:
        print("Create Tables Unsuccessful.")


master_key = "xY9WO3XPmVvAy9KwdmGTULuVIlfy_q4dn3Oi-NNd5oWCAzFuM2azqw=="
default_key = "AvDHmPkNon7U2I2cgVNYkwKORH6H4HBNYQT5OvF1rd41AzFuJdfj3Q=="


if __name__ == "__main__":
    # Configure logging to print to console
    console = logging.StreamHandler()
    console.setLevel(logging.INFO)
    formatter = logging.Formatter('%(asctime)s - %(name)s - %(levelname)s - %(message)s')
    console.setFormatter(formatter)
    logging.getLogger('').addHandler(console)

    # Call the function with logging
    try:
        # CreateDatabaseTables(master_key)
        CreateDatabaseDefaultData(master_key)
        # GetDatabaseData(default_key)
    except Exception as e:
        logging.error(f"Error occurred: {e}")

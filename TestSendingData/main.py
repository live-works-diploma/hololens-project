from app.azure_database import AzureFunctions
from app.json import Json
from app.default_data import Default

import logging
from msal import ConfidentialClientApplication # pip install msal


# Configure logging
logging.basicConfig(level=logging.INFO)


def data_create(key: str):
    """Connects to a function, adds default data then sends that data up and allows the function to add it to the database."""
    
    function_url = "https://iotsensor-funcs.azurewebsites.net/api/HttpDBItemCreate"
    function_key = "_9rGddOS9QjGP6geaXcdQnljGE4Er33vVa6VslgpwqXNAzFufY7FBw=="

    client = AzureFunctions(function_url, function_key, key)

    default_data = Default()
    json_converter = Json()

    data = default_data.create_default_data(5)    
    json_data = json_converter.ConvertToJson(data)
    
    print(f"Data being sent: {json_data}")

    if client.send_data(json_data):
        print("Sending data Successful.")
    else:
        print("Sending data Unsuccessful.")


def data_read(key: str):
    """Retrieves data from database"""

    function_url = "https://iotsensor-funcs.azurewebsites.net/api/HttpDBItemRead"
    function_key = "VaayMcEHGs0P-Pp6OUbCOvWdhoWk9aHc8b7rG-rdh6pqAzFuhKPhbQ=="

    client = AzureFunctions(function_url, function_key, key)
    json_converter = Json()

    table_names = [
        "TelemetryData",
        # "Plant",
        # "Sensor",
    ]

    TableNames = json_converter.ConvertToJson(table_names)

    conditions = ""

    data = client.retrieve_data(TableNames, conditions)
    print(f"data found: {data}")
    

def data_update(key: str):
    function_url = "https://iotsensor-funcs.azurewebsites.net/api/HttpDBItemUpdate"
    function_key = "qnhRzZouK6LPYz3AVhmq-G8if9OmnpBjwnIAS5rnV_2IAzFuamn2Pg=="

    client = AzureFunctions(function_url, function_key, key)

    default_data = Default()
    json_converter = Json()

    data = default_data.create_random_update_data(1)    
    json_data = json_converter.ConvertToJson(data)
    
    print(f"Data being sent: {json_data}")

    queries = [
        "TableName=Sensor",
        "ConditionColumn=id",
    ]

    if client.send_data(json_data, queries):
        print("Sending data Successful.")
    else:
        print("Sending data Unsuccessful.")


def data_delete(key: str):
    function_url = "https://iotsensor-funcs.azurewebsites.net/api/HttpDBItemDelete"
    function_key = "a4sm_52taftAqU78rEWV_8IXC4mEuoay7HbLFsoalzyJAzFuW4WeCA=="

    client = AzureFunctions(function_url, function_key, key)

    data = {}

    data["Sensor"] = [
        {
            "id": 1
        }
    ]

    data["Plant"] = [
        {
            "id": 1
        }
    ]

    data["TelemetryData"] = [
        {
            "id": 1
        }
    ]

    json_convert = Json()

    json_data = json_convert.ConvertToJson(data)

    if client.send_data(json_data):
        print("Sending data Successful")
    else:
        print("Sending data Unsuccessful.")


def table_create(key: str):
    function_url = "https://iotsensor-funcs.azurewebsites.net/api/HttpDBTableCreate"
    function_key = "d9uHxMrfwoG8p1zLuwtbndmdMB1cKVUfpJBv91qBFbC9AzFu8im81Q=="

    client = AzureFunctions(function_url, function_key, key)

    default_data = Default()
    json_converter = Json()

    data = default_data.create_default_instance()
    json_data = json_converter.ConvertToJson(data)

    print(f"Data being sent: {json_data}")

    if client.send_data(json_data):
        print("Create Tables Successful.")
    else:
        print("Create Tables Unsuccessful.")


def send_command(key: str):
    function_url = "https://iotsensor-funcs.azurewebsites.net/api/HttpIOThubCommand"
    function_key = "waSGFDDXda5EGUb77oTVX6_m1FFMSwHOIfb2_ldM-FRPAzFuPibN5g=="

    to_send = {
        "command": "attempt: 1"
    }

    client = AzureFunctions(function_url, function_key, key)
    json_converter = Json()

    json_data = json_converter.ConvertToJson(to_send)

    if client.send_data(json_data):
        print("Sending data Successful")
    else:
        print("Sending data Unsuccessful.")


master_key = "xY9WO3XPmVvAy9KwdmGTULuVIlfy_q4dn3Oi-NNd5oWCAzFuM2azqw=="     # works for everything
default_key = "AvDHmPkNon7U2I2cgVNYkwKORH6H4HBNYQT5OvF1rd41AzFuJdfj3Q=="    # works for function authority and below (basically everything except creating / updating tables)


def main():
    # Configure logging to print to console
    console = logging.StreamHandler()
    console.setLevel(logging.INFO)
    formatter = logging.Formatter('%(asctime)s - %(name)s - %(levelname)s - %(message)s')
    console.setFormatter(formatter)
    logging.getLogger('').addHandler(console)

    # Call the function with logging
    try:
        # table_create(master_key)

        # data_create(default_key)
        # data_read(default_key)        
        # data_update(default_key)  
        # data_delete(default_key)   

        send_command(default_key) 
        pass  
    except Exception as e:
        logging.error(f"Error occurred: {e}")


if __name__ == "__main__":
    main()

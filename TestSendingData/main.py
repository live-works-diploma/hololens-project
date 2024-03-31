from app.azure_database import AzureFunctions
from app.json import Json
from app.default_data import Default

import logging
from msal import ConfidentialClientApplication # pip install msal


# Configure logging
logging.basicConfig(level=logging.INFO)


def create_data(key: str):
    """Connects to a function, adds default data then sends that data up and allows the function to add it to the database."""
    
    function_url = "https://iotsensor-funcs.azurewebsites.net/api/DatabaseInsertItem"
    function_key = "OnW_5R3npJxH-K0YolOjnHqyLis3HQQezb4BzG55MnIgAzFuWXxvzg=="

    client = AzureFunctions(function_url, function_key, key)

    default_data = Default()
    json_converter = Json()

    data = default_data.create_default_data()    
    json_data = json_converter.ConvertToJson(data)
    
    print(f"Data being sent: {json_data}")

    if client.send_data_to_database(json_data):
        print("Sending data Successful.")
    else:
        print("Sending data Unsuccessful.")


def retrieve_data(key: str):
    """Retrieves data from database"""

    function_url = "https://iotsensor-funcs.azurewebsites.net/api/DatabaseGetItem"
    function_key = "tZZDyPwLOOM6QcRV8FufauF1hICAm8-BcrH8POGmPrMXAzFuZ7toqw=="

    client = AzureFunctions(function_url, function_key, key)
    json_converter = Json()

    table_names = [
        # "TelemetryData",
        # "Plant",
        "Sensor",
    ]

    TableNames = json_converter.ConvertToJson(table_names)

    conditions = ""

    data = client.retrieve_data_from_database(TableNames, conditions)
    print(f"data found: {data}")
    

def update_data(key: str):
    function_url = "https://iotsensor-funcs.azurewebsites.net/api/DatabaseUpdateItem"
    function_key = "2Xba3QinJQZ_xk3xDEhKpC9vveiwuB2f3Ca-CxXDaneeAzFu0ATKzQ=="

    client = AzureFunctions(function_url, function_key, key)

    default_data = Default()
    json_converter = Json()

    data = default_data.create_random_upate_data(1)    
    json_data = json_converter.ConvertToJson(data)
    
    print(f"Data being sent: {json_data}")

    queries = [
        "TableName=Sensor",
        "ConditionColumn=Name",
    ]

    if client.send_data_to_database(json_data, queries):
        print("Sending data Successful.")
    else:
        print("Sending data Unsuccessful.")


def delete_data(key: str):
    ...


def create_tables(key: str):
    function_url = "https://iotsensor-funcs.azurewebsites.net/api/DatabaseTableCreation"
    function_key = "JOb-JXMCv0Lg7YvnLPrr679np-pEq-ExL8EhXPo3QnwfAzFuxyUhuA=="

    client = AzureFunctions(function_url, function_key, key)

    default_data = Default()
    json_converter = Json()

    data = default_data.create_default_instance()
    json_data = json_converter.ConvertToJson(data)

    print(f"Data being sent: {json_data}")

    if client.send_data_to_database(json_data):
        print("Create Tables Successful.")
    else:
        print("Create Tables Unsuccessful.")


master_key = "xY9WO3XPmVvAy9KwdmGTULuVIlfy_q4dn3Oi-NNd5oWCAzFuM2azqw=="     # works for everything
default_key = "AvDHmPkNon7U2I2cgVNYkwKORH6H4HBNYQT5OvF1rd41AzFuJdfj3Q=="    # works for function authority and below (basically everything except creating / updating tables)


if __name__ == "__main__":
    # Configure logging to print to console
    console = logging.StreamHandler()
    console.setLevel(logging.INFO)
    formatter = logging.Formatter('%(asctime)s - %(name)s - %(levelname)s - %(message)s')
    console.setFormatter(formatter)
    logging.getLogger('').addHandler(console)

    # Call the function with logging
    try:
        # create_tables(master_key)

        # retrieve_data(default_key)
        # create_data(default_key)
        # update_data(default_key)  
        # delete_data(default_key)    
        pass  
    except Exception as e:
        logging.error(f"Error occurred: {e}")

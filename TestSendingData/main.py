from app.azure_blob_container import AzureBlobContainer
from app.azure_database import AzureFunctionsClient
from app.json import Json
from app.default_data import Default

import time

def CreateBlobDefaultData():
    connection_string = "DefaultEndpointsProtocol=https;AccountName=iotdataretrieval;AccountKey=5rsT9GWrQGLKLlFOMvqUBCA72W74RQqWnBUU6X/WD2zkTOsRB/GeggJG956laWukPZeijH1+ChIw+AStkd7rCg==;EndpointSuffix=core.windows.net"
    container_name = "sensordata"

    azure = AzureBlobContainer(connection_string, container_name)
    json = Json()
    default_data = Default()

    created_data = default_data.CreateDefaultData()
    print(f"Created Data: {created_data}")

    json_data = json.ConvertToJson(created_data)
    print(f"Json Data: {json_data}")

    name_of_data = f"test_sensor_data_{time.time()}"
    print(f"Name of Data: {name_of_data}")

    azure.Send(json_data, name_of_data)


def CreateDBDefaultData():
    function_url = "https://iotsensor-funcs.azurewebsites.net/api/DatabaseCreateTable?code=6edFuUYY4CJIFSuP9M0TuvzWdFew2iJW-UaA2I_pTIiOAzFuWlCZow=="

    client = AzureFunctionsClient(function_url)
    default_data = Default()
    json = Json()

    data = default_data.create_default_instance()
    json_data = json.ConvertToJson(data)
    client.send_data_to_database(json_data)


def CreateDBDefaultDataTwo():
    function_url = "https://iotsensor-funcs.azurewebsites.net/api/DatabaseInteraction?code=X41by5IoKZ8CC4k7qp8qgmvFQX6sZYqdOQvDbjTbbC1hAzFuVCIAlg=="

    client = AzureFunctionsClient(function_url)
    default_data = Default()
    json = Json()

    data = default_data.CreateDefaultData()
    json_data = json.ConvertToJson(data)
    client.send_data_to_database(json_data)


if __name__ == ("__main__"):
    CreateDBDefaultData()
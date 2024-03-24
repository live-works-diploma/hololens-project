from app.azure import Azure
from app.json import Json
from app.default_data import Default

import time

def main():
    connection_string = "DefaultEndpointsProtocol=https;AccountName=iotdataretrieval;AccountKey=5rsT9GWrQGLKLlFOMvqUBCA72W74RQqWnBUU6X/WD2zkTOsRB/GeggJG956laWukPZeijH1+ChIw+AStkd7rCg==;EndpointSuffix=core.windows.net"
    container_name = "sensordata"

    azure = Azure(connection_string, container_name)
    json = Json()
    default_data = Default()

    created_data = default_data.CreateDefaultData()
    print(f"Created Data: {created_data}")

    json_data = json.ConvertToJson(created_data)
    print(f"Json Data: {json_data}")

    name_of_data = f"test_sensor_data_{time.time()}"
    print(f"Name of Data: {name_of_data}")

    azure.Send(json_data, name_of_data)


if __name__ == ("__main__"):
    main()
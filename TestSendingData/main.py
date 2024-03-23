from app.azure import Azure
from app.json import Json
from app.default_data import Default

import time

def main():
    how_long_to_sleep_for = 5

    connection_string = "DefaultEndpointsProtocol=https;AccountName=iotdataretrieval;AccountKey=5rsT9GWrQGLKLlFOMvqUBCA72W74RQqWnBUU6X/WD2zkTOsRB/GeggJG956laWukPZeijH1+ChIw+AStkd7rCg==;EndpointSuffix=core.windows.net"
    container_name = "sensordata"

    azure = Azure(connection_string, container_name)
    json = Json()
    default_data = Default()

    while True:
        print("Sending Data...")
        created_data = default_data.CreateDefaultData()
        data = json.ConvertToJson(created_data)
        azure.Send(data, f"test_sensor_data_{time.time()}")
        
        # sleep(how_long_to_sleep_for)
        break


if __name__ == ("__main__"):
    main()
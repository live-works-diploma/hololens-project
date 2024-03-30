import random


class Default:
    def __init__(self) -> None:
        pass

    def create_default_data(self, number_of_instances: int = 1) -> dict[str, list[dict[str, str]]]:
        data: dict[str, list[dict[str, str]]] = dict()

        types_of_data_to_send: dict = {
            "Sensor": create_sensor_data,
            "Plant": create_plant_data,
            "TelemetryData": create_telemetry_data,
        }

        for name in types_of_data_to_send:
            instances: list[dict[str, str]] = []
            
            instances.append(types_of_data_to_send[name](f"updated data"))

            data[name] = instances

        return data
    
    def create_default_instance(self):
        tables = {}

        tables["Sensor"] = ["Name", "WaterLevel", "PhLevel"]
        # tables["Plant"] = ["Name", "Scale", "Fruiting"]
        # tables["TelemetryData"] = ["Name", "Temperature", "Humidity", "WaterLevel", "OverHeating"]

        return tables
    

def create_random_value(min_value: float, max_value: float) -> float:
    random_number = random.uniform(min_value, max_value)
    return random_number


create_plant_data = lambda name: {
    "Name": f"{name}",
    "Scale": f"{create_random_value(0.5, 2)}",
    "Fruiting": "true" if create_random_value(0, 1) > 0.5 else "false",
}

create_sensor_data = lambda name: {
    "Name": f"{name}",
    # "WaterLevel": f"{create_random_value(0.1, 3)}",
    # "PhLevel": f"{create_random_value(2, 10)}",
}

create_telemetry_data = lambda name: {
    "Name": f"{name}", 
    # "Temperature": create_random_value(0, 1), 
    # "Humidity": f"{create_random_value(0, 1)}", 
    # "WaterLevel": f"{create_random_value(0, 1)}", 
    # "OverHeating": "true" if create_random_value(0, 1) > 0.5 else "false",
}


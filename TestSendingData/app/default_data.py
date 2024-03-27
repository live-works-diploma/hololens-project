import random


class Default:
    def __init__(self) -> None:
        pass

    def CreateDefaultData(self, number_of_instances: int = 1) -> dict[str, list[dict[str, str]]]:
        data: dict[str, list[dict[str, str]]] = dict()

        types_of_data_to_send: dict = {
            "Sensor": create_sensor_data,
            # "Plant": create_plant_data,
        }

        for name in types_of_data_to_send:
            instances: list[dict[str, str]] = []

            for i in range(number_of_instances):
                instances.append(types_of_data_to_send[name](f"{name}: {i+1}"))

            data[name] = instances

        return data
    
    def create_default_instance(self):
        data = {}

        data["Sensor"] = ["Name", "WaterLevel", "PhLevel"]

        return data
    

def CreateDefaultValue(min_value: float, max_value: float) -> float:
    random_number = random.uniform(min_value, max_value)
    return random_number


create_plant_data = lambda name : {
    "Name": f"{name}",
    "Scale": f"{CreateDefaultValue(0.5, 2)}",
    "Fruiting": "true" if CreateDefaultValue(0, 1) > 0.5 else "false",
}

create_sensor_data = lambda name : {
    "Name": f"{name}",
    "WaterLevel": f"{CreateDefaultValue(0.1, 3)}",
    "PhLevel": f"{CreateDefaultValue(2, 10)}",
}


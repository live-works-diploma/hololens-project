import random


class Default:
    def __init__(self) -> None:
        pass

    def CreateDefaultData(self, number_of_instances: int = 1) -> dict[str, list[dict[str, str]]]:
        data: dict[str, list[dict[str, str]]] = dict()

        types_of_data_to_send: dict = {
            "Plant": create_plant_data,
            "Sensor": create_sensor_data,
        }

        for name in types_of_data_to_send:
            instances: list[dict[str, str]] = []

            for i in range(number_of_instances):
                instances.append(types_of_data_to_send[name](f"{name}: {i}"))

            data[name] = instances

        return data
    

def CreateDefaultValue(min_value: float, max_value: float) -> float:
    random_number = random.uniform(min_value, max_value)
    return random_number


create_plant_data = lambda name : {
    "name": name,
    "scale": f"{CreateDefaultValue(0.5, 2)}",
    "fruiting": "true" if CreateDefaultValue(0, 1) > 0.5 else "false",
}

create_sensor_data = lambda name : {
    "name": name,
    "water level": f"{CreateDefaultValue(0.1, 3)}",
    "ph level": f"{CreateDefaultValue(2, 10)}",
}


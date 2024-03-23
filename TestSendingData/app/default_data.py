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

            for _ in range(number_of_instances):
                instances.append(types_of_data_to_send[name]())

            data[name] = instances

        return data
    

create_plant_data = lambda : {
    "scale": "2",
    "fruiting": "true",
}

create_sensor_data = lambda : {
    "water level": "15",
    "ph level": "5",
}
import json


class Json:
    def __init__(self) -> None:
        ...

    def ConvertToJson(self, data: dict[str, list[dict[str, str]]]):
        return json.dumps(data)






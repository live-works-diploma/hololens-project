import json


class Json:
    def __init__(self) -> None:
        ...

    def ConvertToJson(self, data):
        return json.loads(json.dumps(data))






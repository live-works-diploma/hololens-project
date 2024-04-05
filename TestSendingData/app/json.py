import json


class Json:
    def ConvertToJson(self, data):
        return json.loads(json.dumps(data))

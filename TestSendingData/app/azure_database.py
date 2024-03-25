import requests

class AzureFunctionsClient:
    def __init__(self, function_url: str) -> None:
        self.function_url = function_url

    def send_data_to_database(self, data: dict) -> bool:
        """Sends data to the Azure Function to be saved to the database."""
        try:
            response = requests.post(self.function_url, json=data)
            response.raise_for_status()
            return True
        except requests.exceptions.RequestException as e:
            print(f"Error sending data to database: {e}")
            return False
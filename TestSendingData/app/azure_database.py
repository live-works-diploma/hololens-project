import requests

class AzureFunctions:
    def __init__(self, function_url: str, function_key: str, access_key: str) -> None:
        self.function_url = function_url
        self.function_key = function_key
        self.access_key = access_key

    def send_data_to_database(self, data: dict) -> bool:
        """Sends data to the Azure Function to be saved to the database."""
        try:
            headers = {
                "Content-Type": "application/json",
                "x-functions-key": self.access_key
            }

            response = requests.post(self.function_url, headers=headers, json=data)
            response.raise_for_status()
            return True
        except requests.exceptions.RequestException as e:
            print(f"Error sending data to database: {e}")
            if hasattr(e, 'response') and e.response is not None:
                print(f"Response status code: {e.response.status_code}")
                print(f"Response text: {e.response.text}")
            return False
        

    def retrieve_data_from_database(self, expected_types: str) -> dict:
        """Retrieves data from the database."""

        if not expected_types:
            print("Expected types cannot be null or empty.")
            return {}

        headers = {
            "Content-Type": "application/json",
            "x-functions-key": self.access_key,
        }

        print(expected_types)

        try:
            url_with_query = f"{self.function_url}?jsonString={expected_types}"
            response = requests.get(url_with_query, headers=headers)
            response.raise_for_status()
            return response.json()
        except requests.exceptions.RequestException as e:
            print(f"Error retrieving data from database: {e}")
            if response.status_code != 200:
                print(f"Response text: {response.text}")
            return {}
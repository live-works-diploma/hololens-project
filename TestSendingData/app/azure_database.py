import requests


class AzureFunctions:
    def __init__(self, function_url: str, function_key: str, access_key: str) -> None:
        self.function_url = function_url
        self.function_key = function_key
        self.access_key = access_key

    def send_data(self, data: dict, extra_queries: list[str] = []) -> bool:
        """Sends data to the Azure Function to be saved to the database."""
        try:
            headers = {
                "Content-Type": "application/json",
                "x-functions-key": self.access_key
            }

            function_url_query = f"{self.function_url}?{'&'.join(extra_queries)}"

            response = requests.post(function_url_query, headers=headers, json=data)
            response.raise_for_status()
            return True
        except requests.exceptions.RequestException as e:
            print(f"Error sending data to database: {e}")
            if hasattr(e, 'response') and e.response is not None:
                print(f"Response status code: {e.response.status_code}")
            return False
        

    def retrieve_data(self, table_names: str, conditions: str = "") -> dict | str:
        """Retrieves data from the database."""

        if not table_names:
            print("Expected types cannot be null or empty.")
            return {}

        headers = {
            "Content-Type": "application/json",
            "x-functions-key": self.access_key,
        }

        print(table_names)

        try:
            queries = [
                f"TableNames={table_names}"      
            ]

            if conditions != "":
                queries.append(f"Conditions={conditions}")

            url_with_query = f"{self.function_url}?{'&'.join(queries)}"

            print(url_with_query)

            response = requests.get(url_with_query, headers=headers)
            response.raise_for_status()
            return response.json()
        except requests.exceptions.RequestException as e:
            print(f"Error retrieving data from database: {e}")
            return {}

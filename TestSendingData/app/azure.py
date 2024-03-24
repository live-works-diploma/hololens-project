from azure.storage.blob import BlobServiceClient, BlobClient, ContainerClient
# pip install azure-storage-blob

class Azure:
    def __init__(self, connection_string: str, container_name: str) -> None:
        self.connection_string = connection_string
        self.container_name = container_name

        self.blob_service_client: BlobServiceClient = BlobServiceClient.from_connection_string(self.connection_string)
        self.container_client = self.blob_service_client.get_container_client(self.container_name)

    def Send(self, data: bytes, data_name: str):
        """Takes in the data you wish to save and uploads it as a blob.""" 
        try:
            self.container_client.upload_blob(name=data_name, data=data)
            return True

        except Exception as e:
            print(f"Error sending data: {e}")
            return False
   
    def Recieve(self):
        """Retrieves everything from container"""
        ...
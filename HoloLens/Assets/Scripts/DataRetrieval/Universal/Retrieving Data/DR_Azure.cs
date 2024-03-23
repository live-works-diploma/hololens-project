using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;

public class DR_Azure<T> : IDataRetrieval<T>, IJsonHandler<T> where T : class
{
    string connectionString;
    string containerName;
    double howLongToWaitForConnection = 10;

    Func<Dictionary<string, string>, Type, T> howToBuildTask;

    Dictionary<string, Type> expectedTypes = new Dictionary<string, Type>();
    public void SetExpectedTypes(Dictionary<string, Type> typesToListenFor)
    {
        expectedTypes = typesToListenFor;
    }

    BlobServiceClient blobServiceClient;
    BlobContainerClient containerClient;

    public DR_Azure(Func<Dictionary<string, string>, Type, T> howToBuildTask,
        string connectionString, string containerName)
    {
        if (howToBuildTask == null)
        {
            throw new Exception("How to build task can't be null");
        }

        blobServiceClient = new BlobServiceClient(connectionString);
        containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        this.howToBuildTask = howToBuildTask;

        this.connectionString = connectionString;
        this.containerName = containerName;
    }

    public async void Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData)
    {
        if (howToBuildTask == null)
        {
            throw new Exception("How to build task can't be null");
        }

        string json = await RetrieveJson();
        Dictionary<string, List<T>> builtData = IJsonHandler<T>.BuildData(json, howToBuildTask, expectedTypes);

        callWhenFoundData?.Invoke(builtData);
    }

    public async Task<string> RetrieveJson()
    {
        return await GetMostRecentJsonData();
    }

    async Task<string> GetMostRecentJsonData()
    {
        Debug.WriteLine("Getting most recent Blob");
        List<string> allBlobs = await GetAllJsonData(blobServiceClient, containerClient, connectionString, containerName, howLongToWaitForConnection);

        if (allBlobs.Count == 0)
        {
            throw new Exception("There was no data found in the string (unless the code has been edited this means you have connected to " +
                "azure but it didnt find anything in json format)");
        }

        return allBlobs[0];
    }

    public static async Task<List<string>> GetAllJsonData(BlobServiceClient blobServiceClient, BlobContainerClient containerClient, 
        string connectionString, string containerName, double howLongToWaitForConnection)
    {
        Debug.WriteLine("Getting all Blobs");

        var allBlobs = await GetAllBlobItems(blobServiceClient, containerClient, connectionString, containerName);

        List<string> allJsonStrings = new List<string>();

        Debug.WriteLine(allBlobs.Length);

        for (int i = 0; i < allBlobs.Length; i++)
        {
            BlobClient blobClient = containerClient.GetBlobClient(allBlobs[i].Name);
            BlobProperties properties = blobClient.GetProperties();

            Debug.WriteLine(properties.ContentType);

            try
            {
                // Set up a CancellationTokenSource with a 10-second timeout
                var cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(howLongToWaitForConnection));

                // Use DownloadContentAsync with CancellationToken to add a timeout
                var response = await blobClient.DownloadContentAsync(cancellationToken: cts.Token);

                string jsonContent = response.Value.Content.ToString();
                JObject.Parse(jsonContent);
                allJsonStrings.Add(jsonContent);
            }
            catch (OperationCanceledException e)
            {
                throw new Exception($"Failed connecting to Azure for time reasons: {e}");
            }
            catch (JsonReaderException e)
            {
                Debug.WriteLine($"Couldn't turn data into a json: {e}");
            }
        }

        return allJsonStrings;
    }

    public static async Task<BlobItem[]> GetAllBlobItems(BlobServiceClient blobServiceClient, BlobContainerClient containerClient, string connectionString, string containerName)
    {
        try
        {
            // Connect to Azure Blob Storage
            blobServiceClient = new BlobServiceClient(connectionString);

            Debug.WriteLine("Connected to Azure Blob Storage");

            // Get a reference to the specified container
            containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobItem[] blobs = containerClient.GetBlobs().OrderByDescending(blob => blob.Properties.LastModified).ToArray();

            return blobs;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to connect to Azure Blob Storage: {ex.Message}");
        }
    }
}

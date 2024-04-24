using Azure.Storage.Blobs;
using DatabaseFunctions.Models.Connecting;
using DatabaseFunctions.Models.Information;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DatabaseFunctions.Models.Items
{
    public class ModelDBItemRead
    {
        /// <summary>
        /// Choses where it gets its data from. Currently only azure blob storage and database. Blob storage is default unless stated otherwise.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dataTypes"></param>
        /// <param name="conditions"></param>
        /// <param name="blobStorage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Dictionary<string, List<Dictionary<string, object>>>>? GetData(ILogger logger, string[] dataTypes, string conditions, bool blobStorage = true)
        {
            if (blobStorage)
            {
                logger.LogInformation("Retrieving data from blob storage");
                return await BlobStorageGet(logger, dataTypes, conditions);
            }
            else
            {
                logger.LogInformation("Retrieving data from database");
                return DatabaseGet(logger, dataTypes, conditions);
            }
        }

        Dictionary<string, List<Dictionary<string, object>>>? DatabaseGet(ILogger logger, string[] tableNames, string conditions)
        {
            logger.LogInformation($"conditions: {conditions}");

            Func<string, SqlConnection, List<Dictionary<string, object>>> function = (tableName, connection) =>
            {
                string query = conditions == "" ? $"SELECT * FROM {tableName}" : $"SELECT * FROM {tableName} WHERE {conditions}";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Dictionary<string, object>> allInstances = new();

                    while (reader.Read())
                    {
                        Dictionary<string, object> instance = new();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            string? columnValue = reader.GetValue(i).ToString();

                            if (columnValue == null)
                            {
                                logger.LogError($"Error getting data for column name: {columnName}");
                                continue;
                            }

                            instance[columnName] = columnValue;
                        }

                        allInstances.Add(instance);
                    }

                    return allInstances;
                }
            };

            Dictionary<string, List<Dictionary<string, object>>> allData = new();

            for (int i = 0; i < tableNames.Length; i++)
            {
                var instancesFound = ModelDBConnect.AccessDatabase(logger, tableNames[i], ModelDBAccountInfo.builder, function);

                if (instancesFound == null)
                {
                    return null;
                }

                string strippedKey = Regex.Match(tableNames[i], @"\[dbo\]\.\[(.*?)\]").Groups[1].Value;
                string key = strippedKey == "" ? tableNames[i] : strippedKey;

                logger.LogInformation($"key: {key}, stripped key: {strippedKey}");
                allData[key] = instancesFound;
            }

            return allData;
        }

        async Task<Dictionary<string, List<Dictionary<string, object>>>> BlobStorageGet(ILogger logger, string[] tableNames, string conditions)
        {
            Dictionary<string, List<Dictionary<string, object>>> allData = new();

            try
            {
                var blobData = await GetJsonFromBlobAsync(logger, ModelBSAccountInfo.connectionString, ModelBSAccountInfo.containerName);

                logger.LogInformation($"Blob Data count: {blobData.Count}");

                List<Dictionary<string, object>> convertedBlobData = new();

                foreach (var jsonData in blobData)
                {
                    try
                    {
                        logger.LogInformation($"\nData found: {jsonData}");

                        var dataObject = JObject.Parse(jsonData);
                        var bodyData = dataObject["Body"].ToString();

                        if (bodyData == null)
                        {
                            logger.LogError("Did not contain body");
                            continue;
                        }

                        var decodedBody = DecodeBase64String(bodyData);

                        var systemProperties = dataObject["SystemProperties"].ToString();
                        var connectionDeviceId = JObject.Parse(systemProperties)["connectionDeviceId"].ToString();
                        var enqueuedTimeUtc = dataObject["EnqueuedTimeUtc"].ToString();

                        var body = new Dictionary<string, object>();

                        body["DeviceSent"] = connectionDeviceId;
                        body["DateSent"] = enqueuedTimeUtc;

                        // Flatten the BodyData dictionary
                        var bodyDataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(decodedBody);
                        foreach (var kvp in bodyDataDict)
                        {
                            body[kvp.Key] = kvp.Value;
                        }

                        convertedBlobData.Add(body);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Error processing data: {ex}");
                    }
                }

                allData["TelemetryData"] = convertedBlobData;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error getting data from blob storage: {ex}");
            }

            return allData;
        }

        string DecodeBase64String(string base64String)
        {
            var bytes = Convert.FromBase64String(base64String);
            return Encoding.UTF8.GetString(bytes);
        }

        async Task<List<string>> GetJsonFromBlobAsync(ILogger logger, string connectionString, string containerName)
        {
            List<string> jsonList = new List<string>();

            try
            {
                var blobServiceClient = new BlobServiceClient(connectionString);
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);              

                foreach (var blobItem in blobContainerClient.GetBlobs())
                {
                    var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
                    var response = await blobClient.DownloadAsync();
                    using (var memoryStream = new MemoryStream())
                    {
                        await response.Value.Content.CopyToAsync(memoryStream);
                        string foundString = Encoding.UTF8.GetString(memoryStream.ToArray());

                        logger.LogInformation($"\nBlob Item Name: {blobItem.Name}, Found string: {foundString}, json list count: {jsonList.Count}");

                        // Split the concatenated JSON string into individual JSON objects
                        List<string> individualJsonStrings = SplitJsonString(foundString);

                        // Add each individual JSON string to the list
                        jsonList.AddRange(individualJsonStrings);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error getting data from blob storage: {ex}");
            }

            return jsonList;
        }

        static List<string> SplitJsonString(string concatenatedJson)
        {
            List<string> jsonObjects = new List<string>();
            int startIndex = 0;
            int braceCount = 0;

            for (int i = 0; i < concatenatedJson.Length; i++)
            {
                if (concatenatedJson[i] == '{')
                {
                    braceCount++;
                }
                else if (concatenatedJson[i] == '}')
                {
                    braceCount--;
                    if (braceCount == 0)
                    {
                        // Found the end of a JSON object
                        int length = i - startIndex + 1;
                        jsonObjects.Add(concatenatedJson.Substring(startIndex, length));
                        startIndex = i + 1;
                    }
                }
            }

            return jsonObjects;
        }
    }
}

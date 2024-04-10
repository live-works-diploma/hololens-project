using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using Azure.Core;

public class DR_AzureDB<T> : IDataRetrieval<T>, IJsonHandler<T> where T : class
{
    public string functionKey;
    public string functionUrl;
    public string defaultKey;

    public Action<string> logger;

    public Func<Dictionary<string, string>, Type, T> howToBuildTask;

    public async void Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData, Dictionary<string, Type> expectedTypes)
    {
        logger("starting the retrieve of data");

        string jsonData = await RetrieveJson(expectedTypes);
        
        if (jsonData == null || jsonData == "")
        {
            logger("There was no data found");
            return;
        }

        logger(jsonData);

        Dictionary<string, List<T>> builtData = await JsonBuildTask<T>.BuildData(jsonData, howToBuildTask, expectedTypes);
        callWhenFoundData(builtData);
    }

    public async Task<string> RetrieveJson(Dictionary<string, Type> expectedTypes)
    {
        logger("sending req to function");

        List<string> tableNames = new List<string>();

        foreach (var name in expectedTypes.Keys)
        {
            tableNames.Add(name);
        }

        string query = $"TableNames={Uri.EscapeDataString(JsonConvert.SerializeObject(tableNames))}";

        try
        {
            // Send a GET request with the JSON data as a query parameter
            var response = await AzureFunctionRequestHandler.Get(query, functionUrl, defaultKey, logger);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                logger($"response code wasnt successful: {response.StatusCode}");
            }
            else
            {
                logger("didnt error");
            }           
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            logger($"Error retrieving data from database: {e.Message}; {functionUrl}; {query}");
            throw new Exception($"Error retrieving data from database: {e.Message}");
        }
    }
}

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

public class DR_AzureDB<T> : IDataRetrieval<T>, IJsonHandler<T>, IAzure where T : class
{
    public string functionKey { get; set; }
    public string functionUrl { get; set; }
    public string defaultKey { get; set; }

    public Action<string> logger;

    public Func<Dictionary<string, string>, Type, T> howToBuildTask;

    public async void Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData, Dictionary<string, Type> expectedTypes, string query)
    {
        logger("starting the retrieve of data");

        logger($"query: {query}");

        string jsonData = await RetrieveJson(query);
        
        if (jsonData == null || jsonData == "")
        {
            logger("There was no data found");
            return;
        }

        logger(jsonData);

        Dictionary<string, List<T>> builtData = IJsonHandler<T>.BuildData(jsonData, howToBuildTask, expectedTypes);
        callWhenFoundData(builtData);
    }

    public async Task<string> RetrieveJson(string queries)
    {
        logger("sending req to function");

        try
        {
            // Send a GET request with the JSON data as a query parameter
            var response = await IAzure.Get(queries, functionUrl, defaultKey, logger);

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
            logger($"Error retrieving data from database: {e.Message}; {functionUrl}; {queries}");
            throw new Exception($"Error retrieving data from database: {e.Message}");
        }
    }
}

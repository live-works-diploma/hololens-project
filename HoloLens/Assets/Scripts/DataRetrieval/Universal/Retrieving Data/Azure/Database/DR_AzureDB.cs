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
    public string functionKey;
    public string functionUrl;
    public string defaultKey;

    public Action<string> error;

    public Func<Dictionary<string, string>, Type, T> howToBuildTask;

    public async void Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData)
    {
        error("starting the retrieve of data");

        Action<SqlDataReader> dataReader = reader =>
        {
            while (reader.Read())
            {
                throw new Exception();
            }
        };

        List<string> keysList = expectedTypes.Keys.ToList();

        string toSend = JsonConvert.SerializeObject(keysList);
        string jsonData = await RetrieveJson(toSend);
        error(jsonData);

        Dictionary<string, List<T>> builtData = IJsonHandler<T>.BuildData(jsonData, howToBuildTask, expectedTypes);
        callWhenFoundData(builtData);
    }

    Dictionary<string, Type> expectedTypes;
    public void SetExpectedTypes(Dictionary<string, Type> typesToListenFor)
    {
        expectedTypes = typesToListenFor;
    }

    public async Task<string> RetrieveJson(string jsonData)
    {
        error("sending req to function");

        // JSON string to pass in the GET request
        string queryString = $"?jsonString={Uri.EscapeDataString(jsonData)}";

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("x-functions-key", defaultKey);

        try
        {
            // Send a GET request with the JSON data as a query parameter
            var response = await client.GetAsync($"{functionUrl}{queryString}");

            if (response.StatusCode != HttpStatusCode.OK)
            {
                error($"response code wasnt successful: {response.StatusCode}");
            }
            else
            {
                error("didnt error");
            }           
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            error($"Error retrieving data from database: {e.Message}; {functionUrl}; {queryString}");
            throw new Exception($"Error retrieving data from database: {e.Message}");
        }
    }

}

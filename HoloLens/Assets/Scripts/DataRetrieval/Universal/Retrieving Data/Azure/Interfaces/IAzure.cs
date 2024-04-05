using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IAzure
{
    public string functionKey { get; set; }
    public string functionUrl { get; set; }
    public string defaultKey { get; set; }

    static async Task<HttpResponseMessage> Get(string queryString, string functionUrl, string defaultKey, Action<string> logError)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("x-functions-key", defaultKey);

        try
        {
            logError("Searching for data");
            return await client.GetAsync($"{functionUrl}?{queryString}");
        }
        catch (Exception e)
        {
            logError($"Error retrieving data from database: {e.Message}; {functionUrl}; {queryString}");
            throw new Exception($"Error retrieving data from database: {e.Message}");
        }
    }

    static async Task<HttpResponseMessage> Post(string body, string queries, string functionUrl, string defaultKey, Action<string> logError)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("x-functions-key", defaultKey);

        try
        {
            string queryUrl = $"{functionUrl}?{queries}";
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            return await client.PostAsync(queryUrl, content);
        }
        catch (Exception e)
        {
            logError($"Error posting data: {e.Message}; {functionUrl}");
            throw new Exception($"Error posting data: {e.Message}");
        }
    }
}

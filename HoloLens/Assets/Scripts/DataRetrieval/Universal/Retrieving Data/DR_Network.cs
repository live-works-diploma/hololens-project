using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DR_Network<T> : IDataRetrieval<T>, IJsonHandler<T> where T : class
{
    Func<Dictionary<string, string>, Type, T> howToBuildTask;

    string url;

    public DR_Network(Func<Dictionary<string, string>, Type, T> howToBuildTask, string url)
    {
        if (howToBuildTask == null)
        {
            throw new Exception("How to build task can't be null");
        }

        this.howToBuildTask = howToBuildTask;
        this.url = url;
    }

    Dictionary<string, Type> expectedTypes = new Dictionary<string, Type>();
    public void SetExpectedTypes(Dictionary<string, Type> typesToListenFor)
    {
        expectedTypes = typesToListenFor;
    }

    public async void Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData)
    {
        callWhenFoundData?.Invoke(await ConvertJson(await RetrieveJson()));
    }

    async Task<Dictionary<string, List<T>>> ConvertJson(string json)
    {
        return IJsonHandler<T>.BuildData(json, howToBuildTask, expectedTypes);
    }

    public async Task<string> RetrieveJson()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Send the request and wait for the response asynchronously
            webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"Error Retrieving json from network: {webRequest.error}");
            }

            // Get the downloaded data as a string
            string jsonString = webRequest.downloadHandler.text;
            return jsonString;
        }        
    }
}

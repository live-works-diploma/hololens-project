using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DR_Azure<T> : IDataRetrieval<T>, IJsonHandler<T> where T : class
{
    Func<Dictionary<string, string>, Type, T> howToBuildTask;

    Dictionary<string, Type> expectedTypes = new Dictionary<string, Type>();
    public void SetExpectedTypes(Dictionary<string, Type> typesToListenFor)
    {
        expectedTypes = typesToListenFor;
    }

    public DR_Azure(Func<Dictionary<string, string>, Type, T> howToBuildTask)
    {
        if (howToBuildTask == null)
        {
            throw new Exception("How to build task can't be null");
        }

        this.howToBuildTask = howToBuildTask;
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
        string azureUrl = "Your Azure Function URL";
        using UnityWebRequest website = UnityWebRequest.Get(azureUrl);

        website.SetRequestHeader("Authorization", "Bearer YourAccessToken");    // need to replace with whats needed

        website.SendWebRequest();

        while (!website.isDone)
        {

        }

        if (website.result != UnityWebRequest.Result.Success)
        {
            throw new Exception($"error connecting to Azure: {website.error}");
        }

        return website.downloadHandler.text;
    }
}

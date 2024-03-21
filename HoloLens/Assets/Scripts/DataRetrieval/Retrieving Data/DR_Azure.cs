using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DR_Azure<T> : IDataRetrieval<T>, IJsonHandler<T> where T : class
{
    public async void Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData, Func<Dictionary<string, string>, Type, T> howToBuildTask, Func<T, Dictionary<string, string>> howToTurnIntoDictionary = null, Func<Type, T> buildDefaultData = null)
    {
        // used by class which has an instance of this class
        string json = await RetrieveJson(null, null);
        Dictionary<string, List<T>> builtData = IJsonHandler<T>.BuildData(json, howToBuildTask, expectedTypes);

        callWhenFoundData?.Invoke(builtData);
    }

    public async Task<string> RetrieveJson(Func<Type, T> howToBuildTask, Func<T, Dictionary<string, string>> howToTurnIntoDictionary)
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

    Dictionary<string, Type> expectedTypes = new Dictionary<string, Type>();
    public void SetExpectedTypes(Dictionary<string, Type> typesToListenFor)
    {
        expectedTypes = typesToListenFor;
    }
}

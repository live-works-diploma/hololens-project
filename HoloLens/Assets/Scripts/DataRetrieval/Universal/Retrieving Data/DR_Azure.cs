using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DR_Azure<T> : IDataRetrieval<T>, IJsonHandler<T> where T : class
{
    string azureUrl;
    string authentication;
    string accessToken;

    Func<Dictionary<string, string>, Type, T> howToBuildTask;

    Dictionary<string, Type> expectedTypes = new Dictionary<string, Type>();
    public void SetExpectedTypes(Dictionary<string, Type> typesToListenFor)
    {
        expectedTypes = typesToListenFor;
    }

    public DR_Azure(Func<Dictionary<string, string>, Type, T> howToBuildTask, string azureUrl, string authentication, string accessToken)
    {
        if (howToBuildTask == null)
        {
            throw new Exception("How to build task can't be null");
        }

        this.howToBuildTask = howToBuildTask;

        this.azureUrl = azureUrl;
        this.authentication = authentication;
        this.accessToken = accessToken;
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
        using (UnityWebRequest website = UnityWebRequest.Get(azureUrl))
        {
            website.SetRequestHeader(authentication, accessToken);

            website.SendWebRequest();

            while (!website.isDone)
            {
                await Task.Delay(0); // Yield the coroutine briefly
            }

            if (website.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"error connecting to Azure: {website.error}");
            }

            return website.downloadHandler.text;
        }
    }
}

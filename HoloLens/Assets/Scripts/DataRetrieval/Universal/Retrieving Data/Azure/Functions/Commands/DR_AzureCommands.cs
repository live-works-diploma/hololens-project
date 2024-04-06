using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DR_AzureCommands
{
    public string functionKey;
    public string functionUrl;
    public string defaultKey;

    public async Task SendCommand(string commandName)
    {
        Dictionary<string, string> body = new Dictionary<string, string>();
        body["command"] = commandName;

        string jsonBody = JsonConvert.SerializeObject(body);

        await AzureFunctionRequestHandler.Post(jsonBody, "", functionUrl, defaultKey, null);
    }
}

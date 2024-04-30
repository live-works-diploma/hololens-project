using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockIDataHandler : IDataHandler
{
    public string name { get; set; }

    public Dictionary<string, object> data = new();

    public Dictionary<string, object> CreateDefaultData(float heightAlter, string name, float maxDistanceToSpawn = 30)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "type", this.GetType().Name }
        };

        return data;
    }

    public void FillData(Dictionary<string, object> dataNeeded)
    {
        data = dataNeeded;
    }

    public Dictionary<string, object> TurnDataIntoDictionary()
    {
        return data;
    }
}

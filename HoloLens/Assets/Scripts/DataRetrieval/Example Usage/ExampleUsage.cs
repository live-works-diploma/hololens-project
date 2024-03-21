using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    IDRHandler<IDataHandler> dataRetrieval;

    void Start()
    {
        dataRetrieval = GetComponent<IDRHandler<IDataHandler>>();

        dataRetrieval.AddListener<Plant>(ListenerReturn);
        dataRetrieval.AddListener<Sensor>(ListenerReturn);
    }

    void ListenerReturn(List<IDataHandler> foundData)
    {
        for (int i = 0; i < foundData.Count; i++)
        {
            print($"type: {foundData[i].GetType().Name}");
        }
    }
}

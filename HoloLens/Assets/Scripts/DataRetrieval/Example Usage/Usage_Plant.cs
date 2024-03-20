using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usage_Plant : MonoBehaviour, IExampleUsage
{
    IDRHandler<IDataHandler> dataRetrieval;

    void Start()
    {
        dataRetrieval = GetComponent<IDRHandler<IDataHandler>>();

        dataRetrieval.AddListener<Plant>(ListenerReturn);
    }

    void ListenerReturn(List<IDataHandler> foundData)
    {
        print(foundData.Count);
    }
}

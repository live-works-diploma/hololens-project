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
    }

    void ListenerReturn(List<IDataHandler> foundData)
    {
        print(foundData.Count);
    }
}

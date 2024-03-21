using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just a Demo class. It listens for any data involving Plant or Sensor and then a method gets called which iterates over the found data and prints the type.
/// </summary>
public class ExampleUsage : MonoBehaviour
{
    IDRHandler<IDataHandler> dataRetrieval;

    void Start()
    {
        dataRetrieval = GetComponent<IDRHandler<IDataHandler>>();

        if (dataRetrieval == null )
        {
            throw new System.Exception("There is no class attached to object which implements needed interface");
        }

        // You enter the type of Class you are listening for and when it finds data which has the same name as the class name, it will call the method passed as an agument.
        // the method that you pass has to take List<T> as an argument and no more then that. T is whatever data retrival class is using as its generic, in this case
        // it's IDataHandler.
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

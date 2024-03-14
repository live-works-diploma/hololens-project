using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public delegate void RetrievedDataDelegate<T>(List<T> retrievedData);


public abstract class Data_Base : MonoBehaviour
{
    internal delegate void DataRetrival(Dictionary<string, List<Dictionary<string, string>>> foundData);

    [Tooltip("gets invoked when ever there is new data involving plants - passes a List<Plant> in as an argument")]
    public RetrievedDataDelegate<Plant> plantDataDelegate;

    [Tooltip("gets invoked when ever there is new data involving sensors - passes a List<Sensor> in as an argument")]
    public RetrievedDataDelegate<Sensor> sensorDataDelegate;

    [SerializeField] float timeInbetweenCalls = 5;
    [SerializeField] float delayBeforeFirstCall = 1;

    int _anchors = 0;
    /// <summary>
    /// Acts as an anchor to pause the routine until all dependent processes have finished.
    /// Increase this counter to pause the routine, and decrease it when the processes are complete to resume.
    /// Won't resume until anchors is 0
    /// </summary>
    public int anchors
    {
        get
        {
            return _anchors;
        }
        set
        {
            _anchors = value;

            if (_anchors < 0)
            {
                print("Error: anchors below 0 - shouldn't happen");
            }

            if (_anchors <= 0)
            {
                StartCoroutine(RetrieveDataRoutine(timeInbetweenCalls));
            }
        }
    }

    /// <summary>
    /// Func for showing how to create a Plant class
    /// </summary>
    Func<Dictionary<string, string>, Plant> plantFunc = data => new Plant
    {
        locationX = data.ContainsKey("locationX") ? float.Parse(data["locationX"]) : 0,
        locationY = data.ContainsKey("locationY") ? float.Parse(data["locationY"]) : 0,
        locationZ = data.ContainsKey("locationZ") ? float.Parse(data["locationZ"]) : 0,

        scale = data.ContainsKey("scale") ? float.Parse(data["scale"]) : 1,
        fruiting = data.ContainsKey("fruiting") ? bool.Parse(data["fruiting"]) : false,
    };

    /// <summary>
    /// Func for showing how to create a Sensor class
    /// </summary>
    Func<Dictionary<string, string>, Sensor> sensorFunc = data => new Sensor
    {
        locationX = data.ContainsKey("locationX") ? float.Parse(data["locationX"]) : 0,
        locationY = data.ContainsKey("locationY") ? float.Parse(data["locationY"]) : 0,
        locationZ = data.ContainsKey("locationZ") ? float.Parse(data["locationZ"]) : 0,

        waterLevel = data.ContainsKey("water level") ? float.Parse(data["water level"]) : 0,
        humidity = data.ContainsKey("humidity") ? float.Parse(data["humidity"]) : 0,
    };

    void Start()
    {
        StartCoroutine(RetrieveDataRoutine(delayBeforeFirstCall));
        StartArg();
    }

    /// <summary>
    /// Call when you want to reset the process of retrieving data - doesn't avoid the delay before the call
    /// </summary>
    public void RestartDataRetrieveRoutine()
    {
        StopCoroutine(RetrieveDataRoutine(delayBeforeFirstCall));
        StartCoroutine(RetrieveDataRoutine(delayBeforeFirstCall));
    }

    /// <summary>
    /// routine which calls a RetrieveData(), pauses, and then repeats infinitely. To Reset call RestartDataRetrieveRoutine()
    /// </summary>
    /// <returns></returns>
    IEnumerator RetrieveDataRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);       
        GetData(RetrieveData);
    }

    /// <summary>
    /// Processes the retrieved data to create instances of Plant and Sensor classes
    /// and calls the delegates associated with each type of data to handle the created instances.
    /// </summary>
    /// <param name="allData">A dictionary containing all the retrieved data.</param>
    void RetrieveData(Dictionary<string, List<Dictionary<string, string>>> allData)
    {
        if (plantDataDelegate != null)
        {
            StartCoroutine(CreateData(allData["plant"], plantDataDelegate, plantFunc));
        }

        if (sensorDataDelegate != null)
        {
            StartCoroutine(CreateData(allData["sensor"], sensorDataDelegate, sensorFunc));
        }
    }

    /// <summary>
    /// abstract method which is called by RetrieveData(). This should return everything about what is needed (Plants and Sensors)
    /// </summary>
    /// <param name="foundData">
    /// the delegate that is called when data has been retrieved. it is like this to avoid any race conditions that may occur and allows flexibility
    /// </param>
    internal abstract void GetData(DataRetrival foundData);

    /// <summary>
    /// Converts data retrieved from a dictionary into a list of objects of a specified class,
    /// and then invokes a delegate with that list as an argument.
    /// </summary>
    /// <typeparam name="T">The class of objects to create a list of.</typeparam>
    /// <param name="data">A list of dictionaries containing the data for creating the objects.</param>
    /// <param name="delegateToCall">The delegate to be invoked with the list of objects.</param>
    /// <param name="howToCreateItem">A function that specifies how to create an object from a dictionary entry.</param>
    /// <returns>An enumerator for coroutine execution.</returns>
    IEnumerator CreateData<T>(List<Dictionary<string, string>> data, 
        RetrievedDataDelegate<T> delegateToCall, 
        Func<Dictionary<string, string>, T> howToCreateItem)
    {
        anchors++;

        List<T> allItems = new();

        float maxToCreateAtOnce = 100;

        for (int i = 0; i < data.Count; i++)
        {
            T item = howToCreateItem(data[i]);

            allItems.Add(item);

            if ((i + 1) % maxToCreateAtOnce == 0)
            {
                // allowing the process to be broken up
                yield return null;
            }
        }

        delegateToCall?.Invoke(allItems);
        anchors--;
    }

    internal virtual void StartArg()
    {

    }
}

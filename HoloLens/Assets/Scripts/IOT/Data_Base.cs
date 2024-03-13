using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public delegate void RetrievedDataDelegate<T>(List<T> retrievedData);

public abstract class Data_Base : MonoBehaviour
{
    [Tooltip("gets invoked when ever there is new data involving plants - passes a List<Plant> in as an argument")]
    public RetrievedDataDelegate<Plant> plantDataDelegate;

    [Tooltip("gets invoked when ever there is new data involving sensors - passes a List<Sensor> in as an argument")]
    public RetrievedDataDelegate<Sensor> sensorDataDelegate;

    [SerializeField] float timeInbetweenCalls = 5;
    [SerializeField] float delayBeforeFirstCall = 1;

    void Start()
    {
        StartCoroutine(RetrieveDataRoutine());
        StartArg();
    }

    /// <summary>
    /// Call when you want to reset the process of retrieving data - doesn't avoid the delay before the call
    /// </summary>
    public void RestartDataRetrieveRoutine()
    {
        StopCoroutine(RetrieveDataRoutine());
        StartCoroutine(RetrieveDataRoutine());
    }

    /// <summary>
    /// routine which calls a RetrieveData(), pauses, and then repeats infinitely. To Reset call RestartDataRetrieveRoutine()
    /// </summary>
    /// <returns></returns>
    IEnumerator RetrieveDataRoutine()
    {
        yield return new WaitForSeconds(delayBeforeFirstCall);

        while (true)
        {
            RetrieveData(GetData());
            yield return new WaitForSeconds(timeInbetweenCalls);
        }
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
            Func<Dictionary<string, string>, Plant> function = data => new Plant
            {

            };

            StartCoroutine(CreateData(allData["plant"], plantDataDelegate, function));
        }

        if (sensorDataDelegate != null)
        {
            Func<Dictionary<string, string>, Sensor> function = data => new Sensor
            {
                locationX = data.ContainsKey("locationX") ? float.Parse(data["locationX"]) : 0,
                locationY = data.ContainsKey("locationY") ? float.Parse(data["locationY"]) : 0,
                locationZ = data.ContainsKey("locationZ") ? float.Parse(data["locationZ"]) : 0,

                waterLevel = data.ContainsKey("water level") ? float.Parse(data["water level"]) : 0,
                humidity = data.ContainsKey("humidity") ? float.Parse(data["humidity"]) : 0,
            };

            StartCoroutine(CreateData(allData["sensor"], sensorDataDelegate, function));
        }
    }

    /// <summary>
    /// abstract method which is called by RetrieveData(). This should return everything about what is needed (Plants and Sensors)
    /// </summary>
    /// <returns></returns>
    internal abstract Dictionary<string, List<Dictionary<string, string>>> GetData();

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
        List<T> allItems = new();

        float maxToCreateAtOnce = 20;

        for (int i = 0; i < data.Count; i++)
        {
            T item = howToCreateItem(data[i]);

            allItems.Add(item);

            if ((i + 1) % maxToCreateAtOnce == 0)
            {
                // allowing the process to be broken up
                print($"iteration: {i}");
                yield return null;
            }
        }

        delegateToCall?.Invoke(allItems);
    }

    internal virtual void StartArg()
    {

    }
}

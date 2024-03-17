using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.FilePathAttribute;




public abstract class Data_Base : MonoBehaviour
{
    internal delegate void DataRetrival(Dictionary<string, List<Dictionary<string, string>>> foundData);

    [SerializeField] float timeInbetweenCalls = 5;
    [SerializeField] float delayBeforeFirstCall = 1;
    
    Dictionary<string, List<ICreation>> listeners = new();

    int _anchors = 0;
    /// <summary>
    /// Used as a trigger to start the next call. Increase when you have a process that needs to finish before the next call starts and then decrease when 
    /// its finished. If you increase without decreasing it will stop this script from working and will cause silent errors if decrease without 
    /// increasing first.
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
                print("reset");
                StartCoroutine(RetrieveDataRoutine(timeInbetweenCalls));
            }

            print(_anchors);
        }
    }

    void Start()
    {
        StartCoroutine(RetrieveDataRoutine(delayBeforeFirstCall));
        StartArg();
    }

    /// <summary>
    /// A way to reset the process of retrieving data. May lead to errors if you restart straight after its found data since there will be scripts
    /// which are increasing / decreasing anchors which also reset the process. Change the delay time in the inspector instead of calling this method as
    /// an alternative. Use with care.
    /// </summary>
    public void RestartDataRetrieveRoutine()
    {
        StopCoroutine(RetrieveDataRoutine(delayBeforeFirstCall));
        StartCoroutine(RetrieveDataRoutine(delayBeforeFirstCall));
    }

    /// <summary>
    /// Use to make your script listen for any key words that are found in retrieving any data. Needs to implement ICreation so when finished it calls 
    /// CreateDataRoutine and passes in the found data and lets your script do what you want with that data.
    /// </summary>
    /// <param name="classCalled">The script which has ICreation interface attached to it</param>
    /// <param name="nameToListenFor">The key which this script is listening for.</param>
    public void AddListener(ICreation classCalled, string nameToListenFor)
    {
        if (!listeners.ContainsKey(nameToListenFor))
        {
            listeners[nameToListenFor] = new List<ICreation>();
        }

        listeners[nameToListenFor].Add(classCalled);
    }

    /// <summary>
    /// Starts the process for retrieving data.
    /// </summary>
    /// <param name="delay">how long you want to wait before it starts the routine</param>
    /// <returns></returns>
    IEnumerator RetrieveDataRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);       
        GetData(RetrieveData);
    }

    /// <summary>
    /// Takes in the data that has been retrieved and then iterates over each of those keys and passes that data into listeners who are listening for that key.
    /// </summary>
    /// <param name="allData">The data that has been found.</param>
    void RetrieveData(Dictionary<string, List<Dictionary<string, string>>> allData)
    {
        anchors++;

        foreach (var key in allData.Keys)
        {
            List<ICreation> data = listeners[key];

            for (int i = 0; i < data.Count; i++)
            {
                StartCoroutine(data[i].CreateDataRoutine(allData[key]));
            }
        }

        anchors--;
    }

    /// <summary>
    /// Gets data and then returns that data. For Data_Dummy it just generates random data and then returns that data.
    /// </summary>
    /// <param name="foundData">The data which has been found.</param>
    internal abstract void GetData(DataRetrival foundData);

    /// <summary>
    /// A way for child classes to implement there own Start method whichout causing racetime issues.
    /// </summary>
    internal virtual void StartArg()
    {

    }
}

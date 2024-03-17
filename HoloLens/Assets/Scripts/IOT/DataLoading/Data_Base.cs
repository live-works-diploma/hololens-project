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

    public void RestartDataRetrieveRoutine()
    {
        StopCoroutine(RetrieveDataRoutine(delayBeforeFirstCall));
        StartCoroutine(RetrieveDataRoutine(delayBeforeFirstCall));
    }

    public void AddListener(ICreation classCalled, string nameToListenFor)
    {
        if (!listeners.ContainsKey(nameToListenFor))
        {
            listeners[nameToListenFor] = new List<ICreation>();
        }

        listeners[nameToListenFor].Add(classCalled);
    }

    IEnumerator RetrieveDataRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);       
        GetData(RetrieveData);
    }

    void RetrieveData(Dictionary<string, List<Dictionary<string, string>>> allData)
    {
        anchors++;

        foreach (var key in allData.Keys)
        {
            List<ICreation> data = listeners[key];

            anchors += data.Count;

            for (int i = 0; i < data.Count; i++)
            {
                StartCoroutine(data[i].CreateDataRoutine(allData[key]));
            }
        }

        anchors--;
    }

    internal abstract void GetData(DataRetrival foundData);

    internal virtual void StartArg()
    {

    }
}

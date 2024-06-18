using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DRU_SensorInstance : MonoBehaviour
{
    IDRInteractor<IDataHandler> interactor;
    [SerializeField] DataTypes.DatabaseTypes orderKey;
    public string OrderKey => orderKey.ToString();

    List<SensorFieldLinkSingle> sensorFields = new List<SensorFieldLinkSingle>();

    void Start()
    {
        interactor = GetComponent<IDRInteractor<IDataHandler>>();
        interactor.AddListener<TelemetryData>(ListenerForData);

        sensorFields = FindObjectsOfType<SensorFieldLinkSingle>().ToList();
    }

    void ListenerForData(List<IDataHandler> foundData)
    {
        Func<IDataHandler, int> order = instance =>
        {
            Dictionary<string, object> data = instance.TurnDataIntoDictionary();

            if (!data.ContainsKey(OrderKey))
            {
                Debug.LogError($"data doesn't contain '{OrderKey}' key. Can't order properly");
                return 0;
            }

            int id = int.Parse(data[OrderKey].ToString());

            return id;
        };

        IDataHandler largest = foundData.OrderByDescending(order).First();

        Dictionary<string, object> instanceData = largest.TurnDataIntoDictionary();

        foreach (var kvp in instanceData) 
        {
            List<SensorFieldLinkSingle> neededFields = sensorFields.FindAll(instance => instance.FieldWanted == kvp.Key);

            for (int i = 0; i < neededFields.Count; i++)
            {
                neededFields[i].AssignKey(kvp.Key);
                neededFields[i].AssignValue(kvp.Value.ToString());
            }
        }
    }
}

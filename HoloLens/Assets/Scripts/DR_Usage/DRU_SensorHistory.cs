using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DRU_SensorHistory : MonoBehaviour
{
    IDRInteractor<IDataHandler> interactor;

    [SerializeField] DataTypes.DatabaseTypes orderKey = DataTypes.DatabaseTypes.id;
    public string OrderKey
    {
        get => orderKey.ToString();
    }

    List<SensorHistoryInstance> historyInstances = new List<SensorHistoryInstance>();

    void Start()
    {
        interactor = GetComponent<IDRInteractor<IDataHandler>>();
        interactor.AddListener<TelemetryData>(ListenerForData);

        historyInstances = FindObjectsOfType<SensorHistoryInstance>().ToList();
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

        List<IDataHandler> largest = foundData.OrderByDescending(order).ToList();

        for (int i = 0; i < historyInstances.Count; i++)
        {
            historyInstances[i].ShowData(largest);
        }
    }
}

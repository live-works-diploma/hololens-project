using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRU_Sensors : MonoBehaviour
{
    IInteractor<IDataHandler> interactor;

    [SerializeField] GameObject sensorPrefab;
    [SerializeField] GameObject startingLocation;
    [SerializeField] GameObject fieldPrefab;

    Dictionary<string, GameObject> sensorsCreated = new();
    

    void Start()
    {
        interactor = GetComponent<IInteractor<IDataHandler>>();
        interactor.AddListener<Sensor>(this, ListenForData);
    }

    void ListenForData(List<IDataHandler> foundItems)
    {
        interactor.AlterAnchors(1);

        if (!Application.isPlaying)
        {
            return;
        }

        for (int i = 0; i < foundItems.Count; i++)
        {
            string sensorName = foundItems[i].name;

            if (sensorsCreated.ContainsKey(sensorName))
            {
                continue;
            }
            else
            {
                Dictionary<string, string> sensorData = IDataHandler.howToTurnIntoDictionary(foundItems[i]);
                CreateSensor(sensorName, sensorData);
            }
        }

        interactor.AlterAnchors(-1);
    }

    void CreateSensor(string name, Dictionary<string, string> sensorData)
    {
        GameObject newSensor = Instantiate(sensorPrefab);
        newSensor.name = name;

        SensorControl sensorControl = newSensor.GetComponent<SensorControl>();
        sensorControl.CreateFields(sensorData, name);

        sensorsCreated[name] = newSensor;
    }

    void UpdateSensor(string name)
    {

    }
}

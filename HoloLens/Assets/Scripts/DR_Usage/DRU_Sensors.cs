using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DRU_Sensors : MonoBehaviour
{
    IDRInteractor<IDataHandler> interactor;

    [SerializeField] GameObject sensorPrefab;
    [SerializeField] GameObject startingLocation;
    [SerializeField] GameObject fieldPrefab;

    public List<GameObject> preCreatedSensors = new List<GameObject>();

    Dictionary<string, GameObject> sensorsCreated = new();
    

    void Start()
    {
        interactor = GetComponent<IDRInteractor<IDataHandler>>();

        interactor.AddListener<TelemetryData>(ListenerForData);
        // interactor.AddListener<Sensor>(ListenerForData);
        // interactor.AddListener<Plant>(ListenerForData);

        for (int i = 0; i < preCreatedSensors.Count; i++)
        {
            sensorsCreated[preCreatedSensors[i].name] = preCreatedSensors[i];
        }
    }

    async void ListenerForData(List<IDataHandler> foundItems)
    {
        if (!Application.isPlaying)
        {
            return;
        }


        interactor.AlterAnchors(1);

        // for (int i = foundItems.Count - 1; i < foundItems.Count; i++)
        for (int i = 0; i < foundItems.Count; i++)
        {
            string sensorName = foundItems[i].name;
            Dictionary<string, string> sensorData = foundItems[i].TurnDataIntoDictionary();

            if (sensorsCreated.ContainsKey(sensorName))
            {
                await UpdateSensor(sensorName, sensorData);
                continue;
            }

            await CreateSensor(sensorName, sensorData);
        }

        interactor.AlterAnchors(-1);
    }

    async Task CreateSensor(string name, Dictionary<string, string> sensorData)
    {
        Vector3 newStartingLocation = startingLocation.transform.position;
        newStartingLocation.x = 1.325f * sensorsCreated.Count;  // makes it so the sensors dont overlap

        GameObject newSensor = Instantiate(sensorPrefab, newStartingLocation, startingLocation.transform.rotation, startingLocation.transform);
        newSensor.name = name;

        SensorControl sensorControl = newSensor.GetComponent<SensorControl>();
        await sensorControl.CreateFields(sensorData, name);

        sensorsCreated[name] = newSensor;
    }

    async Task UpdateSensor(string name, Dictionary<string, string> sensorData)
    {
        GameObject createdSensor = sensorsCreated[name];

        SensorControl sensorControl = createdSensor.GetComponent<SensorControl>();
        await sensorControl.UpdateFields(sensorData, name);
    }
}

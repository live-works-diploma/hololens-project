using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DRU_Sensors : MonoBehaviour
{
    IInteractor<IDataHandler> interactor;

    [SerializeField] GameObject sensorPrefab;
    [SerializeField] GameObject startingLocation;
    [SerializeField] GameObject fieldPrefab;

    public List<GameObject> preCreatedSensors = new List<GameObject>();

    Dictionary<string, GameObject> sensorsCreated = new();
    

    void Start()
    {
        interactor = GetComponent<IInteractor<IDataHandler>>();
        interactor.AddListener<TelemetryData>(this, ListenForData);
        // interactor.AddListener<Sensor>(this, ListenForData);

        for (int i = 0; i < preCreatedSensors.Count; i++)
        {
            sensorsCreated[preCreatedSensors[i].name] = preCreatedSensors[i];
        }
    }

     async void ListenForData(List<IDataHandler> foundItems)
    {
        interactor.AlterAnchors(1);

        if (!Application.isPlaying)
        {
            return;
        }

        for (int i = 0; i < foundItems.Count; i++)
        {
            string sensorName = foundItems[i].name;
            Dictionary<string, string> sensorData = IDataHandler.howToTurnIntoDictionary(foundItems[i]);

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
        newStartingLocation.x = 1.325f * sensorsCreated.Count;

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

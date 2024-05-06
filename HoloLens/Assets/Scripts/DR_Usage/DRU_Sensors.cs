using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        for (int i = 0; i < preCreatedSensors.Count; i++)
        {
            sensorsCreated[preCreatedSensors[i].name] = preCreatedSensors[i];
        }
    }

    async void ListenerForData(List<IDataHandler> foundItems)
    {
        if (!Application.isPlaying)
        {
            print("application isnt playing");
            return;
        }

        print("Found Data");

        interactor.AlterAnchors(1);

        // uncheck to just do the last
        var item = foundItems.Last();

        string sensorName = item.name;
        Dictionary<string, object> sensorData = item.TurnDataIntoDictionary();

        if (sensorsCreated.ContainsKey(sensorName))
        {
            await UpdateSensor(sensorName, sensorData);
        }
        else
        {
            await CreateSensor(sensorName, sensorData);
        }

        // uncheck to do all
        //for (int i = 0; i < foundItems.Count; i++)
        //{
        //    string sensorName = foundItems[i].name;
        //    Dictionary<string, object> sensorData = foundItems[i].TurnDataIntoDictionary();

        //    if (sensorsCreated.ContainsKey(sensorName))
        //    {
        //        await UpdateSensor(sensorName, sensorData);
        //    }
        //    else
        //    {
        //        await CreateSensor(sensorName, sensorData);
        //    }
        //}


        interactor.AlterAnchors(-1);
    }

    async Task CreateSensor(string name, Dictionary<string, object> sensorData)
    {
        Vector3 newStartingLocation = startingLocation.transform.position;
        newStartingLocation.x = 1.325f * sensorsCreated.Count;  // makes it so the sensors dont overlap

        GameObject newSensor = Instantiate(sensorPrefab, newStartingLocation, startingLocation.transform.rotation, startingLocation.transform);
        newSensor.name = name;

        SensorControl sensorControl = newSensor.GetComponent<SensorControl>();
        await sensorControl.CreateFields(sensorData, name);

        sensorsCreated[name] = newSensor;
    }

    async Task UpdateSensor(string name, Dictionary<string, object> sensorData)
    {
        GameObject createdSensor = sensorsCreated[name];

        SensorControl sensorControl = createdSensor.GetComponent<SensorControl>();
        await sensorControl.UpdateFields(sensorData, name);
    }
}

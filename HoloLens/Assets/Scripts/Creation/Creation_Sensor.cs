using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creation_Sensor : MonoBehaviour, ICreation
{
    public GameObject sensorCanvasPrefab;

    Data_Base sensorCreation;

    List<GameObject> sensorList = new();

    void Start()
    {
        sensorCreation = GetComponent<Data_Base>();

        sensorCreation.AddListener(this, "Sensor");
    }

    public IEnumerator CreateDataRoutine(List<Dictionary<string, string>> allData)
    {
        sensorCreation.anchors++;

        int maxToCreate = 30;

        List<Sensor> sensors = new List<Sensor>();

        for (int i = 0; i < allData.Count; i++)
        {
            Sensor sensor = new Sensor();
            sensor.FillData(allData[i]);

            sensors.Add(sensor);

            if ((i + 1) % maxToCreate == 0)
            {
                yield return null;
            }
        }

        StartCoroutine(CreateSensorRoutine(sensors));
    }

    IEnumerator CreateSensorRoutine(List<Sensor> sensorList)
    {
        for (int i = 0; i < this.sensorList.Count; i++)
        {
            Destroy(this.sensorList[i]);
        }

        int maxAmountOfSensorsToCreateAtOnce = 20;

        for (int i = 0; i < sensorList.Count; i++)
        {
            Sensor sensor = sensorList[i];

            Vector3 location = new Vector3(sensor.locationX, sensor.locationY, sensor.locationZ);

            GameObject createdSensor = Instantiate(sensorCanvasPrefab, location, sensorCanvasPrefab.transform.rotation);   

            Sensor sensorComp = createdSensor.AddComponent<Sensor>();
            sensorComp.FillData(sensorList[i].TurnDataIntoDictionary());

            ObjectData_Canvas sensorDataCanvas = createdSensor.GetComponent<ObjectData_Canvas>();
            sensorDataCanvas.SetPositionAndScale(location, sensorCanvasPrefab.transform.localScale);
            
            Dictionary<string, string> fieldsToCreate = new Dictionary<string, string>();

            fieldsToCreate["Water Level :"] = sensor.waterLevel.ToString();
            fieldsToCreate["Wind Level :"] = sensor.windLevel.ToString();
            fieldsToCreate["Humidity :"] = sensor.humidity.ToString();

            sensorDataCanvas.CreateFields(fieldsToCreate);

            this.sensorList.Add(createdSensor);

            if ((i % maxAmountOfSensorsToCreateAtOnce) == 0)
            {
                yield return null;
            }
        }

        sensorCreation.anchors--;
    }

    
}

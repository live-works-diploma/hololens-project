using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creation_Sensor : MonoBehaviour
{
    Data_Base sensorCreation;

    void Start()
    {
        sensorCreation = GetComponent<Data_Base>();

        sensorCreation.sensorDataDelegate += CreateSensors;
    }

    void CreateSensors(List<Sensor> sensorList)
    {
        sensorCreation.anchors++;
        StartCoroutine(CreateSensorRoutine());
    }

    IEnumerator CreateSensorRoutine()
    {
        yield return null;
        sensorCreation.anchors--;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IData
{
    void RetrieveData(RetrievedDataDelegate<Plant> plantDelegate, RetrievedDataDelegate<Sensor> sensorDelegate);

    List<Plant> RetrievePlantData();
    List<Sensor> RetrieveEnviromentData();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRU_Graph : MonoBehaviour
{
    [SerializeField] DataPoints dataPoints;
    IDRInteractor<IDataHandler> interactor;

    void Start()
    {
        interactor = GetComponent<IDRInteractor<IDataHandler>>();

        interactor.AddListener<TelemetryData>(Listener);
    }

    async void Listener(List<IDataHandler> foundData)
    {
        if (!Application.isPlaying)
        {
            print("application isnt playing");
            return;
        }

        if (dataPoints == null)
        {
            print("graph handler not assigned");
            return;
        }

        interactor.AlterAnchors(1);

        List<Vector2> graphCoordinates = new();

        for (int i = 0; i < foundData.Count; i++)
        {
            Dictionary<string, object> sensorData = foundData[i].TurnDataIntoDictionary();

            Vector2 coordinates = new Vector2();
            if (sensorData.ContainsKey("temperature"))
            {
                string value = sensorData["temperature"].ToString();

                print(value);

                coordinates = new Vector2(float.Parse(sensorData["temperature"].ToString()), i);
            }
            else if (sensorData.ContainsKey("temp."))
            {
                string value = sensorData["temp."].ToString();

                print(value);

                coordinates = new Vector2(float.Parse(value), i);
            }
            else
            {
                print("Didnt find temp");
                continue;
            }

            graphCoordinates.Add(coordinates);
        }

        dataPoints.PopulateDataPoints(graphCoordinates);

        interactor.AlterAnchors(-1);
    }
}

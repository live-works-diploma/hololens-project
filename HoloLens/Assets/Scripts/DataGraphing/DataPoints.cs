using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataPoints : MonoBehaviour
{
    private GraphHandler GraphHandler;
    public TextAsset jsonFile;
    public List<Vector2> temperatures = new List<Vector2>();

    private int dataID;
    private string dataType;
    private int dataValue;

    void Start()
    {
        GenerateDummyTemperature();
        PopulateDataPoints(temperatures);
        GraphHandler.UpdateGraph();
    }

    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/" + jsonFile.name);
        DummyTelemetryData data = JsonUtility.FromJson<DummyTelemetryData>(json);

        dataID = data.ID;
        dataType = data.dataType;
        dataValue = data.dataValue;
    }

    public void GenerateDummyTemperature()
    {
        for (int i = 0; i < 10; i++)
        {
            {
                float randomTemp = Random.Range(-20f, 40f);
                Vector2 data = new Vector2(randomTemp, i);
                temperatures.Add(data);
            }
        }
    }
    public void PopulateDataPoints(List<Vector2> datas)
    {
        foreach (Vector2 data in datas)
        {
            GraphHandler.CreatePoint(data);
        }
    }

}

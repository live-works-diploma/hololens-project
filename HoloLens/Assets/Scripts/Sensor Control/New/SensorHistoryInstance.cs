using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SensorHistoryInstance : MonoBehaviour
{
    [SerializeField] DataTypes.DatabaseTypes key;
    public string Key => key.ToString();

    [SerializeField] DataTypes.DatabaseTypes value;
    public string Value => value.ToString();

    [SerializeField] int maxInstancesToShow = 10;
    public int MaxInstancesToShow
    {
        get => maxInstancesToShow;
    }

    [Header("Prefabs")]
    [SerializeField] GameObject spawnLocation;
    [SerializeField] GameObject dataPrefab;
    [SerializeField] ScrollView scrollView;

    [Header("Spawning Control")]
    [SerializeField] float distanceBetweenEachData = 0.1f;

    List<GameObject> shownDataInstances = new List<GameObject>();

    ~SensorHistoryInstance()
    {
        DeleteHistory();
    }

    public void ShowData(List<IDataHandler> data)
    {
        List<IDataHandler> dataToShow = data.Take(MaxInstancesToShow).ToList();

        DeleteHistory();

        for (int i = 0; i < dataToShow.Count; i++)
        {
            Dictionary<string, object> instanceData = dataToShow[i].TurnDataIntoDictionary();

            string prefix = instanceData[Key].ToString();
            string suffix = instanceData[Value].ToString();

            SpawnObject(prefix, suffix);
        }
    }

    void SpawnObject(string prefix, string data)
    {
        if (!Application.isPlaying)
        {
            return;
        }

        Debug.Log($"prefix: {prefix}, data: {data}");

        if (dataPrefab == null)
        {
            Debug.LogError("Need to assign data prefab");
            return;
        }

        if (spawnLocation == null)
        {
            Debug.LogError("Need to assign spawn location");
            return;
        }

        Vector3 location = spawnLocation.transform.position;

        // location = new Vector3(location.x, location.y - ((distanceBetweenEachData * shownDataInstances.Count) * 0.0018485f), location.z);

        GameObject dataInstance = Instantiate(dataPrefab, location, Quaternion.identity, spawnLocation.transform);

        RectTransform rectTransform = dataInstance.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1);
        rectTransform.anchorMax = new Vector2(0.5f, 1);
        rectTransform.anchoredPosition = new Vector2(0, -(distanceBetweenEachData * shownDataInstances.Count));

        SensorFieldLink sensorFieldLink = dataInstance.GetComponent<SensorFieldLink>();

        if (sensorFieldLink == null)
        {
            Debug.LogError("data prefab doesn't have a SensorFieldLink attached");
            return;
        }

        sensorFieldLink.AssignKey(prefix);
        sensorFieldLink.AssignValue(data);

        shownDataInstances.Add(dataInstance);
    }

    void DeleteHistory()
    {
        for (int i = 0; i < shownDataInstances.Count; i++)
        {
            Destroy(shownDataInstances[i]);
        }

        shownDataInstances.Clear();
    }

}

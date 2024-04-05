using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SensorControl : MonoBehaviour
{
    public GameObject fieldPrefab;
    public TextMeshProUGUI title;
    public GameObject locationToCreateFields;

    public List<TextMeshProUGUI> preCreatedFields = new List<TextMeshProUGUI>();

    Dictionary<string, TextMeshProUGUI> fieldsCreated = new();

    float heightDecrease = 0;

    void Start()
    {
        for (int i = 0; i < preCreatedFields.Count; i++)
        {
            fieldsCreated[preCreatedFields[i].name] = preCreatedFields[i];
        }
    }

    public async Task CreateFields(Dictionary<string, string> sensorData, string name)
    {
        title.text = name;

        foreach (var field in sensorData.Keys)
        {
            await CreateField(field, sensorData[field]);
        }
    }

    async Task CreateField(string key, string value) 
    {
        if (fieldsCreated.ContainsKey(key))
        {
            return;
        }

        GameObject createdField = Instantiate(fieldPrefab, Vector3.zero, Quaternion.identity, locationToCreateFields.transform);

        createdField.transform.localPosition = Vector3.zero;

        RectTransform rectTransform = createdField.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1);
        rectTransform.anchorMax = new Vector2(0.5f, 1);
        rectTransform.anchoredPosition = new Vector2(0, heightDecrease);

        heightDecrease -= 25;

        TextMeshProUGUI[] createdChildren = createdField.GetComponentsInChildren<TextMeshProUGUI>();
        createdChildren[0].text = key;
        createdChildren[1].text = value;

        fieldsCreated[key] = createdChildren[1];
    }

    public async Task UpdateFields(Dictionary<string, string> sensorData, string name)
    {
        foreach (var field in sensorData.Keys)
        {
            if (!fieldsCreated.ContainsKey(field))
            {
                await CreateField(field, sensorData[field]);
            }
            else
            {
                TextMeshProUGUI textField = fieldsCreated[field];
                textField.text = sensorData[field];
            }
        }
    }
}

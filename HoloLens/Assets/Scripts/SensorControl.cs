using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SensorControl : MonoBehaviour
{
    public GameObject fieldPrefab;
    public TextMeshProUGUI title;
    public GameObject locationToCreateFields;

    Dictionary<string, TextMeshProUGUI> fieldsCreated = new();

    float heightDecrease = 0;

    public void CreateFields(Dictionary<string, string> sensorData, string name)
    {
        title.text = name;

        foreach (var field in sensorData.Keys)
        {
            CreateField(field, sensorData[field]);
        }
    }

    void CreateField(string key, string value) 
    {
        Vector3 whereToAddFields = locationToCreateFields.transform.position;

        Vector3 position = new Vector3(whereToAddFields.x, whereToAddFields.y, whereToAddFields.z);

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

    public void UpdateFields(Dictionary<string, string> sensorData, string name)
    {
        foreach (var field in sensorData.Keys)
        {
            if (!fieldsCreated.ContainsKey(field))
            {
                CreateField(field, sensorData[field]);
            }
            else
            {
                TextMeshProUGUI textField = fieldsCreated[field];
                textField.text = sensorData[field];
            }
        }
    }
}

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

    Dictionary<string, string> fieldsCreated = new();

    public void CreateFields(Dictionary<string, string> sensorData, string name)
    {
        float heightDecrease = 0;

        title.text = name;

        foreach (var field in sensorData.Keys)
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
            createdChildren[0].text = field;
            createdChildren[1].text = sensorData[field];

            fieldsCreated[field] = sensorData[field];
        }
    }
}

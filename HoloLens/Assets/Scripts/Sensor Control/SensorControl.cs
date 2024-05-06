using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SensorControl : MonoBehaviour
{
    // prefabs
    public GameObject fieldPrefab;
    public TextMeshProUGUI title;
    public GameObject locationToCreateFields;



    // giving options of whats hidden or shown
    public List<string> fieldsToBeShown = new List<string>();

    // keeping track of fields already created so no duplicates
    Dictionary<string, object> fieldData = new();
    Dictionary<string, SensorFieldInfo> fieldsCreated = new();  

    // keeping track of position to place new fields
    public float decreasePerField = 25;
    float heightDecrease = 0;

    public List<GameObject> precreatedFields = new();

    void Start()
    {
        for (int i = 0; i < precreatedFields.Count; i++)
        {
            TextMeshProUGUI[] children = precreatedFields[i].GetComponentsInChildren<TextMeshProUGUI>();

            fieldsCreated[precreatedFields[i].name] = new SensorFieldInfo()
            {
                sensorField = precreatedFields[i].transform.parent.gameObject,

                key = children[0],
                value = children[1],
            };
        }
    }

    public async Task CreateFields(Dictionary<string, object> sensorData, string name)
    {
        if (title)
        {
            title.text = name != null ? name : "No Name";
        }

        fieldData = sensorData;

        foreach (var kvp in fieldData)
        {
            await CreateField(kvp.Key, kvp.Value.ToString());
        }
    }

    async Task CreateField(string key, string value) 
    {
        if (locationToCreateFields == null || fieldPrefab == null || fieldsCreated.ContainsKey(key))
        {
            print($"Field couldnt create: {key}:{value}");
            return;
        }

        await Task.Yield();
        
        GameObject createdField = Instantiate(fieldPrefab, Vector3.zero, Quaternion.identity, locationToCreateFields.transform);

        createdField.transform.localPosition = Vector3.zero;

        RectTransform rectTransform = createdField.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1);
        rectTransform.anchorMax = new Vector2(0.5f, 1);
        rectTransform.anchoredPosition = new Vector2(0, heightDecrease);

        heightDecrease -= decreasePerField;

        TextMeshProUGUI[] createdChildren = createdField.GetComponentsInChildren<TextMeshProUGUI>();
        createdChildren[0].text = key;
        createdChildren[1].text = value;

        var sensorInfo = new SensorFieldInfo()
        {
            sensorField = createdField,

            key = createdChildren[0],
            value = createdChildren[1],

            orderShown = fieldsCreated.Count,
        };

        fieldsCreated[key] = sensorInfo;        
    }

    public async Task UpdateFields(Dictionary<string, object> sensorData, string name)
    {
        foreach (var field in sensorData.Keys)
        {
            if (!fieldsCreated.ContainsKey(field))
            {
                await CreateField(field, sensorData[field].ToString());
            }
            else
            {
                TextMeshProUGUI textField = fieldsCreated[field].value;
                // print("Updating field");
                textField.text = sensorData[field].ToString();
            }
        }
    }

    public void DeleteField(string fieldName)
    {   
        if (!fieldsCreated.ContainsKey(fieldName))
        {
            print($"Field didnt exist: {fieldName}");
            return;
        }

        var fieldToDelete = fieldsCreated[fieldName];

        fieldsCreated.Remove(fieldName);
        heightDecrease += decreasePerField;

        foreach (var kvp in fieldsCreated)
        {
            if (kvp.Value == null)
            {
                throw new System.Exception("Key found in fields created that wasnt created correctly");
            }

            if (fieldToDelete.orderShown < kvp.Value.orderShown)
            {
                kvp.Value.orderShown -= 1;

                RectTransform rectTransform = kvp.Value.sensorField.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + decreasePerField);
            }
        }

        // Destroy(fieldToDelete.sensorField);
        fieldToDelete.sensorField.SetActive(false);
    }

    public async void ShowField(string fieldName)
    {
        if (fieldsCreated.ContainsKey(fieldName))
        {
            var field = fieldsCreated[fieldName].sensorField;

            field.SetActive(true);

            print($"Field already shown: {fieldName}");
            return;
        }

        object newFieldData = fieldData[fieldName];

        await CreateField(fieldName, newFieldData.ToString());
    }

    public void Hide(string name)
    {
        var field = fieldsCreated[name].sensorField;
        field.SetActive(false);
    }

    public void Show(string name)
    {
        var field = fieldsCreated[name].sensorField;
        field.SetActive(true);
    }
}

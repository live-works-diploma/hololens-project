using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectData_Canvas : ObjectData_Tracking
{
    public TextMeshProUGUI title;
    public GameObject fieldPrefab;
    public GameObject whereToAddFields;

    public GameObject placeToKeepButton;
    public GameObject button;

    public void AssignTitle(string title)
    {
        if (title == null)
        {
            throw new System.Exception("title hasnt been set");
        }

        this.title.text = title;
    }

    public void CreateFields(Dictionary<string, string> fieldsToCreate)
    {
        float thingy = 0;
        Vector3 canvasPosition = whereToAddFields.transform.position;

        foreach (var key in fieldsToCreate.Keys)
        {
            Vector3 position = new Vector3(canvasPosition.x, canvasPosition.y, canvasPosition.z);

            GameObject created = Instantiate(fieldPrefab, Vector3.zero, Quaternion.identity, whereToAddFields.transform);

            //created.transform.localPosition = new Vector3(whereToAddFields.transform.position.x, 
            //    whereToAddFields.transform.position.y, 
            //    whereToAddFields.transform.position.z);

            created.transform.localPosition = Vector3.zero;

            RectTransform rectTransform = created.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 1);
            rectTransform.anchorMax = new Vector2(0.5f, 1);
            rectTransform.anchoredPosition = new Vector2(0, thingy);

            thingy -= 25;

            TextMeshProUGUI[] createdChildren = created.GetComponentsInChildren<TextMeshProUGUI>();
            createdChildren[0].text = key;
            createdChildren[1].text = fieldsToCreate[key];
        }
    }

    internal override void StartArg()
    {
        StartCoroutine(PositionButtonRoutine());
    }

    IEnumerator PositionButtonRoutine()
    {
        while (true)
        {
            Quaternion rotation = placeToKeepButton.transform.rotation;
            Vector3 position = placeToKeepButton.transform.position;

            button.transform.position = position;
            button.transform.rotation = rotation;

            yield return null;
        }
    }
}

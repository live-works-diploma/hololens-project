using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creation_Plant : MonoBehaviour, ICreation
{
    Data_Base plantCreation;
    List<GameObject> plantsCreated = new();

    [SerializeField] GameObject defaultPrefab;

    void Start()
    {
        plantCreation = GetComponent<Data_Base>();

        plantCreation.AddListener(this, "Plant");
    }

    public IEnumerator CreateDataRoutine(List<Dictionary<string, string>> allData)
    {
        plantCreation.anchors++;

        int maxToCreate = 30;

        List<Plant> plants = new List<Plant>();

        for (int i = 0; i < allData.Count; i++)
        {
            Plant plant = new Plant();
            plant.FillData(allData[i]);

            plants.Add(plant);

            if ((i + 1) % maxToCreate == 0)
            {
                yield return null;
            }
        }

        StartCoroutine(CreatePlantsRoutine(plants));
    }

    IEnumerator CreatePlantsRoutine(List<Plant> allPlants)
    {
        for (int i = 0; i < plantsCreated.Count; i++)
        {
            Destroy(plantsCreated[i]);
        }

        plantsCreated.Clear();

        int numberOfPlantsToCreateAtOnce = 50;

        for (int i = 0; i < allPlants.Count; i++)
        {
            Plant plant = allPlants[i];

            Vector3 location = new Vector3(plant.locationX, plant.locationY, plant.locationZ);

            GameObject spawnedPlant = Instantiate(plant.prefab ? plant.prefab : defaultPrefab);

            Plant plantComp = spawnedPlant.AddComponent<Plant>();
            plantComp.FillData(allPlants[i].TurnDataIntoDictionary());

            ObjectData spawnedPlantData = spawnedPlant.AddComponent<ObjectData>();
            spawnedPlantData.SetPositionAndScale(location, Vector3.one * plant.scale);

            plantsCreated.Add(spawnedPlant);

            if ((i + 1) % numberOfPlantsToCreateAtOnce == 0)
            {
                yield return null;
            }
        }

        plantCreation.anchors--;
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCreation : MonoBehaviour
{
    Data_Base plantCreation;
    List<GameObject> plantsCreated = new();

    [SerializeField] GameObject defaultPrefab;

    void Start()
    {
        plantCreation = GetComponent<Data_Base>();

        plantCreation.plantDataDelegate += CreatePlants;
    }

    void CreatePlants(List<Plant> allPlants)
    {
        plantCreation.anchors++;
        StartCoroutine(CreatePlantsRoutine(allPlants));
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

            GameObject spawnedPlant = Instantiate(plant.prefab ? plant.prefab : defaultPrefab, location, Quaternion.identity);
            spawnedPlant.transform.localScale = new Vector3(plant.height, plant.height, plant.height);

            plantsCreated.Add(spawnedPlant);

            if ((i + 1) % numberOfPlantsToCreateAtOnce == 0)
            {
                yield return null;
            }
        }

        plantCreation.anchors--;
    }
}

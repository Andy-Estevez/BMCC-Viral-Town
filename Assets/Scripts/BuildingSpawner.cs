using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BuildingSpawner : MonoBehaviour
{
  
    [SerializeField] private GameObject[] officeBuildings; // Array of office building prefabs
    [SerializeField] private GameObject[] residentalBuildings; // Array of residental building prefabs
    [SerializeField] private GameObject[] commercialBuildings; // Array of commercial building prefabs

    [SerializeField] private Transform officeZone;     // Array of all the zone transforms
    [SerializeField] private Transform residentalZone;
    [SerializeField] private Transform commercialZone;
    //[SerializeField] private int[] buildingsPerZone;    // Number of buildings to spawn in each zone

    void Start()
    {

        SpawnBuildings(residentalBuildings, residentalZone, 15);
        SpawnBuildings(officeBuildings, officeZone, 10);
        
        SpawnBuildings(commercialBuildings, commercialZone, 7);

    }

    void SpawnBuildings(GameObject[] prefabs, Transform zone, int amount)
    {
        BoxCollider zoneCollider = zone.GetComponent<BoxCollider> ();
        Bounds zoneBounds = zoneCollider.bounds;


        // Define the spacing between buildings and the number of buildings per row
        float buildingSpacing = 10f; // Adjust this based on the size of your building prefabs and desired street width
        int buildingsPerRow = Mathf.FloorToInt((zoneBounds.size.x - buildingSpacing) / (prefabs[0].transform.localScale.x + buildingSpacing));

        Vector3 startPosition = new Vector3(zoneBounds.min.x, 0, zoneBounds.min.z); // Ground level assumed as '0' for y-axis

        int buildingCount = 0;
        float posX, posZ;

        // Loop to spawn buildings row by row
        for (int i = 0; buildingCount < amount && i < (amount / buildingsPerRow) + 1; i++)
        {
            for (int j = 0; j < buildingsPerRow && buildingCount < amount; j++)
            {
                posX = startPosition.x + j * (prefabs[0].transform.localScale.x + buildingSpacing);
                posZ = startPosition.z + i * (prefabs[0].transform.localScale.z + buildingSpacing);

                // Select a random prefab from the array
                GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

                // Instantiate building at calculated position
                Vector3 spawnPosition = new Vector3(posX, startPosition.y, posZ);
                Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity, zone);

                buildingCount++;
            }
        }
    }

}

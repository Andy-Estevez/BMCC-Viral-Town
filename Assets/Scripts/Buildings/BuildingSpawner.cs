using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BuildingSpawner : MonoBehaviour
{
  
    [SerializeField] private GameObject[] officeBuildings; // Array of office building prefabs
    [SerializeField] private GameObject[] residentalBuildings; // Array of residental building prefabs
    [SerializeField] private GameObject[] commercialBuildings; // Array of commercial building prefabs

    [SerializeField] private Transform[] officeBorder;// Array of all the zone transforms
    [SerializeField] private Transform residentalBorder;
    [SerializeField] private Transform commercialBorder;

    [SerializeField] private Transform officeContainer;     // Array of all the zone transforms
    [SerializeField] private Transform residentalContainer;
    [SerializeField] private Transform commercialContainer;


    //[SerializeField] private int[] buildingsPerZone;    // Number of buildings to spawn in each zone

    void Start()
    {
        /*
        SpawnBuildings(residentalBuildings, residentalContainer, residentalBorder, 1, 18);
        SpawnBuildings(officeBuildings, officeContainer, officeBorder[0], 4/5, 4);
        SpawnBuildings(officeBuildings, officeContainer, officeBorder[1], 4 / 5, 4);
        SpawnBuildings(officeBuildings, officeContainer, officeBorder[2], 4 / 5, 4);
        SpawnBuildings(commercialBuildings, commercialContainer, commercialBorder, 4/5, 18);
        */

        ViralTownEvents.SpawnTown.AddListener(onSpawnTown);
    }

    void onSpawnTown()
    {
        SpawnBuildings(residentalBuildings, residentalContainer, residentalBorder, 1, 18, "res");
        SpawnBuildings(officeBuildings, officeContainer, officeBorder[0], 4 / 5, 4, "off");
        SpawnBuildings(officeBuildings, officeContainer, officeBorder[1], 4 / 5, 4, "off");
        SpawnBuildings(officeBuildings, officeContainer, officeBorder[2], 4 / 5, 4, "off");
        SpawnBuildings(commercialBuildings, commercialContainer, commercialBorder, 4 / 5, 18, "com");

        Town.initResidentialBuildingData();
        Town.initCommercialBuildingData();
    }

    void SpawnBuildings(GameObject[] prefabs, Transform zoneContainer, Transform zoneBorder, float streetWidth, int amount, string type)
    {
        
        float buildingWidth = prefabs[0].transform.localScale.x;

        BoxCollider2D collid = zoneBorder.GetComponent<BoxCollider2D>();

        // The starting position for the first building. Here we set the starting pos to left upper corner of the zone.
        Vector3 nextPosition = new Vector3(collid.bounds.min.x, collid.bounds.max.y, 0);

        // The distance to the next building spot (building width plus street width)
        Vector3 offset = new Vector3(buildingWidth + streetWidth, buildingWidth + streetWidth, 0);

        // Calculate the area of the zone container using collider bounds
        float zoneWidth = collid.bounds.size.x;

        // Handle potential division by zero (assuming 'amount' is number of buildings)
        if (amount == 0)
        {
            Debug.LogError("Number of buildings cannot be zero!");
            return; // Or handle the case with a default value
        }

        // Calculate the number of buildings per row
        int buildingsPerRow = Mathf.FloorToInt((zoneWidth / (buildingWidth + streetWidth))+1); 

        for (int i = 0; i < amount; i++)
        {
            // Calculate new position for each building in a row
            if (i % buildingsPerRow == 0 && i != 0) // After a row is complete, reset to next row
            {
                nextPosition.x = collid.bounds.min.x; // Reset to first column
                nextPosition.y -= offset.y; // Move to next row
                Debug.Log("in the loop");
            }
            else if (i != 0)
            {
                nextPosition.x += offset.x; // Move to next column
            }


            // Instantiate the building at nextPosition
            GameObject newBuilding = Instantiate(prefabs[Random.Range(0, prefabs.Length)], nextPosition, Quaternion.identity);
            Town.insertBuildingIntoList(newBuilding, type);

            // Parent it to the zone container so it's organized correctly in the scene
            newBuilding.transform.SetParent(zoneContainer);
        }
    }

}

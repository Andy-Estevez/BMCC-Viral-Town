using System.Collections.Generic;
using UnityEngine;

// Static helper class that manages high-level, town-related functionality
public static class Town
{
    // Variables (Structs)
    private static TownStatistics stats;
    private static TownBuildings buildings;

    // Properties
    public static int TotalPop => stats.totalPop;
    public static int HealthyPop => stats.healthyPop;
    public static int InfectedPop => stats.infectedPop;
    public static int GDP => stats.gdp;

    // Methods

    // Initializes class
    public static void init(float ort, int hlt, int inf, int gdp)
    {
        // Create structs
        stats = new TownStatistics();
        buildings = new TownBuildings();

        // Initial town statistics
        //stats.totalPop = totalCapacity * ort;
        stats.healthyPop = hlt;
        stats.infectedPop = inf;
        stats.gdp = gdp;

        // Town building lists
        buildings.residentialBuildings = new List <GameObject>();
        buildings.commercialBuildings = new List <GameObject>();
    }

    // Generates town according to zones & blocks
    public static void generateTown() {
        Debug.Log("Loading town...");

        // Generate Town
        // townSpawner.spawn();
    }

    // Updates town's total, healthy, & infected population data
    public static void updatePop()
    {
        stats.totalPop = 0;
        stats.healthyPop = 0;
        stats.infectedPop = 0;

        foreach (GameObject residence in buildings.residentialBuildings) {
            /*BuildingTemplate residenceObj = residence.GetComponent<BuildingTemplate>();

            stats.totalPop += residenceObj.Occupants;
            stats.healthyPop += residenceObj.Healthy;
            stats.infectedPop += residenceObj.Infected;*/
        }
    }

    // Moves population to residential buildings
    public static void movePopToRes()
    {
        // ...
    }

    // Distributes population to commercial buildings
    public static void movePopToCom()
    {
        // ...
    }

    // Iterates through residences & activates their infection propagation
    public static void propagateTownInfections()
    {
        foreach (GameObject residence in buildings.residentialBuildings) {
            //residence.GetComponent<BuildingTemplate>().propagateInfections();
        }
    }

    // Iterates through residences & activates their fatality progagation
    public static void propagateTownFatalities()
    {
        foreach (GameObject residence in buildings.residentialBuildings)
        {
            //residence.GetComponent<BuildingTemplate>().propagateFatalities();
        }
    }
}

struct TownStatistics
{
    // Total population
    public int totalPop;
    // Healthy segment of population
    public int healthyPop;
    // Infected segment of population
    public int infectedPop;
    // Gross Domestic Product
    public int gdp;
}

struct TownBuildings
{
    // List of residential buildings (homes)
    public List <GameObject> residentialBuildings;
    // List of commercial buildings (offices, shops)
    public List <GameObject> commercialBuildings;
    // Hospital reference
    public GameObject hospital;
}

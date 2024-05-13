using System;
using System.Collections.Generic;
using UnityEngine;

// Static helper class that manages high-level, town-related functionality
public static class Town
{
    // Variables (Structs)
    private static TownStatistics stats;
    private static TownBuildings buildings;
    private static BuildingSpawner spawner;

    // Properties
    public static int InitialPop
    {
        get => stats.initPop;
        set => stats.initPop = value;
    }

    public static int CurrentPop
    {
        get => stats.curPop;
        set => stats.curPop = value;
    }

    public static int HealthyPop
    {
        get => stats.healthyPop;
        set => stats.healthyPop = value;
    }

    public static int InfectedPop
    {
        get => stats.infectedPop;
        set => stats.infectedPop = value;
    }

    public static int InitialGDP
    {
        get => stats.initGDP;
        set => stats.initGDP = value;
    }

    public static int CurrentGDP
    {
        get => stats.curGDP;
        set => stats.curGDP = value;
    }


    // Methods

    // Initializes class
    public static void init(int pop, int hlt, int inf, int gdp)
    {
        // Create structs
        stats = new TownStatistics();
        buildings = new TownBuildings();

        // Initial town statistics
        stats.initPop = pop;
        stats.curPop = pop;
        stats.healthyPop = hlt;
        stats.infectedPop = inf;
        stats.initGDP = gdp;
        stats.curGDP = gdp;

        // Town building lists
        buildings.residentialBuildings = new List<GameObject>();
        buildings.commercialBuildings = new List<GameObject>();
        buildings.hospital = GameObject.Find("Hospital");
    }

    // Generates town
    public static void generateTown()
    {
        Debug.Log("Loading town...");

        // Generate Town
        ViralTownEvents.SpawnTown.Invoke();
    }

    // Initializes Building Lists
    public static void insertBuildingIntoList(GameObject prefab, string type)
    {
        if (type == "res")
        {
            buildings.residentialBuildings.Add(prefab);
        }
        else if (type == "off" || type == "com")
        {
            buildings.commercialBuildings.Add(prefab);
        }
        else
        {
            Debug.Log("ERROR: type provided to insertBuildingIntoList() function of Town class is invalid.");
        }
    }

    // Initializes Building Data
    public static void initBuildingData()
    {
        int tempPop = InitialPop;
        int tempInf = InfectedPop;

        foreach (GameObject residence in buildings.residentialBuildings)
        {
            BuildingTemplate residenceScript = residence.GetComponent<BuildingTemplate>();

            int initOccupants = 2;

            if (residence.name == "CondoRed(Clone)" || residence.name == "DuplexBlue(Clone)")
                initOccupants *= 4;

            residenceScript.origOccupants = initOccupants;
            residenceScript.occupants = initOccupants;

            residenceScript.origHealthyOccupants = initOccupants;
            residenceScript.healthyOccupants = initOccupants;

            residenceScript.origInfectedOccupants = 0;
            residenceScript.infectedOccupants = 0;

            tempPop -= initOccupants;
        }

        while (tempPop > 0)
        {
            int randIndex = UnityEngine.Random.Range(0, buildings.residentialBuildings.Count);

            GameObject residence = buildings.residentialBuildings[randIndex];
            BuildingTemplate residenceScript = residence.GetComponent<BuildingTemplate>();

            int initOccupants = 1;

            if (residence.name == "CondoRed(Clone)" || residence.name == "DuplexBlue(Clone)")
                initOccupants *= 4;

            if (initOccupants > tempPop)
                initOccupants = tempPop;

            residenceScript.origOccupants += initOccupants;
            residenceScript.occupants += initOccupants;

            residenceScript.origHealthyOccupants += initOccupants;
            residenceScript.healthyOccupants += initOccupants;

            tempPop -= initOccupants;
        }

        while (tempInf > 0)
        {
            int randIndex = UnityEngine.Random.Range(0, buildings.residentialBuildings.Count);

            GameObject residence = buildings.residentialBuildings[randIndex];
            BuildingTemplate residenceScript = residence.GetComponent<BuildingTemplate>();

            residenceScript.origOccupants++;
            residenceScript.occupants++;

            residenceScript.origInfectedOccupants++;
            residenceScript.infectedOccupants++;

            tempInf--;
        }
    }

    // Updates map prefabs between day & night
    public static void updateMap(string section)
    {
        if (section == "day")
        {

        }
        else if (section == "night")
        {

        }
        else
        {
            Debug.Log("ERROR: Invalid section type passed to updateMap() function of Town class");
        }
    }

    // Updates town's total, healthy, & infected population data
    public static void updatePop()
    {
        stats.curPop = 0;
        stats.healthyPop = 0;
        stats.infectedPop = 0;

        foreach (GameObject residence in buildings.residentialBuildings)
        {
            BuildingTemplate residenceScript = residence.GetComponent<BuildingTemplate>();

            stats.curPop += residenceScript.occupants;
            stats.healthyPop += residenceScript.healthyOccupants;
            stats.infectedPop += residenceScript.infectedOccupants;
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
}

struct TownStatistics
{
    // Initial population
    public int initPop;
    // Current (total) population
    public int curPop;
    // Healthy segment of population
    public int healthyPop;
    // Infected segment of population
    public int infectedPop;
    // Initial Gross Domestic Product
    public int initGDP;
    // Gross Domestic Product
    public int curGDP;
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

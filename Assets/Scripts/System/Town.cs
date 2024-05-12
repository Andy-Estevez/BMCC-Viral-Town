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
    public static int InitialPop => stats.initPop;
    public static int CurrentPop => stats.curPop;
    public static int HealthyPop => stats.healthyPop;
    public static int InfectedPop => stats.infectedPop;
    public static int InitialGDP => stats.initGDP;
    public static int CurrentGDP => stats.curGDP;

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
    public static void updateMap(string section, MapSprites mapBuildingSprites)
    {
        if (section == "day")
        {
            foreach (GameObject residence in buildings.residentialBuildings)
            {
                SpriteRenderer residenceSprite = residence.GetComponent<SpriteRenderer>();

                switch(residence.name)
                {
                    case "CondoRed(Clone)":
                        residenceSprite.sprite = mapBuildingSprites.condoRedDay;
                        break;

                    case "DuplexBlue(Clone)":
                        residenceSprite.sprite = mapBuildingSprites.duplexBlueDay;
                        break;

                    case "House(Clone)":
                        residenceSprite.sprite = mapBuildingSprites.houseDay;
                        break;
                }
            }

            foreach (GameObject commercial in buildings.commercialBuildings)
            {
                SpriteRenderer commercialSprite = commercial.GetComponent<SpriteRenderer>();

                switch (commercial.name)
                {
                    case "OfficeBlue(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.officeBlueDay;
                        break;

                    case "OfficeGrey(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.officeGreyDay;
                        break;

                    case "Restaurant(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.restaurantDay;
                        break;

                    case "Shop(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.shopDay;
                        break;

                    case "ShopCoffee(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.shopCoffeeDay;
                        break;

                    case "ShopDeli(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.shopDeliDay;
                        break;

                    case "ShoppingMall(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.shoppingMallDay;
                        break;

                    case "Supermarket(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.supermarketDay;
                        break;
                }
            }
        }
        else if (section == "night")
        {
            foreach (GameObject residence in buildings.residentialBuildings)
            {
                SpriteRenderer residenceSprite = residence.GetComponent<SpriteRenderer>();

                switch (residence.name)
                {
                    case "CondoRed(Clone)":
                        residenceSprite.sprite = mapBuildingSprites.condoRedNight;
                        break;

                    case "DuplexBlue(Clone)":
                        residenceSprite.sprite = mapBuildingSprites.duplexBlueNight;
                        break;

                    case "House(Clone)":
                        residenceSprite.sprite = mapBuildingSprites.houseNight;
                        break;
                }
            }

            foreach (GameObject commercial in buildings.commercialBuildings)
            {
                SpriteRenderer commercialSprite = commercial.GetComponent<SpriteRenderer>();

                switch (commercial.name)
                {
                    case "OfficeBlue(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.officeBlueNight;
                        break;

                    case "OfficeGrey(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.officeGreyNight;
                        break;

                    case "Restaurant(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.restaurantNight;
                        break;

                    case "Shop(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.shopNight;
                        break;

                    case "ShopCoffee(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.shopCoffeeNight;
                        break;

                    case "ShopDeli(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.shopDeliNight;
                        break;

                    case "ShoppingMall(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.shoppingMallNight;
                        break;

                    case "Supermarket(Clone)":
                        commercialSprite.sprite = mapBuildingSprites.supermarketNight;
                        break;
                }
            }
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

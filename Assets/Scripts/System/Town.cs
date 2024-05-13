using System;
using System.Collections.Generic;
using UnityEditor;
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

    // Decreases GDP when somebody dies
    public static void onDeath(int deathAmount)
    {
        CurrentGDP -= (InitialGDP / InitialPop) * deathAmount;
    }

    // Moves population to residential buildings
    public static void movePopToRes()
    {
        int interimTotal = 0;
        int interimHealthy = 0;
        int interimInfected = 0;
        int atHospitalHealthy = 0;
        int atHospitalInfected = 0;

        // Get number of people at hospital
        BuildingTemplate hospitalScript = buildings.hospital.GetComponent<BuildingTemplate>();
        atHospitalHealthy = hospitalScript.healthyOccupants;
        atHospitalInfected = hospitalScript.infectedOccupants;

        // Scoop up all occupants from commercial buildings
        foreach (GameObject commercial in buildings.commercialBuildings)
        {
            BuildingTemplate buildingScript = commercial.GetComponent<BuildingTemplate>();

            interimTotal += buildingScript.occupants;
            interimHealthy += buildingScript.healthyOccupants;
            interimInfected += buildingScript.infectedOccupants;

            buildingScript.occupants = 0;
            buildingScript.healthyOccupants = 0;
            buildingScript.infectedOccupants = 0;
        }

        // Distribute healthy residents to residentials
        while (interimHealthy > 0)
        {
            int randIndex = UnityEngine.Random.Range(0, buildings.residentialBuildings.Count);

            GameObject curBuilding = buildings.residentialBuildings[randIndex];
            BuildingTemplate buildingScript = curBuilding.GetComponent<BuildingTemplate>();

            if (buildingScript.healthyOccupants < buildingScript.origHealthyOccupants)
            {
                buildingScript.healthyOccupants++;
                interimHealthy--;
            }
        }

        // Distribute infected residents to residentials
        while (interimInfected > 0)
        {
            int randIndex = UnityEngine.Random.Range(0, buildings.residentialBuildings.Count);

            GameObject curBuilding = buildings.residentialBuildings[randIndex];
            BuildingTemplate buildingScript = curBuilding.GetComponent<BuildingTemplate>();

            if (buildingScript.infectedOccupants < buildingScript.origInfectedOccupants)
            {
                buildingScript.infectedOccupants++;
                interimInfected--;
            }
        }

        // Update Building Orig Data
        foreach (GameObject residential in buildings.residentialBuildings)
        {
            BuildingTemplate buildingScript = residential.GetComponent<BuildingTemplate>();

            // If healthy people missing (infectd)
            if (buildingScript.healthyOccupants < buildingScript.origHealthyOccupants)
            {
                buildingScript.origHealthyOccupants = buildingScript.healthyOccupants;
            }

            // If infected people missing (died, healed, or at hospital)
            if (buildingScript.infectedOccupants < buildingScript.origInfectedOccupants)
            {
                buildingScript.origInfectedOccupants = buildingScript.infectedOccupants;
            }

            // If people missing (died, or at hospital)
            if (buildingScript.origOccupants > (buildingScript.healthyOccupants +  buildingScript.infectedOccupants))
            {
                buildingScript.origOccupants = (buildingScript.healthyOccupants + buildingScript.infectedOccupants);
            }
        }

        // Reserve residential spots for hospitalized people
        while (atHospitalInfected > 0)
        {
            int randIndex = UnityEngine.Random.Range(0, buildings.residentialBuildings.Count);

            GameObject curBuilding = buildings.residentialBuildings[randIndex];
            BuildingTemplate buildingScript = curBuilding.GetComponent<BuildingTemplate>();

            buildingScript.origInfectedOccupants++;
            buildingScript.origOccupants++;
            atHospitalInfected--;
        }

        // Move discharged people to residentials
        while (atHospitalHealthy > 0)
        {
            int randIndex = UnityEngine.Random.Range(0, buildings.residentialBuildings.Count);

            GameObject curBuilding = buildings.residentialBuildings[randIndex];
            BuildingTemplate buildingScript = curBuilding.GetComponent<BuildingTemplate>();

            buildingScript.healthyOccupants++;
            buildingScript.occupants++;
            buildingScript.origHealthyOccupants++;
            buildingScript.origOccupants++;
            atHospitalHealthy--;
        }
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

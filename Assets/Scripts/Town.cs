using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public static class Town
{
    // Variables
    private static int population = 10000;
    private static int healthy = 10000;
    private static int infected = 0;
    private static int gdp = 100000000;
    private static Buildings buildings;

    // Properties
    public static int Population { get { return population; } }
    public static int Healthy { get { return healthy; } }
    public static int Infected { get { return infected; } }
    public static int GDP { get { return gdp; } }

    // Methods

    // Generates town according to zones & blocks
    public static void generateTown() {
        Debug.Log("Loading town");

        buildings = new Buildings();

        // Generate

    }
}

public class Buildings
{
    public List <int> residentialBuildings = new List<int>();
    public List <int> commercialBuildings = new List<int>();
    public GameObject hospital;
}

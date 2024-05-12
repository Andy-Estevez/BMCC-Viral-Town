using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class TrackerBuddy : MonoBehaviour
{

    public int InitialGDP = Town.InitialGDP;
    public int InitialPop = Town.InitialPop;

    public int CurrentGDP = Town.CurrentGDP;
    public int CurrentPop = Town.CurrentPop;
    public int HealthyPop = Town.HealthyPop;
    public int InfectedPop = Town.InfectedPop;

    public float infectionChance = Virus.infectionChance;
    public float deathChance = Virus.deathChance;
    public float infectionRate = Virus.infectionRate;
    public float deathRate = Virus.deathRate;

    public float minBuffRate = Virus.minBuffRate;
    public float maxBuffRate = Virus.maxBuffRate;



    //void Awake()
    //{

    //}

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {

        InitialPop = Town.InitialPop;
        InitialGDP = Town.InitialGDP;
        CurrentGDP = Town.CurrentGDP;
        CurrentPop = Town.CurrentPop;
        HealthyPop = Town.HealthyPop;
        InfectedPop = Town.InfectedPop;

    }
}

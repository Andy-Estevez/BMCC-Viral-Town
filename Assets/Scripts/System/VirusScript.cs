using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class VirusScript
{
    // chance is how likely it is to happen
    // rate is how many people it impacts
    public static float infectionChance = 0.20f;
    public static float deathChance = 0.10f;
    public static float infectionRate = 0.20f;
    public static float deathRate = 0.10f;

    public static float minBuffRate = 0.05f;
    public static float maxBuffRate = 0.10f;

    public static float baseBuffRateIncrease = 0.05f;


    public static void InfectionBuff()
    {
        infectionChance += Random.Range(minBuffRate, maxBuffRate);
        deathChance += Random.Range(minBuffRate, maxBuffRate);
        infectionRate += Random.Range(minBuffRate, maxBuffRate);
        deathRate += Random.Range(minBuffRate, maxBuffRate);
    }

    public static void IncreateBuffRate()
    {
        minBuffRate += baseBuffRateIncrease;
        maxBuffRate += baseBuffRateIncrease;
    }
    


    //void Awake()
    //{

    //}

    //// Start is called before the first frame update
    //void Start()
    //{
    //    infectionRate = 0.20f;
    //    deathChance = 0.10f;
    //    spreadOut = 0.20f;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    // Every 5 minutes before 10 minutes, increase infectionRate by 5-10%.
    //    // in Game class, every x rounds, call VirusScript.InfectionBuff

    //    // Every 5 minutes before 20 minutes, increase infectionRate by 10-15%.
    //    // in Game class, after x rounds, every x rounds, call VirusScript.InfectionBuff

    //    // After 20 minutes, end game or make it almost unplayable
    //}

}

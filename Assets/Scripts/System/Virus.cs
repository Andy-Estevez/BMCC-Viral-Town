using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Virus
{
    // chance is how likely it is to happen
    // rate is how many people it impacts
    public static float infectionChance = 0.1f;
    public static float deathChance = 0.05f;
    public static float infectionRate = 0.10f;
    public static float deathRate = 0.10f;

    public static float minBuffRate = 0.02f;
    public static float maxBuffRate = 0.04f;

    public static float baseBuffRateIncrease = 0.03f;


    public static void InfectionBuff()
    {
        float randFloat = Random.Range(minBuffRate, maxBuffRate);
        float randFloatRounded = Mathf.Round(randFloat * 100) / 100;

        if (infectionChance + randFloatRounded >= 1.0f)
        {
            infectionChance = 1.0f;
        }
        else
        {
            infectionChance += randFloatRounded;
        }

        if (deathChance + randFloatRounded >= 1.0f)
        {
            deathChance = 1.0f;
        }
        else
        {
            deathChance += randFloatRounded;
        }

        if (infectionRate + randFloatRounded >= 1.0f)
        {
            infectionRate = 1.0f;
        }
        else
        {
            infectionRate += randFloatRounded;
        }

        if (deathRate + randFloatRounded >= 1.0f)
        {
            deathRate = 1.0f;
        }
        else
        {
            deathRate += randFloatRounded;
        }
    }

    public static void IncreateBuffRate()
    {
        if (minBuffRate + baseBuffRateIncrease >= 1.0f)
        {
            minBuffRate = 1.0f;
        }
        else
        {
            minBuffRate += baseBuffRateIncrease;
        }

        if (maxBuffRate + baseBuffRateIncrease >= 1.0f)
        {
            maxBuffRate = 1.0f;
        }
        else
        {
            maxBuffRate += baseBuffRateIncrease;
        }
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

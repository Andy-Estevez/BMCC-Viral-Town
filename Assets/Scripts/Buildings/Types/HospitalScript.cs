using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalScript : BuildingTemplate
{

    public float cureChance = 0.25f;

    public void VirusCure()
    {
        if (Random.value <= cureChance && infectedOccupants != 0)
        {
            int randCuredPeople = Random.Range(1, infectedOccupants);
            healthyOccupants = healthyOccupants + randCuredPeople;
            infectedOccupants = infectedOccupants - randCuredPeople;
            //Town.moveDischargedToRes(randCuredPeople);
            // delete after test
            Debug.Log($"{gameObject.name}: {randCuredPeople} of {origInfectedOccupants} sick occupants cured");
        }
    }

    public override void Awake()
    {
        base.Awake();
        ViralTownEvents.PropagateHealing.AddListener(VirusCure);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

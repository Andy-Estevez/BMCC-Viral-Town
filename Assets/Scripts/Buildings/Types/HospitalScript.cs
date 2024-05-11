using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalScript : BuildingTemplate
{

    public float cureChance = 0.50f;

    public void VirusCure()
    {
        if (Random.value <= cureChance)
        {
            int randCuredPeople = Random.Range(1, infectedOccupants);
            healthyOccupants = healthyOccupants + randCuredPeople;
            infectedOccupants = infectedOccupants - randCuredPeople;
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

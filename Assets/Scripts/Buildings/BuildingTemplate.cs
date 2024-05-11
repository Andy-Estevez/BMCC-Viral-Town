using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

// abstract class to contain elements
public abstract class BuildingTemplate : MonoBehaviour
{
    public int capacity;

    public int occupants;
    public int healthyOccupants;
    public int infectedOccupants;

    public int origOccupants;
    public int origHealthyOccupants;
    public int origInfectedOccupants;

    // spreads the virus 
    public virtual void PropagateVirus()
    {
        if (occupants == 0)
        {
            return;
        }
        if (infectedOccupants == occupants)
        {
            return;
        }

        if (Random.value <= Virus.infectionChance)
        {
            // amount of people to infect
            int infectAmount;

            infectAmount = (int)( Mathf.Ceil( occupants * Random.Range(0.01f, Virus.infectionRate) ) );


            if (infectAmount >= healthyOccupants)
            {
                healthyOccupants = 0;
                infectedOccupants = occupants;
                Debug.Log($"SPECIAL | {gameObject.name}: {infectedOccupants} sick / {healthyOccupants} healthy of {occupants} occupants");
            }
            else
            {
                infectedOccupants = infectedOccupants + infectAmount;
                healthyOccupants = occupants - infectedOccupants;
                // delete after testing
                Debug.Log($"{gameObject.name}: {infectedOccupants} sick / {healthyOccupants} healthy of {occupants} occupants");
            }
        }
    }

    // kills people with 10% or deathChance probability. random.value returns value bewteen 0.0 (inclusive) and 1.0 (inclusive)
    public virtual void PropagateDeath()
    {
        if (infectedOccupants == 0)
        {
            return;
        }
        if (Random.value <= Virus.deathChance)
        {
            int deathAmount = (int)(Mathf.Ceil(infectedOccupants * Random.Range(0.01f, Virus.deathRate)));

            infectedOccupants = infectedOccupants - deathAmount;
            occupants = infectedOccupants + healthyOccupants;
            Debug.Log($"{gameObject.name}: {deathAmount} occupants died. {occupants} / {origOccupants} occupants left");
        }
    }

    public virtual void Awake()
    {
        Debug.Log($"buildingTemplate {gameObject.name} : I'm being called");
        ViralTownEvents.PropagateVirus.AddListener(PropagateVirus);
        ViralTownEvents.PropagateDeath.AddListener(PropagateDeath);
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}


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
            Debug.Log($"Building [{gameObject.name}] is unoccupied. Please remove from list.");
            return;
        }
        if (infectedOccupants == occupants)
        {
            Debug.Log($"Cannot propagate virus; all residents are already infected");
            return;
        }

        if (Random.value <= VirusStats.infectionChance)
        {
            // amount of people to infect
            int infectAmount;

            if (infectedOccupants > 0)
            {
                infectAmount = (int)( Mathf.Ceil(occupants * (Random.Range(0.01f, VirusStats.infectionRate) + (infectedOccupants / occupants)) ));
            }
            else
            {
                infectAmount = (int)(Mathf.Ceil(occupants * Random.Range(0.01f, VirusStats.infectionRate)));
            }

            if (infectAmount + infectedOccupants + healthyOccupants >= occupants)
            {
                Debug.Log($"Succeeded to propagate virus at {VirusStats.infectionChance * 100}% chance | " +
                            $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants to {occupants} infectedOccupants / {0} healthyOccupants");
                healthyOccupants = 0;
                infectedOccupants = occupants;
            }
            else
            {
                Debug.Log($"Succeeded to propagate virus at {VirusStats.infectionChance * 100}% chance | " +
                            $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants to {infectedOccupants + infectAmount} infectedOccupants / {occupants - (infectedOccupants + infectAmount)} healthyOccupants");
                infectedOccupants = infectedOccupants + infectAmount;
                healthyOccupants = occupants - infectedOccupants;
            }
        }
        else
        {
            Debug.Log($"Failed to propagate virus at {VirusStats.infectionRate * 100}% chance| " +
                        $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants of {occupants} occupants");
        }
    }

    // kills people with 10% or deathChance probability. random.value returns value bewteen 0.0 (inclusive) and 1.0 (inclusive)
    public virtual void PropagateDeath()
    {
        if (infectedOccupants == 0)
        {
            Debug.Log($"Death cannot be propagated for {infectedOccupants} infectedOccupants occupants | " +
                        $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants of {occupants} occupants");
            return;
        }
        else if (Random.value <= VirusStats.deathChance)
        {
            int deathAmount = (int)(Mathf.Ceil(infectedOccupants * Random.Range(0.01f, VirusStats.deathRate)));

            infectedOccupants = infectedOccupants - deathAmount;
            occupants = infectedOccupants + healthyOccupants;

            Debug.Log($"Succeeded to cause infection deaths at {VirusStats.deathChance * 100}% chance | " + 
                        $"{deathAmount} out of {infectedOccupants + deathAmount} infectedOccupants occupants died | " +
                        $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants of {occupants} occupants");
        }
        else
        {
            Debug.Log($"Failed to cause infection deaths at {VirusStats.deathChance * 100}% chance | " + 
                        $"{infectedOccupants} infectedOccupants occupants still alive | " +
                        $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants of {occupants} occupants");
        }

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


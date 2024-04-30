using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

// abstract class to contain elements
public abstract class BuildingTemplate : MonoBehaviour
{

    public int occupancy;
    public int capacity;
    public int healthy;
    public int sick;

    // spreads the virus 
    public virtual void PropagateVirus()
    {
        if (occupancy == 0)
        {
            Debug.Log($"Building [{gameObject.name}] is unoccupied. Please remove from list.");
            return;
        }
        if (sick == occupancy)
        {
            Debug.Log($"Cannot propagate virus when all residents are already infected");
            return;
        }

        if (Random.value <= VirusScript.infectionChance)
        {
            // amount of people to infect
            int infectAmount;

            if (sick > 0)
            {
                infectAmount = (int)( Mathf.Ceil(occupancy * (Random.Range(0.01f, VirusScript.infectionRate) + (sick / occupancy)) ));
            }
            else
            {
                infectAmount = (int)(Mathf.Ceil(occupancy * Random.Range(0.01f, VirusScript.infectionRate)));
            }

            if (infectAmount + sick + healthy >= occupancy)
            {
                Debug.Log($"Succeeded to propagate virus at {VirusScript.infectionChance * 100}% chance | " +
                            $"{sick} sick / {healthy} healthy to {occupancy} sick / {0} healthy");
                healthy = 0;
                sick = occupancy;
            }
            else
            {
                Debug.Log($"Succeeded to propagate virus at {VirusScript.infectionChance * 100}% chance | " +
                            $"{sick} sick / {healthy} healthy to {sick + infectAmount} sick / {occupancy - (sick + infectAmount)} healthy");
                sick = sick + infectAmount;
                healthy = occupancy - sick;
            }
        }
        else
        {
            Debug.Log($"Failed to propagate virus at {VirusScript.infectionRate * 100}% chance| " +
                        $"{sick} sick / {healthy} healthy of {occupancy} occupants");
        }
    }

    // kills people with 10% or deathChance probability. random.value returns value bewteen 0.0 (inclusive) and 1.0 (inclusive)
    public virtual void PropagateDeath()
    {
        if (sick == 0)
        {
            Debug.Log($"Death cannot be propagated for {sick} sick occupants | " +
                        $"{sick} sick / {healthy} healthy of {occupancy} occupants");
            return;
        }
        else if (Random.value <= VirusScript.deathChance)
        {
            int deathAmount = (int)(Mathf.Ceil(sick * Random.Range(0.01f, VirusScript.deathRate)));

            sick = sick - deathAmount;
            occupancy = sick + healthy;

            Debug.Log($"Succeeded to cause infection deaths at {VirusScript.deathChance * 100}% chance | " + 
                        $"{deathAmount} out of {sick + deathAmount} sick occupants died | " +
                        $"{sick} sick / {healthy} healthy of {occupancy} occupants");
        }
        else
        {
            Debug.Log($"Failed to cause infection deaths at {VirusScript.deathChance * 100}% chance | " + 
                        $"{sick} sick occupants still alive | " +
                        $"{sick} sick / {healthy} healthy of {occupancy} occupants");
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


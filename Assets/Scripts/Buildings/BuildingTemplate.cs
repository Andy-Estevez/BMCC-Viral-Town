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

    public int residents;
    public int healthyResidents;
    public int infectedResidents;

    [SerializeField] private GameObject exclamationMarkPrefab; 
    private GameObject exclamationMarkInstance;

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
            Debug.Log($"Cannot propagate virus when all residents are already infected");
            return;
        }

        if (Random.value <= VirusScript.infectionChance)
        {
            // amount of people to infect
            int infectAmount;

            if (infectedOccupants > 0)
            {
                infectAmount = (int)( Mathf.Ceil(occupants * (Random.Range(0.01f, VirusScript.infectionRate) + (infectedOccupants / occupants)) ));
            }
            else
            {
                infectAmount = (int)(Mathf.Ceil(occupants * Random.Range(0.01f, VirusScript.infectionRate)));
            }

            if (infectAmount + infectedOccupants + healthyOccupants >= occupants)
            {
                Debug.Log($"Succeeded to propagate virus at {VirusScript.infectionChance * 100}% chance | " +
                            $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants to {occupants} infectedOccupants / {0} healthyOccupants");
                healthyOccupants = 0;
                infectedOccupants = occupants;
            }
            else
            {
                Debug.Log($"Succeeded to propagate virus at {VirusScript.infectionChance * 100}% chance | " +
                            $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants to {infectedOccupants + infectAmount} infectedOccupants / {occupants - (infectedOccupants + infectAmount)} healthyOccupants");
                infectedOccupants = infectedOccupants + infectAmount;
                healthyOccupants = occupants - infectedOccupants;
            }
        }
        else
        {
            Debug.Log($"Failed to propagate virus at {VirusScript.infectionRate * 100}% chance| " +
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
        else if (Random.value <= VirusScript.deathChance)
        {
            int deathAmount = (int)(Mathf.Ceil(infectedOccupants * Random.Range(0.01f, VirusScript.deathRate)));

            infectedOccupants = infectedOccupants - deathAmount;
            occupants = infectedOccupants + healthyOccupants;

            Debug.Log($"Succeeded to cause infection deaths at {VirusScript.deathChance * 100}% chance | " + 
                        $"{deathAmount} out of {infectedOccupants + deathAmount} infectedOccupants occupants died | " +
                        $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants of {occupants} occupants");
        }
        else
        {
            Debug.Log($"Failed to cause infection deaths at {VirusScript.deathChance * 100}% chance | " + 
                        $"{infectedOccupants} infectedOccupants occupants still alive | " +
                        $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants of {occupants} occupants");
        }

    }

    //pop up warning sign according to the 
    public void UpdateInfectionIndicator()
    {
        // Define the infection rate and threshold for displaying the indicator
        float infectionRate = (float)infectedOccupants / occupants;
        float infectionThreshold1 = 0.3f; // Example threshold, adjust as needed
        //float infectionThreshold2 = 0.3f;
        //float infectionThreshold3 = 0.3f;

        // Check if an exclamation mark needs to be shown or hidden
        if (infectionRate >= infectionThreshold1 && exclamationMarkInstance == null)
        {
            // Instantiate the exclamation mark above the building
            Vector3 offset = new Vector3(0, 1, 0); // Offset above the building
            exclamationMarkInstance = Instantiate(exclamationMarkPrefab, transform.position + offset, Quaternion.identity, transform);
        }
        //else if (infectionRate >= infectionThreshold2 || infectionRate < infectionThreshold3 )
        //{
        //    // Instantiate the exclamation mark above the building
        //    Vector3 offset = new Vector3(1/2, 1, 0); // Offset above the building
        //    Vector3 offset1 = new Vector3(-1/2, 1, 0);
        //    exclamationMarkInstance = Instantiate(exclamationMarkPrefab, transform.position + offset, Quaternion.identity, transform);
        //    exclamationMarkInstance = Instantiate(exclamationMarkPrefab, transform.position + offset1, Quaternion.identity, transform);

        //}
        else if (infectionRate < infectionThreshold1 && exclamationMarkInstance != null)
        {
            // Destroy the exclamation mark if the condition is no longer met
            Destroy(exclamationMarkInstance);
        }
    }


    //// Start is called before the first frame update
    //void Start()
    //{

    //}

}


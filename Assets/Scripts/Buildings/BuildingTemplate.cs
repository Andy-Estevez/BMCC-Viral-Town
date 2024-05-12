using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

    [SerializeField] private GameObject exclamationMarkPrefab; 
    private GameObject exclamationMarkInstance;
    [SerializeField] private GameObject infectionRatePopupPrefab;
    private GameObject infectionRatePopupInstance;

    // spreads the virus 
    public virtual void PropagateVirus()
    {
        Debug.Log($"VIRUS PROPAGATED! -{gameObject.name}");
        if (occupants == 0)
        {
            Debug.Log($"Building [{gameObject.name}] is unoccupied. Cannot propagate virus in empty building.");
            return;
        }
        if (infectedOccupants == occupants)
        {
            Debug.Log($"Cannot propagate virus; all residents are already infected");
            return;
        }

        if (Random.value <= Virus.infectionChance)
        {
            // amount of people to infect
            int infectAmount;

            if (infectedOccupants > 0)
            {
                infectAmount = (int)(Mathf.Ceil(occupants * (Random.Range(0.01f, Virus.infectionRate) + (infectedOccupants / occupants)) ));
            }
            else
            {
                infectAmount = (int)(Mathf.Ceil(occupants * Random.Range(0.01f, Virus.infectionRate)));
            }

            if (infectAmount + infectedOccupants + healthyOccupants >= occupants)
            {
                Debug.Log($"Succeeded to propagate virus at {Virus.infectionChance * 100}% chance | " +
                            $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants to {occupants} infectedOccupants / {0} healthyOccupants");
                healthyOccupants = 0;
                infectedOccupants = occupants;
            }
            else
            {
                Debug.Log($"Succeeded to propagate virus at {Virus.infectionChance * 100}% chance | " +
                            $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants to {infectedOccupants + infectAmount} infectedOccupants / {occupants - (infectedOccupants + infectAmount)} healthyOccupants");
                infectedOccupants = infectedOccupants + infectAmount;
                healthyOccupants = occupants - infectedOccupants;
            }
        }
        else
        {
            Debug.Log($"Failed to propagate virus at {Virus.infectionRate * 100}% chance| " +
                        $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants of {occupants} occupants");
        }
    }

    // kills people with 10% or deathChance probability. random.value returns value bewteen 0.0 (inclusive) and 1.0 (inclusive)
    public virtual void PropagateDeath()
    {
        Debug.Log($"DEATH PROPAGATED! -{gameObject.name}");
        if (occupants == 0)
        {
            Debug.Log($"Building [{gameObject.name}] is unoccupied. Cannot propagate death in empty building.");
            return;
        }
        if (infectedOccupants == 0)
        {
            Debug.Log($"Death cannot be propagated for {infectedOccupants} infectedOccupants occupants | " +
                        $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants of {occupants} occupants");
            return;
        }
        if (Random.value <= Virus.deathChance)
        {
            int deathAmount = (int)(Mathf.Ceil(infectedOccupants * Random.Range(0.01f, Virus.deathRate)));

            infectedOccupants = infectedOccupants - deathAmount;
            occupants = infectedOccupants + healthyOccupants;

            Debug.Log($"Succeeded to cause infection deaths at {Virus.deathChance * 100}% chance | " + 
                        $"{deathAmount} out of {infectedOccupants + deathAmount} infectedOccupants occupants died | " +
                        $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants of {occupants} occupants");
        }
        else
        {
            Debug.Log($"Failed to cause infection deaths at {Virus.deathChance * 100}% chance | " + 
                        $"{infectedOccupants} infectedOccupants occupants still alive | " +
                        $"{infectedOccupants} infectedOccupants / {healthyOccupants} healthyOccupants of {occupants} occupants");
        }

    }


    public virtual void Awake()
    {
        Debug.Log($"buildingTemplate {gameObject.name} : I'm being called");
        ViralTownEvents.PropagateVirus.AddListener(PropagateVirus);
        ViralTownEvents.PropagateDeath.AddListener(PropagateDeath);
        ViralTownEvents.UpdateExclamationMark.AddListener(UpdateInfectionIndicator);
        ViralTownEvents.UpdateUINotification.AddListener(UpdateUINotificationIndicator);
    }

    public void UpdateInfectionIndicator()
    {
        // Define the infection rate and threshold for displaying the indicator
        float infectionRate = (float)infectedOccupants / occupants;
        float infectionThreshold1 = 0.3f; // Example threshold, adjust as needed

        // Check if an exclamation mark needs to be shown or hidden
        if (infectionRate >= infectionThreshold1 && exclamationMarkInstance == null)
        {
            // Instantiate the exclamation mark above the building
            Vector3 offset = new Vector3(0, 1, 0); // Offset above the building
            exclamationMarkInstance = Instantiate(exclamationMarkPrefab, transform.position + offset, Quaternion.identity, transform);

        }

        else if (infectionRate < infectionThreshold1 && exclamationMarkInstance != null)
        {
            // Destroy the exclamation mark if the condition is no longer met
            Destroy(exclamationMarkInstance);
            Destroy(infectionRatePopupInstance);
        }
    }



    public void UpdateUINotificationIndicator()
    {
 
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Clicked");
                    infectionRatePopupInstance = Instantiate(infectionRatePopupPrefab, transform.position, Quaternion.identity, transform);
                    UnityEngine.UI.Text textComponent = infectionRatePopupInstance.GetComponentInChildren<UnityEngine.UI.Text>();
                    textComponent.text = $"Infection: {infectedOccupants * 100:F1}%";
                }
            }
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


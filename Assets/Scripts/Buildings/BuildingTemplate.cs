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
                Debug.Log($"{gameObject.name}: {infectedOccupants} sick / {healthyOccupants} healthy of {occupants} occupants");
            }
            else
            {
                healthyOccupants = healthyOccupants - infectAmount;
                infectedOccupants = infectedOccupants + infectAmount;
                // delete after testing
                //Debug.Log($"{gameObject.name}: {infectedOccupants} sick / {healthyOccupants} healthy of {occupants} occupants");
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
            int deathAmount = (int)( Mathf.Ceil( infectedOccupants * Random.Range(0.01f, Virus.deathRate) ) );

            infectedOccupants = infectedOccupants - deathAmount;
            occupants = occupants - deathAmount;
            //Debug.Log($"{gameObject.name}: {deathAmount} occupants died. {occupants} / {origOccupants} occupants left");
        }
    }


    public virtual void Awake()
    {
        Debug.Log($"buildingTemplate {gameObject.name} : I'm being called");
        ViralTownEvents.PropagateVirus.AddListener(PropagateVirus);
        ViralTownEvents.PropagateDeath.AddListener(PropagateDeath);
        ViralTownEvents.UpdateExclamationMark.AddListener(UpdateInfectionIndicator);
        //ViralTownEvents.UpdateUINotification.AddListener(OnMouseDown);
    }

    public void UpdateInfectionIndicator()
    {
        // Define the infection rate and threshold for displaying the indicator
        float infectionRate = (float)infectedOccupants / occupants;
        float infectionThreshold1 = 0.5f; // Example threshold, adjust as needed

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



    
    void OnMouseDown()
    {
        Debug.Log("clicked");
        float infectionRate = (float)infectedOccupants / occupants;

        // Position the popup above the building or adjust as necessary
        Vector3 popupPosition = transform.position + new Vector3(0, 2, 0); // Adjusts position slightly above the building

        // Check if a popup already exists and destroy it if it does
        if (infectionRatePopupInstance != null)
        {
            Destroy(infectionRatePopupInstance);
        }

        // Instantiate a new popup
        infectionRatePopupInstance = Instantiate(infectionRatePopupPrefab, popupPosition, Quaternion.identity);
        UnityEngine.UI.Text textComponent = infectionRatePopupInstance.GetComponentInChildren<UnityEngine.UI.Text>();
        textComponent.text = $"Infection: {infectedOccupants * 100 / occupants:F1}%";

        // Destroy the popup after 2 seconds
        Destroy(infectionRatePopupInstance, 2f);  // 2f is the time in seconds after which the popup will be destroyed
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



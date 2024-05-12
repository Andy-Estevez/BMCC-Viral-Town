using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    //[SerializeField]
    public Text economyHealthText;
    public Text infectionRateText;
    //public Text deathRateText;
    public Text populationText;
    public Text healtyPopText;

    // Update is called once per frame
    void UpdateHUDIndicator()
    {
        populationText.text = "Total Population: " + Town.CurrentPop.ToString();
        healtyPopText.text = "Healthy Population: " + Town.HealthyPop.ToString();
        infectionRateText.text = "Infected Population: " + Town.InfectedPop.ToString();
        economyHealthText.text = "GDP: $" + Town.CurrentGDP.ToString();

    }

    private void Awake()
    {
        ViralTownEvents.UpdateHUD.AddListener(UpdateHUDIndicator);
    }
}
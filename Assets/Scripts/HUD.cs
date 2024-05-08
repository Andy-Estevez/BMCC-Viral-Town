using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    TownStatistics ts = new TownStatistics();

    //[SerializeField]
    public Text economyHealthText;
    public Text infectionRateText;
    //public Text deathRateText;
    public Text populationText;
    public Text healtyPopText;

    // Start is called before the first frame update
    void Start()
    {
        economyHealthText.text = "GDP: $" + ts.gdp.ToString();
        populationText.text = "Total Population: " + ts.totalPop.ToString();
        healtyPopText.text = "Healthy Population: " + ts.healthyPop.ToString();
        infectionRateText.text = "Infected Population: " + ts.infectedPop.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        populationText.text = "Total Population: " + Town.TotalPop.ToString();
        healtyPopText.text = "Healthy Population: " + Town.HealthyPop.ToString();
        infectionRateText.text = "Infected Population: " + Town.InfectedPop.ToString();
        economyHealthText.text = "GDP: $" + Town.GDP.ToString();

    }
}

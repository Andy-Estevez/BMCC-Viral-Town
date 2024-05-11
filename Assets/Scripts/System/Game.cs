using UnityEditor;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Variables
    [SerializeField] private int initialPopulation;
    [SerializeField] private int initialHealthy;
    [SerializeField] private int initialInfected;
    [SerializeField] private int initialGDP;

    [SerializeField] private int roundLengthSec;
    private string roundSection = "night";
    private int roundNum = 0;

    private bool gameOver = false;

    private Timer timer;

    // Methods

    // Start is called before the first frame update
    void Start()
    {
        // Initialize town class data
        Town.init(initialPopulation, initialHealthy, initialInfected, initialGDP);

        // Load map
        Town.generateTown();

        timer = new Timer();
        timer.setTimer(roundLengthSec / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            // If section (night/day) is over
            if (timer.IsFinished)
            {
                // DAWN: When night ends & day begins
                if (roundSection == "night")
                {
                    Debug.Log("Night has ended...");

                    // Terminate Occurrence Pop-ups
                    ViralTownEvents.TerminateOccurrencePopups.Invoke();

                    // Propagate Virus Infection & Virus Death
                    ViralTownEvents.PropagateVirus.Invoke();
                    ViralTownEvents.PropagateDeath.Invoke();

                    // Update population
                    Town.updatePop();

                    // Update HUD
                    ViralTownEvents.UpdateHUD.Invoke();

                    // Increment round info
                    roundSection = "day";
                    roundNum++;

                    // Update map visuals
                    Town.updateMap("day");

                    // Move population to commercial buildings
                    Town.movePopToCom();

                    // Propagate virus infection (Optional)
                    // ...
                }
                // DUSK: When day ends & night begins
                else if (roundSection == "day")
                {
                    Debug.Log("Day has ended...");

                    // Propagate hospital healing
                    // ...

                    // Increment section
                    roundSection = "night";

                    // Update map visuals
                    Town.updateMap("night");

                    // Move population to residential buildings
                    Town.movePopToRes();

                    // Execute Random Occurrences & Player Policies
                    ViralTownEvents.ActivateOccurrences.Invoke();
                }
                else
                {
                    Debug.LogError("Invalid roundSection value given during round #" + roundNum);
                }

                // Restart section timer
                timer.startTimer();
            }
            else
            {
                // Increment section timer
                timer.updateTimer(Time.deltaTime);
            }
        }
    }
}

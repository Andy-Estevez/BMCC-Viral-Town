using UnityEditor;
using UnityEditor.Experimental.GraphView;
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

    [SerializeField] private MapSprites mapBuildingSprites;
    [SerializeField] private GameObject lake1;
    [SerializeField] private GameObject lake2;

    private Camera camera;

    private Timer timer;

    // Methods

    // Start is called before the first frame update
    void Start()
    {
        // Initialize town class data
        Town.init(initialPopulation, initialHealthy, initialInfected, initialGDP);

        // Load map
        Town.generateTown();

        camera  = Camera.main;

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
                // Mayor observes the infection rate
                ViralTownEvents.UpdateExclamationMark.Invoke();
                ViralTownEvents.UpdateUINotification.Invoke();

                // DAWN: When night ends & day begins
                if (roundSection == "night")
                {
                    Debug.Log("Night has ended...");

                    // Terminate Occurrence Pop-ups
                    ViralTownEvents.TerminateOccurrencePopups.Invoke();

                    // Propagate virus infection & virus death
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
                    Town.updateMap("day", mapBuildingSprites);
                    camera.backgroundColor = new Color32(116, 238, 129, 255);
                    lake1.GetComponent<SpriteRenderer>().color = new Color32(167, 245, 255, 255);
                    lake2.GetComponent<SpriteRenderer>().color = new Color32(167, 245, 255, 255);

                    // Move population to commercial buildings
                    Town.movePopToCom();

                    // Propagate virus infection (Optional)
                    ViralTownEvents.PropagateVirus.Invoke();
                }
                // DUSK: When day ends & night begins
                else if (roundSection == "day")
                {
                    Debug.Log("Day has ended...");

                    // Propagate virus infection
                    ViralTownEvents.PropagateVirus.Invoke();

                    // Propagate hospital healing
                    ViralTownEvents.PropagateHealing.Invoke();

                    // Update population
                    Town.updatePop();

                    // Update HUD
                    ViralTownEvents.UpdateHUD.Invoke();

                    // Increment section
                    roundSection = "night";

                    // Update map visuals
                    Town.updateMap("night", mapBuildingSprites);
                    camera.backgroundColor = new Color32(10, 45, 40, 255);
                    lake1.GetComponent<SpriteRenderer>().color = new Color32(10, 50, 60, 255);
                    lake2.GetComponent<SpriteRenderer>().color = new Color32(10, 50, 60, 255);

                    // Move population to residential buildings
                    Town.movePopToRes();
                    
                    // Execute Random Occurrences & Player Policies
                    ViralTownEvents.MayorOccurrences.Invoke();
                }
                else
                {
                    Debug.LogError("Invalid roundSection value given during round #" + roundNum);
                }

                if (roundNum % 3 == 0)
                {
                    Virus.InfectionBuff();
                }

                if (Town.HealthyPop == 0)
                {
                    gameOver = true;
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
        else
        {
            Debug.Log("GAME OVER!");
        }
    }
}

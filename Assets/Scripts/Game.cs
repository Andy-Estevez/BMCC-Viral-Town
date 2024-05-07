using UnityEditor;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Variables
    [SerializeField] private float initialOccupancyRate;
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
        Town.init(initialOccupancyRate, initialHealthy, initialInfected, initialGDP);
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

                    // Propagate Virus Infection & Virus Death
                    // ...

                    roundSection = "day";
                    roundNum++;

                    // Move population to commercial buildings
                    // Town.movePopToCom();

                    // ...
                }
                // DUSK: When day ends & night begins
                else if (roundSection == "day") 
                {
                    Debug.Log("Day has ended...");

                    // Propagate Hospital Healing
                    // ...

                    roundSection = "night";

                    // Move population to residential buildings
                    // Town.movePopToRes();

                    // Execute Game Events & Player Policies
                    // ...

                    // ...
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

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Variables
    [SerializeField] private int roundLengthSec = 30;
    private string roundSection = "day";
    private int roundNum = 1;

    private Timer timer;

    // Methods

    // Start is called before the first frame update
    void Start()
    {
        Town.generateTown();

        timer = new Timer();
        timer.setTimer(roundLengthSec / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.IsFinished)
        {
            Debug.Log("timer is finished");
            // Day Section Logic
            if (roundSection == "day") 
            {
                roundSection = "night";

                // Do Night Section Things
            }
            // Night Section Logic
            else if (roundSection == "night") 
            {
                roundSection = "day";
                roundNum++;

                // Do Day Section Things
            }
            else 
            {
                Debug.LogError("Invalid roundSection value given during round #" + roundNum);
            }

            timer.startTimer();
        }
        else
        {
            // Increment Timer
            timer.updateTimer(Time.deltaTime);
        }
    }
}

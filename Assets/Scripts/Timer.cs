using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    // Variables
    private float timeGoal = 0;
    private float timeElapsed = 0;
    private bool isRunning = false;
    private bool isFinished = true;

    // Properties
    public bool IsRunning { get { return isRunning; } }
    public bool IsFinished { get { return isFinished; } }

    // Methods

    public void setTimer(float goal)
    {
        timeGoal = goal;
    }

    public void startTimer()
    {
        timeElapsed = 0;

        isRunning = true;
        isFinished = false;
    }

    public void updateTimer(float dTime)
    {

        if (isRunning)
        {
            timeElapsed += dTime;

            if (timeIsUp())
            {
                isRunning = false;
                isFinished = true;
            }
        }
    }

    public void pauseTimer()
    {
        isRunning = false;
    }

    public void unpauseTimer()
    {
        if (!isFinished)
        {
            isRunning = true;
        }
    }

    public bool timeIsUp()
    {
        return (timeElapsed >= timeGoal);
    }
}

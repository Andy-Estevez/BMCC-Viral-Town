using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OccurrencePrefab : MonoBehaviour
{
    [SerializeField]
    private TMP_Text mainTitle;
    [SerializeField]
    private TMP_Text mainDesc;

    [SerializeField]
    private TMP_Text sideTitle;
    [SerializeField]
    private TMP_Text sideDesc;
    [SerializeField]
    private TMP_Text moodleHolder;
    private TMP_Text leftSide;
    private TMP_Text rightSide;

    private void Start()
    {
        // Access the child TMP_Text components
        leftSide = moodleHolder.transform.GetChild(0).GetComponent<TMP_Text>();
        rightSide = moodleHolder.transform.GetChild(1).GetComponent<TMP_Text>();
    }

    public void SetMainTitle(string text)
    {
        mainTitle.text = text;
    }
    public void SetMainDesc(string text)
    {
        mainDesc.text = text;
    }


    public void SetSideDesc(string text)
    {
        sideDesc.text = text;
    }
    public void SetMoodles(OccurrenceMoodles[] moodles)
    {
        foreach (OccurrenceMoodles moodle in moodles)
        {
            // Gets font size of the text
            float arrowSize = leftSide.fontSize;
            // Icrement
            float arrowIncrement = 30f;
            // Calculate the new font size for the arrow
            float newArrowSize = arrowSize + arrowIncrement;

            // If the trait is negative, go left
            if (moodle.Trait == "Negative")
            {
                leftSide.text += moodle.TemporaryValue != null ?
                    moodle.MoodleName + " " + "<size=" + newArrowSize.ToString() + ">-</size>" + moodle.TemporaryValue + "\n" :
                    moodle.MoodleName + " " + "<size=" + newArrowSize.ToString() + ">↓</size>" + moodle.Intensity + "%" + "\n";
            }
            else // Else, go right
            {
                rightSide.text += moodle.TemporaryValue != null ?
                    moodle.MoodleName + " " + "<size=" + newArrowSize.ToString() + ">+</size>" + moodle.TemporaryValue + "\n" :
                    moodle.MoodleName + " " + "<size=" + newArrowSize.ToString() + ">↑</size>" + moodle.Intensity + "%" + "\n";
            }
        }
    }

    public void onClick_closeButton() 
    {
        ViralTownEvents.TerminateOccurrencePopups.Invoke();
    }

    public void onClick_acceptButton()
    {
        ViralTownEvents.SentOccurrenceReq.Invoke();
        ViralTownEvents.TerminateOccurrencePopups.Invoke();
    }
}

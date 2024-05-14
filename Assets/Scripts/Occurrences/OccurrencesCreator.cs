using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEditor.PackageManager;
using UnityEngine;

// Addresses the Moodles of the occurrences
[System.Serializable]
public class OccurrenceMoodles
{
    public string Trait;
    public string MoodleName;
    public bool Switch;
    public float Intensity;
    public float? TemporaryValue;
}
// Addresses the details of the occurrences
[System.Serializable]
public class OccurrenceInfo
{
    public float IntensityValue;
    public string EventTitle;
    public string Description;
    public string SideDescription;
    public OccurrenceMoodles[] Moodles;
}
// Addresses the occurences
[System.Serializable]
public class OccurrencesList
{
    // This will hold the information of the details
    public OccurrenceInfo[] Occurrences;
}

public class OccurrencesCreator : MonoBehaviour
{
    // Stores the occurrence prefab
    OccurrencePrefab newPreFab;
    // Stores the path of the mayor prefab
    string MayorPrefab = "Assets/Prefabs/MayorOccurrence.prefab";
    // Stores the path of the random occurrence prefab
    string RandomPrefab = "Assets/Prefabs/RandomOccurrence.prefab";
    // Choose certain occurence
    OccurrenceInfo chosenOccurence;
    // References the unity events
    public virtual void Awake()
    {
        ViralTownEvents.MayorOccurrences.AddListener(MayorOccurrences);
        ViralTownEvents.RandomOccurrences.AddListener(RandomOccurrences);
        ViralTownEvents.TerminateOccurrencePopups.AddListener(DestroyOccurrence);
        ViralTownEvents.SentOccurrenceReq.AddListener(SentOccurrenceReq);
    }

    // Policies/Mayor
    public void MayorOccurrences()
    {
        // Have to discuss with members but for now, lets say intensity level 1 ("Covid broke out")
        int intensity = 1;

        // Stores the path of the JSON file holding the occurences
        string jsonFilePath = "Scripts/Occurrences/MayorOccurrences.json";
        // Construct the full file path
        string fullPath = Path.Combine(Application.dataPath, jsonFilePath);

        // Check if the file exists
        if (File.Exists(fullPath))
        {
            // Read the JSON file
            string jsonText = File.ReadAllText(fullPath);
            // Deserialize JSON into a list of OccurrenceHolder objects
            OccurrencesList occurenceList = JsonUtility.FromJson<OccurrencesList>(jsonText);
            // Match an occurence based on the intensity
            chosenOccurence = SelectOccurrenceByClosestIntensity(occurenceList, intensity);
            CheckForSwitch();
            StartCoroutine(SummonEvent(chosenOccurence, MayorPrefab));
        }
        else
        {
            Debug.LogError("JSON file not found: " + fullPath);
        }
    }
    // Random Occurrences
    public void RandomOccurrences()
    {
        // Stores the path of the JSON file holding the occurences
        string jsonFilePath = "Scripts/Occurrences/RandomOccurrences.json";
        // Construct the full file path
        string fullPath = Path.Combine(Application.dataPath, jsonFilePath);

        // Check if the file exists
        if (File.Exists(fullPath))
        {
            // Read the JSON file
            string jsonText = File.ReadAllText(fullPath);
            // Deserialize JSON into a list of OccurrenceHolder objects
            OccurrencesList occurenceList = JsonUtility.FromJson<OccurrencesList>(jsonText);
            // Chooses a random occurence (based on probablity)
            chosenOccurence = SelectRandomOccurrence(occurenceList);
            // Checks if a percentage needs to be numerical
            CheckForSwitch();
            StartCoroutine(SummonEvent(chosenOccurence, RandomPrefab));
        }
        else
        {
            Debug.LogError("JSON file not found: " + fullPath);
        }
    }
    // Function to summon the prefab 
    private IEnumerator SummonEvent(OccurrenceInfo occurrence, string link)
    {
        // Load the prefab
        OccurrencePrefab prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<OccurrencePrefab>(link);
        // Check if the prefab is successfully loaded
        if (prefab != null)
        {
            // Instantiate the prefab
            newPreFab = Instantiate(prefab);
            yield return null;

            // Setting up the prefab based on the information
            // Main title
            newPreFab.SetMainTitle(occurrence.EventTitle);
            // Main description
            newPreFab.SetMainDesc(occurrence.Description);
            // Side description
            newPreFab.SetSideDesc(occurrence.SideDescription);
            // Moodles
            newPreFab.SetMoodles(occurrence.Moodles);
        }
        else
        {
            Debug.LogError("Failed to load prefab at path: " + link);
        }
    }

    // Function to randomly select an occurrence based on intensity values (chances)
    public OccurrenceInfo SelectRandomOccurrence(OccurrencesList occurrencesList)
    {
        // Access the Occurrences array from the occurrencesList
        OccurrenceInfo[] occurrences = occurrencesList.Occurrences;

        // Calculate the total intensity sum of all occurrences
        float totalIntensitySum = 0;
        foreach (OccurrenceInfo occurrence in occurrences)
        {
            totalIntensitySum += occurrence.IntensityValue;
        }

        // Generate a random number between 0 and the total intensity sum
        float randomNumber = UnityEngine.Random.Range(0, totalIntensitySum);

        // Iterate through occurrences and determine which one corresponds to the random number
        float cumulativeIntensity = 0;
        foreach (OccurrenceInfo occurrence in occurrences)
        {
            cumulativeIntensity += occurrence.IntensityValue;
            if (randomNumber < cumulativeIntensity)
            {
                // Return the selected occurrence
                return occurrence;
            }
        }

        // This should not be reached under normal circumstances,
        // but return null if no occurrence is selected
        Debug.LogWarning("No occurrence selected. Returning null.");
        return null;
    }
    // Check for any switches
    private void CheckForSwitch()
    {
        float? numericalImpact;
        foreach (OccurrenceMoodles moodle in chosenOccurence.Moodles)
        {
            numericalImpact = null;
            if (moodle.Switch)
            {
                switch (moodle.MoodleName)
                {
                    //Population
                    case "HealthyPop":
                        numericalImpact = Town.HealthyPop * moodle.Intensity;
                        break;
                    case "InfectedPop":
                        numericalImpact = Town.InfectedPop * moodle.Intensity;
                        break;
                    case "Death":
                        break;
                    // Mesc
                    case "GDP":
                        break;
                    default:
                        break;

                }
            }
            moodle.TemporaryValue = numericalImpact;
        }
    }

    // If mayor declined, nothing happens, just exits it out.
    // If mayor accepted or random request
    public void SentOccurrenceReq()
    {
        double impact = 0;
        int newInfectedPop;
        int newHealthyPop;

        foreach (OccurrenceMoodles moodle in chosenOccurence.Moodles)
        {
            switch (moodle.MoodleName)
            {
                //Population
                case "HealthyPop":
                    impact = moodle.Trait == "Positive" ?
                        (moodle.Intensity / 100.0) + 1.0 : 1.0 - (moodle.Intensity / 100.0);

                    // Calculate the new population
                    newHealthyPop = (int)(Town.HealthyPop * impact);
                    newInfectedPop = Town.InfectedPop - (newHealthyPop - Town.HealthyPop);

                    // Ensure the new population does not exceed the initial population
                    if (newHealthyPop > Town.InitialPop)
                    {
                        newInfectedPop -= (newHealthyPop - Town.InitialPop);
                        newHealthyPop = Town.InitialPop;
                    }

                    // Ensure the infected population does not go into negative
                    if (newInfectedPop < 0)
                    {
                        newHealthyPop += newInfectedPop; // Subtract excess from healthy population
                        newInfectedPop = 0;
                    }

                    // Update population values
                    Town.HealthyPop = newHealthyPop;
                    Town.InfectedPop = newInfectedPop;
                    break;

                // Population
                case "InfectedPop":
                    impact = moodle.Trait == "Positive" ?
                        (moodle.Intensity / 100.0) + 1.0 : 1.0 - (moodle.Intensity / 100.0);

                    // Calculate the new population
                    newInfectedPop = (int)(Town.InfectedPop * impact);
                    newHealthyPop = Town.HealthyPop - (newInfectedPop - Town.InfectedPop);

                    // Ensure the new population does not exceed the initial population
                    if (newInfectedPop > Town.InitialPop)
                    {
                        newHealthyPop -= (newInfectedPop - Town.InitialPop);
                        newInfectedPop = Town.InitialPop;
                    }

                    // Ensure the healthy population does not go into negative
                    if (newHealthyPop < 0)
                    {
                        newInfectedPop += newHealthyPop; // Subtract excess from infected population
                        newHealthyPop = 0;
                    }

                    // Update population values
                    Town.InfectedPop = newInfectedPop;
                    Town.HealthyPop = newHealthyPop;
                    break;

                case "Death":
                    break;
                // Mesc
                case "GDP":
                    impact = moodle.Trait == "Positive" ?
                        (moodle.Intensity / 100.0) + 1.0 : 1.0 - (moodle.Intensity / 100.0);
                    Town.CurrentGDP = (int)(Town.CurrentGDP * impact);
                    break;
                default:
                    break;
            }
        }

    }

    // Function to destroy the prefab
    private void DestroyOccurrence()
    {
        if (newPreFab != null)
        {
            // Destroy the GameObject associated with the prefab
            Destroy(newPreFab.gameObject);
        }
    }
    // Function to select an occurence
    private OccurrenceInfo SelectOccurrenceByClosestIntensity(OccurrencesList data, double targetIntensity)
    {
        // Find the occurrence with intensity closest to the target intensity
        return data.Occurrences.OrderBy(occurrence => Math.Abs(occurrence.IntensityValue - targetIntensity)).FirstOrDefault();
    }
}

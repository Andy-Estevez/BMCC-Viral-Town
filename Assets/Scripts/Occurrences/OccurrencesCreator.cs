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
    public string TypeIntensity;
    public int Intensity;
}
// Addresses the details of the occurrences
[System.Serializable]
public class OccurrenceInfo
{
    public int IntensityValue;
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
    // Stores the path of the prefab
    string occurrencePreFab = "Assets/Prefabs/eventCanvas.prefab";
    // Stores the prefab
    OccurrencesPreFab newPreFab;
    // Choose certain occurence
    OccurrenceInfo chosenOccurence;

    // References the unity events
    public virtual void Awake()
    {
        ViralTownEvents.ActivateOccurrences.AddListener(MayorOccurrences);
        ViralTownEvents.TerminateOccurrencePopups.AddListener(DestroyOccurrence);
        ViralTownEvents.AcceptRequest.AddListener(AcceptRequest);
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
            StartCoroutine(SummonEvent(chosenOccurence));
        }
        else
        {
            Debug.LogError("JSON file not found: " + fullPath);
        }
    }
    // Random Occurrences
    public void RandomOccurrences()
    {
        // Have to discuss with members but for now, lets say intensity level 1 ("Covid broke out")
        int intensity = 1;

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
            // Match an occurence based on the intensity
            chosenOccurence = SelectOccurrenceByClosestIntensity(occurenceList, intensity);
            StartCoroutine(SummonEvent(chosenOccurence));
        }
        else
        {
            Debug.LogError("JSON file not found: " + fullPath);
        }
    }

    // Function to select an occurence
    private OccurrenceInfo SelectOccurrenceByClosestIntensity(OccurrencesList data, double targetIntensity)
    {
        // Find the occurrence with intensity closest to the target intensity
        return data.Occurrences.OrderBy(occurrence => Math.Abs(occurrence.IntensityValue - targetIntensity)).FirstOrDefault();
    }
    // Function to summon the prefab 
    public IEnumerator SummonEvent(OccurrenceInfo occurrence)
    {
        // Load the prefab
        OccurrencesPreFab prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<OccurrencesPreFab>(occurrencePreFab);
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
            Debug.LogError("Failed to load prefab at path: " + occurrencePreFab);
        }
    }

    // If declined, nothing happens, just exits it out.
    // If accepted
    public void AcceptRequest()
    {
        // Sets the impact score
        double impact = 0;
        foreach (OccurrenceMoodles moodle in chosenOccurence.Moodles)
        {
            switch (moodle.MoodleName)
            {
                //Population
                case "HealthyPop":
                    if(moodle.TypeIntensity == "Arithmetic")
                    {
                        impact = moodle.Trait == "Positive" ?
                            moodle.Intensity : -moodle.Intensity;
                        Town.HealthyPop += (int)impact;
                        Town.InfectedPop -= (int)impact;
                    }
                    else
                    {
                        impact = moodle.Trait == "Positive" ?
                            (moodle.Intensity / 100) + 100 : 100 - moodle.Intensity;
                        Town.HealthyPop = (int)(Town.HealthyPop * impact);
                        Town.InfectedPop -= (int)(Town.HealthyPop * impact);
                    }

                    break;
                case "InfectedPop":
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

        DestroyOccurrence();
    }

    // Function to destroy the prefab
    public void DestroyOccurrence()
    {
        if (newPreFab != null)
        {
            // Destroy the GameObject associated with the prefab
            Destroy(newPreFab.gameObject);
        }
    }
}

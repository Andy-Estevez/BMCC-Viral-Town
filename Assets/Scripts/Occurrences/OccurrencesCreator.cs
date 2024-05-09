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
    public string MoodleName;
    public int Intensity;
}
// Addresses the details of the occurrences
[System.Serializable]
public class OccurrenceInfo
{
    public int IntensityValue;
    public string EventTitle;
    public string Description;
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
    string occurencePreFab = "Assets/Prefabs/eventCanvas.prefab";

    public void MayorOccurences()
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
            OccurrenceInfo chosenOccurence = SelectOccurrenceByClosestIntensity(occurenceList, intensity);
            SummonEvent(chosenOccurence);
        }
        else
        {
            Debug.LogError("JSON file not found: " + fullPath);
        }
    }
    public void RandomOccurences()
    {

    }
    // Function to select an occurence
    private OccurrenceInfo SelectOccurrenceByClosestIntensity(OccurrencesList data, double targetIntensity)
    {
        // Find the occurrence with intensity closest to the target intensity
        return data.Occurrences.OrderBy(occurrence => Math.Abs(occurrence.IntensityValue - targetIntensity)).FirstOrDefault();
    }
    // Function to summon the prefab 
    public void SummonEvent(OccurrenceInfo occurrence)
    {
        // Load the prefab
        OccurrencesPreFab prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<OccurrencesPreFab>(occurencePreFab);
        // Check if the prefab is successfully loaded
        if (prefab != null)
        {
            // Instantiate the prefab
            OccurrencesPreFab newPreFab = Instantiate(prefab);

            // Call a method or set properties of the script component
        }
        else
        {
            Debug.LogError("Failed to load prefab at path: " + occurencePreFab);
        }
    }

}

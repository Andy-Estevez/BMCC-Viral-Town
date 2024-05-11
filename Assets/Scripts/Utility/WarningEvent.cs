using UnityEngine;
using UnityEngine.Events;

public class WarningEvents : MonoBehaviour
{
    // UnityEvent for infection rate changes. The float parameter can be used to pass the infection rate.
    
    GameObject exclemationMark;

    public void Awake()
    {
        //ViralTownEvents.warning.AddListener(); 
    }

    public void OnUpdateUI(float infectionRate, GameObject exclamationMarkPrefab)
    {
        
        if (infectionRate > 0.5f)
        {
            // Show some serious alert visuals
            Debug.Log("Show serious infection alerts!");
            Vector3 offset = new Vector3(0, 1, 0); // Offset above the building
            exclemationMark = Instantiate(exclamationMarkPrefab, transform.position + offset, Quaternion.identity, transform);
        }
        //else if (infectionRate > 0.2f)
        //{
        //    // Show moderate alert visuals
        //    Debug.Log("Show moderate infection alerts!");
        //    if(exclemationMark != null)
        //    {
        //        Destroy(exclemationMark);
        //    }
        //}
        else
        {
            // Clear or hide infection alerts
            Debug.Log("Infection rate low, hide alerts.");
            Destroy(exclemationMark);
        }
    }
}


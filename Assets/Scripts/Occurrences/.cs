using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class eventsMaker : MonoBehaviour
{
    [SerializeField]
    private Canvas canvasPrefab;
    private Canvas instantiatedCanvas;
    private events eventScriptComponent;


    public void summonEvent()
    {
        // Instantiate the canvas prefab
        instantiatedCanvas = Instantiate(canvasPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        eventScriptComponent = instantiatedCanvas.GetComponentInChildren<events>();
    }
}

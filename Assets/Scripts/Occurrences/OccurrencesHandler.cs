using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

public class OccurrencesHandler : MonoBehaviour
{
    public UnityEvent invoker;

    private void OnMouseDown()
    {
        invoker.Invoke();
    }
}

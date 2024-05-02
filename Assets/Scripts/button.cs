using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class button : MonoBehaviour
{
    public UnityEvent onClick;
    public void OnMouseDown()
    {
        Debug.Log("On gang");
        onClick.Invoke();
    }
}

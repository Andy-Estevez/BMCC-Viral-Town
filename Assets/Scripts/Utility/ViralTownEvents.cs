using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class ViralTownEvents
{
    public static UnityEvent dayTime = new UnityEvent();
    public static UnityEvent nightTime = new UnityEvent();

}

//public class UEvent : UnityEvent<int> { }



// Advice for using UnityEvents:
//
// type:
//      ViralTownEvents.dayTime.Invoke();
// where daytime is activated.
// 
// type: 
//      ViralTownEvents.dayTime.AddListener([InsertFunction]);
// in the Awake() for all classes that need to change at daytime
// and insert the only the function name in the parenthesis
// i.e. ViralTownEvents.dayTime.AddListener(Propagate);
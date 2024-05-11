using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class ViralTownEvents
{
    public static UnityEvent SpawnTown = new UnityEvent();
    public static UnityEvent PropagateVirus = new UnityEvent();
    public static UnityEvent PropagateDeath = new UnityEvent();

    public static UnityEvent UpdateHUD = new UnityEvent();

    public static UnityEvent ActivateOccurrences = new UnityEvent();
    public static UnityEvent TerminateOccurrencePopups = new UnityEvent();

    // ex1:
    // public static IndicatorEvent stageOne = new IndicatorEvent();

    // ex2:
    // public static PolicyEvent enactPolicy = new PolicyEvent();

}


// example1 (passing one parameter):
// public class IndicatorEvent : UnityEvent<int> { }
// 
// invoke looks like: 
// ViralTownEvents.stageOne.Invoke();
//
// listener looks like:
// ViralTownEvents.stageOne.AddListener([insertFunctionName]);


// example2 (passing multiple parameters):
// public class PolicyEvent : UnityEvent<float, int, float> { }
// 
// invoke looks like:
// ViralTownEvents.enactPolicy.Invoke();
//
// listener looks like:
// ViralTownEvents.enactPolicy.AddListener([insertFunctionName]);


// example3 (passing multple object parameters): 
// public class 




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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    Encounter encounter;

    public Encounter SetEncounter<T>() where T : Encounter
    {
        encounter = (T) Activator.CreateInstance(typeof(T));
        encounter.Node = this;
        return encounter;
    }

    public Encounter SetEncounter(Encounter newEncounter)
    {
        encounter = newEncounter;
        encounter.Node = this;
        return encounter;
    }
}

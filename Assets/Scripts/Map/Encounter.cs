using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    public EncounterStateEnum EncounterState;
    public List<Encounter> NextEncounters;
    public List<Encounter> PreviousEncounters;
    // Start is called before the first frame update
    Encounter()
    {
        EncounterState = EncounterStateEnum.Incompleted;
        NextEncounters = new List<Encounter>();
        PreviousEncounters = new List<Encounter>();
    }
    
}

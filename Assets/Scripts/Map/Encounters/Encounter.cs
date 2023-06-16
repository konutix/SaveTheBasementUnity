using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter 
{
    public EncounterStateEnum EncounterState;
    public List<Encounter> NextEncounters;
    public List<Encounter> PreviousEncounters;
    public MapNode Node;
    // Start is called before the first frame update
    public Encounter()
    {
        EncounterState = EncounterStateEnum.Incompleted;
        NextEncounters = new List<Encounter>();
        PreviousEncounters = new List<Encounter>();
    }

    public virtual void LaunchEncounter()
    {

    }
    
}

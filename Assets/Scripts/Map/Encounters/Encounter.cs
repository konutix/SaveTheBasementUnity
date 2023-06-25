using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter 
{
    public EncounterStateEnum encounterState;
    public List<Encounter> nextEncounters;
    public List<Encounter> previousEncounters;
    public MapNode node;
    public int vampireFangsReward;
    // Start is called before the first frame update
    public Encounter()
    {
        encounterState = EncounterStateEnum.Incompleted;
        nextEncounters = new List<Encounter>();
        previousEncounters = new List<Encounter>();
    }

    public virtual void LaunchEncounter()
    {

    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEncounter : Encounter
{
    public StartEncounter()
    {
        encounterState = EncounterStateEnum.Completed;
    }
}

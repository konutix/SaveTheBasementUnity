using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    Encounter Encounter;
    public Map map;
    SpriteRenderer SRenderer;
    bool IsMouseOver;
    [SerializeField] Color NormalColor;
    [SerializeField] Color HighlightColor;
    [Space]
    [SerializeField] SpriteRenderer CompletedOutlineRenderer;
    [SerializeField] SpriteRenderer ReadyOutlineRenderer;

    private void Start()
    {
        SRenderer = GetComponent<SpriteRenderer>();
        SRenderer.color = NormalColor;
        IsMouseOver = false;
        switch(Encounter.encounterState)
        {
            case EncounterStateEnum.Completed:
                CompletedOutlineRenderer.enabled = true;
                ReadyOutlineRenderer.enabled = false;
                break;
            case EncounterStateEnum.Failed:
                CompletedOutlineRenderer.enabled = true;
                ReadyOutlineRenderer.enabled = false;
                break;
            case EncounterStateEnum.Ready:
                CompletedOutlineRenderer.enabled = false;
                ReadyOutlineRenderer.enabled = true;
                break;
            case EncounterStateEnum.Incompleted:
                CompletedOutlineRenderer.enabled = false;
                ReadyOutlineRenderer.enabled = false;
                break;

        }
    }

    public Encounter SetEncounter<T>() where T : Encounter
    {
        Encounter = (T) Activator.CreateInstance(typeof(T));
        Encounter.node = this;
        return Encounter;
    }

    public Encounter SetEncounter(Encounter newEncounter, Map map)
    {
        Encounter = newEncounter;
        Encounter.node = this;
        this.map = map;
        return Encounter;
    }

    private void OnMouseEnter()
    {
        if(Encounter.encounterState == EncounterStateEnum.Ready)
        {
            SRenderer.color = HighlightColor;
            IsMouseOver = true;
        }
    }

    private void OnMouseExit()
    {
        if (Encounter.encounterState == EncounterStateEnum.Ready)
        {
            SRenderer.color = NormalColor;
            IsMouseOver = false;
        }
    }
    private void OnMouseDown()
    {
        if(IsMouseOver)
        {
            map.GetComponent<Map>().CheckCurrentEncounter(Encounter);
            Encounter.LaunchEncounter();
        }
    }

}

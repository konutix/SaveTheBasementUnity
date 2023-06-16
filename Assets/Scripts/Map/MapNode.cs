using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    Encounter Encounter;
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
        switch(Encounter.EncounterState)
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
        Encounter.Node = this;
        return Encounter;
    }

    public Encounter SetEncounter(Encounter newEncounter)
    {
        Encounter = newEncounter;
        Encounter.Node = this;
        return Encounter;
    }

    private void OnMouseEnter()
    {
        if(Encounter.EncounterState == EncounterStateEnum.Ready)
        {
            SRenderer.color = HighlightColor;
            IsMouseOver = true;
        }
    }

    private void OnMouseExit()
    {
        if (Encounter.EncounterState == EncounterStateEnum.Ready)
        {
            SRenderer.color = NormalColor;
            IsMouseOver = false;
        }
    }
    private void OnMouseDown()
    {
        if(IsMouseOver)
        {
            transform.parent.GetComponent<Map>().CheckCurrentEncounter(Encounter);
            Encounter.LaunchEncounter();
        }
    }

}

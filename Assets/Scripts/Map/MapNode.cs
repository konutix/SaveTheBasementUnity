using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField] GameObject SummaryObject;
    [SerializeField] TextMeshProUGUI SummaryText;
    [SerializeField] TextMeshProUGUI RewardText;
    [SerializeField] string Summary;
    [SerializeField] string Reward;

    private void Start()
    {
        SRenderer = GetComponent<SpriteRenderer>();
        SRenderer.color = NormalColor;
        IsMouseOver = false;
        switch(Encounter.encounterState)
        {
            case EncounterStateEnum.Completed:
                CompletedOutlineRenderer.enabled = true;
                ReadyOutlineRenderer.gameObject.SetActive(false);
                break;
            case EncounterStateEnum.Failed:
                CompletedOutlineRenderer.enabled = true;
                ReadyOutlineRenderer.gameObject.SetActive(false);
                break;
            case EncounterStateEnum.Ready:
                CompletedOutlineRenderer.enabled = false;
                ReadyOutlineRenderer.gameObject.SetActive(true);
                break;
            case EncounterStateEnum.Incompleted:
                CompletedOutlineRenderer.enabled = false;
                ReadyOutlineRenderer.gameObject.SetActive(false);
                break;

        };
        SummaryText.text = Summary;
        RewardText.text = TextReplace.Replace(Reward, Encounter.vampireFangsReward);
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
        EncounterStateEnum state = Encounter.encounterState;
        if (state == EncounterStateEnum.Ready)
        {
            SRenderer.color = HighlightColor;
            IsMouseOver = true;
        }
        if(state == EncounterStateEnum.Incompleted || state == EncounterStateEnum.Ready)
        {
            SummaryObject.SetActive(true);
        }

    }

    private void OnMouseExit()
    {
        EncounterStateEnum state = Encounter.encounterState;
        if (state == EncounterStateEnum.Ready)
        {
            SRenderer.color = NormalColor;
            IsMouseOver = false;
        }
        if (state == EncounterStateEnum.Incompleted || state == EncounterStateEnum.Ready)
        {
            SummaryObject.SetActive(false);
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

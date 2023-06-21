using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamDecription : MonoBehaviour, PlaceableDescription
{
    ApplyStatsModifier statsModifier;
    
    PlaceableInfo placeableInfo;

    void Awake()
    {
        placeableInfo = FindObjectOfType<PlaceableInfo>();
        statsModifier = GetComponent<ApplyStatsModifier>();
    }

    public string GetDescription()
    {
        return "Applies " + statsModifier.weakAmount + " weakness to all units it passes. Can move through objects.";
    }

    public void Display(bool display)
    {
        if (display)
        {
            placeableInfo.ShowInfo(GetDescription());
        }
        else
        {
            placeableInfo.HideInfo();
        }
    }
}

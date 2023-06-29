using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterDescription : MonoBehaviour, ObjectDescription
{
    PlaceableInfo placeableInfo;

    void Awake()
    {
        placeableInfo = FindObjectOfType<PlaceableInfo>();
    }

    public string GetDescription()
    {
        return "Teleports player upon hit.";
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
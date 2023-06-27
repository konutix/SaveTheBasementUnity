using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapDescription : MonoBehaviour, ObjectDescription
{
    Projectile projectile;
    Zap zap;
    
    PlaceableInfo placeableInfo;

    void Awake()
    {
        placeableInfo = FindObjectOfType<PlaceableInfo>();
        projectile = GetComponent<Projectile>();
        zap = GetComponent<Zap>();
    }

    public string GetDescription()
    {
        return "Upon impact, triggers all other zaps, dealing " + zap.damage + " damage on the line between them.";
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


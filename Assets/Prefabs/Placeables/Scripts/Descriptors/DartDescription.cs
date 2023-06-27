using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartDescription : MonoBehaviour, ObjectDescription
{
    Projectile projectile;
    ApplyStatsModifier statsModifier;
    
    PlaceableInfo placeableInfo;

    void Awake()
    {
        placeableInfo = FindObjectOfType<PlaceableInfo>();
        projectile = GetComponent<Projectile>();
        statsModifier = GetComponent<ApplyStatsModifier>();
    }

    public string GetDescription()
    {
        int damage = projectile.CalculateDamage();

        return "Deals " + damage + " damage and applies " + statsModifier.vulnerableAmount + " vulnerable.";
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

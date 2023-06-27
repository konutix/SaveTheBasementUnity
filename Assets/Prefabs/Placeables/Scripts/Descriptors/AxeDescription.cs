using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeDescription : MonoBehaviour, ObjectDescription
{
    Projectile projectile;
    Axe axe;
    
    PlaceableInfo placeableInfo;

    void Awake()
    {
        placeableInfo = FindObjectOfType<PlaceableInfo>();
        projectile = GetComponent<Projectile>();
        axe = GetComponent<Axe>();
    }

    public string GetDescription()
    {
        int bladeDamage  = projectile.CalculateDamage(axe.bladeDamage);
        int handleDamage = projectile.CalculateDamage(axe.handleDamage);

        return "Blade deals " + bladeDamage + " damage. Handle deals " + handleDamage + " damage. Strong penetration.";
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

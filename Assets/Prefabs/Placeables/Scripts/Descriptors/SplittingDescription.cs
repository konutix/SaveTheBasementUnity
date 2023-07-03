using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplittingDescription : MonoBehaviour, ObjectDescription
{
    Projectile projectile;

    PlaceableInfo placeableInfo;

    void Awake()
    {
        placeableInfo = FindObjectOfType<PlaceableInfo>();
        projectile = GetComponent<Projectile>();
    }

    public string GetDescription()
    {
        int damage = projectile.CalculateDamage();

        return "Deals " + damage + " damage. On first hit splits into 3 bullets.";
    }

    public void Display(bool display)
    {
        if (display)
        {
            placeableInfo.ShowInfo(GetDescription(), transform.position);
        }
        else
        {
            placeableInfo.HideInfo();
        }
    }
}

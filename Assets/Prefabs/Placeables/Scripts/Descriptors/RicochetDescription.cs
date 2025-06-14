using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetDescription : MonoBehaviour, ObjectDescription
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

        return "Deals " + damage + " damage.";
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

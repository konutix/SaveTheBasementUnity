using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsDescription : MonoBehaviour, ObjectDescription
{
    public bool strength = false;
    public bool weak = false;
    public bool vulnerable = false;
    public bool vampirism = false;

    BattleStats stats;

    PlaceableInfo placeableInfo;

    bool destroyed = false;

    void Awake()
    {
        placeableInfo = FindObjectOfType<PlaceableInfo>();
        stats = GetComponentInParent<BattleStats>();
        stats.onDeathEvent += OnSourceDeath;
    }

    public string GetDescription()
    {
        if (strength)
        {
            return "This unit will deal additional " + stats.strength + " damage.";
        } 
        else if (weak)
        {
            return "This unit will deal " + stats.weakMultiplier + "X of its damage.";
        } 
        else if (vulnerable)
        {
            return "This unit will receive " + stats.vulnerableMultiplier + "X damage.";
        } 
        else if (vampirism)
        {
            return "This unit will heal after it deals damage";
        }

        return "Unknown";
    }

    public void Display(bool display)
    {
        if (destroyed) return;
        
        if (display)
        {
            placeableInfo.ShowInfo(GetDescription());
        }
        else
        {
            placeableInfo.HideInfo();
        }
    }

    void OnSourceDeath()
    {
        Display(false);
        destroyed = true;
    }

    void OnDestroy()
    {
        stats.onDeathEvent -= OnSourceDeath;
    }
}

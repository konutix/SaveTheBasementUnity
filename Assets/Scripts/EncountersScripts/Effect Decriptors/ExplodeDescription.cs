using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeDescription : MonoBehaviour, ObjectDescription
{
    EnemyExplode explosion;

    PlaceableInfo placeableInfo;

    bool destroyed = false;

    void Awake()
    {
        placeableInfo = FindObjectOfType<PlaceableInfo>();
        explosion = GetComponentInParent<EnemyExplode>();
        explosion.GetComponent<BattleStats>().onDeathEvent += OnSourceDeath;
    }

    public string GetDescription()
    {
        return "Explodes after " + explosion.turnsToExplode + " dealing " + explosion.explodeDamage + " damage.";
    }

    public void Display(bool display)
    {
        if (destroyed) return;
        
        if (display)
        {
            placeableInfo.ShowInfo(GetDescription(), transform.position);
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
        explosion.GetComponent<BattleStats>().onDeathEvent -= OnSourceDeath;
    }
}

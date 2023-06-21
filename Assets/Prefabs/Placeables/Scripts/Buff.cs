using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ApplyStatsModifier))]
public class Buff : MonoBehaviour
{
    ApplyStatsModifier modifier;

    void Start()
    {
        modifier = GetComponent<ApplyStatsModifier>();
    }

    public void Init()
    {
        GetComponent<Placeable>().owner.ApplyModifiers(modifier);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDescription : MonoBehaviour, ObjectDescription
{
    PlaceableInfo placeableInfo;

    void Awake()
    {
        placeableInfo = FindObjectOfType<PlaceableInfo>();
    }

    public string GetDescription()
    {
        return "Gives owner vampirism.";
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ShieldDescription : MonoBehaviour, PlaceableDescription
{
    Shield shield;

    PlaceableInfo placeableInfo;

    void Awake()
    {
        placeableInfo = FindObjectOfType<PlaceableInfo>();
        shield = GetComponent<Shield>();
    }

    public string GetDescription()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("{0} incoming projectiles. {1} durability.", 
                        (shield.shouldDestroyProjectile) ? "Destroys" : "Reflects",
                        (shield.evnironmentalHealth < 2) ? "Low" : "High");
        
        return sb.ToString();
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

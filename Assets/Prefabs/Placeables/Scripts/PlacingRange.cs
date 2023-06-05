using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingRange : MonoBehaviour
{
    public bool isInRange = false;

    private void OnMouseEnter() 
    {
        isInRange = true;    
    }

    private void OnMouseExit() 
    {
        isInRange = false;
    }
}

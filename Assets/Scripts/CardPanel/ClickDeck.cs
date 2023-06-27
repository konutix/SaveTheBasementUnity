using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDeck : MonoBehaviour
{
    public delegate void Clicked();
    public Clicked onClicked;
    public bool hovered;
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        onClicked?.Invoke();
    }

    private void OnMouseEnter()
    {
        hovered = true;
    }

    private void OnMouseExit()
    {
        hovered = false;
    }
}

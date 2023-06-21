using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaceableInfo : MonoBehaviour
{
    TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void ShowInfo(string description)
    {
        text.text = description;
        text.transform.parent.position = Input.mousePosition + new Vector3(0, 50, 0);
    }

    public void HideInfo()
    {
        text.transform.parent.position = new Vector3(100000, 100000, 0);
    }
}

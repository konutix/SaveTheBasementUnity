using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VampireFangs : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI amountText;

    private void Start()
    {
        TextLoad();
    }

    public void TextLoad()
    {
        amountText.text = RunState.vampireFangs.ToString();
    }
}

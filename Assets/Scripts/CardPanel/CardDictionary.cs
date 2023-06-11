using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDictionary : MonoBehaviour
{
    public Card[] cardDefs;

    public void Start()
    {
        for(int i = 0; i < cardDefs.Length; i++)
        {
            if (cardDefs[i].id != i)
            {
                Debug.Log("Card " + i + " ID missmatch");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardList : MonoBehaviour
{
    public virtual List<int> GetCards()
    {
        return new List<int>();
    }
}

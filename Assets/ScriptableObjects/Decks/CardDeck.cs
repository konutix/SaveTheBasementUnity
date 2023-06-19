using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardDeck", menuName = "CardDeck")]
public class CardDeck : ScriptableObject
{
    public List<int> deck;
    
}

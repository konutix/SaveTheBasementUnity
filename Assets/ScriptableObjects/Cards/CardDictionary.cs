using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CardDef
{
    public int id;
    public Card card;
}

[CreateAssetMenu(fileName = "NewCardDictionary", menuName = "CardDictionary")]
public class CardDictionary : ScriptableObject
{
    public List<CardDef> cardDefs;

    public Card GetCard(int id)
    {
        return cardDefs.Find(card => card.id == id).card;
    }

}

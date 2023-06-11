using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public int id;
    public string cardName;
    public string description;
    public string type;

    public Sprite art;

    public int manaCost;

}

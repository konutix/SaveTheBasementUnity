using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsFromDeck : CardList
{
    public override List<int> GetCards()
    {
        return RunState.deck;
    }
}

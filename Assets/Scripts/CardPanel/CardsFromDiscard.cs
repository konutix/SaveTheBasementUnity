using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsFromDiscard : CardList
{
    [SerializeField] CardPanelScript cardPanel;
    public override List<int> GetCards()
    {
        return cardPanel.discarded;
    }
}

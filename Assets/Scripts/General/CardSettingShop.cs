using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardSettingShop : CardSetting
{
    [SerializeField] TextMeshProUGUI shopCost;
    public GameObject shopCostPanel;
    public int cardCost;
    public Shop shop;
    public override void SetupCard(Card card)
    {
        base.SetupCard(card);
        cardCost = Random.Range(card.minShopCost, card.maxShopCost + 1);
        print(cardCost);
        shopCost.text = cardCost.ToString();
    }
}

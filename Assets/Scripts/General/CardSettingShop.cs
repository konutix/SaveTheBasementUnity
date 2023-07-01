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
        shopCost.text = cardCost.ToString();
        RunState.shopRewards.Add(new ShopReward(cardID, cardCost));
    }
    public void SetupCard(Card card, int cardCost)
    {
        base.SetupCard(card);
        this.cardCost = cardCost;
        shopCost.text = this.cardCost.ToString();
        RunState.shopRewards.Add(new ShopReward(cardID, cardCost));
    }

    public void SetupExitstingCard(Card card, int cardCost)
    {
        base.SetupCard(card);
        this.cardCost = cardCost;
        shopCost.text = this.cardCost.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardSettingShop : CardSetting
{
    [SerializeField] TextMeshProUGUI shopCost;
    public GameObject shopCostPanel;
    public override void SetupCard(Card card)
    {
        base.SetupCard(card);
        shopCost.text = Random.Range(card.minShopCost, card.maxShopCost + 1).ToString();
    }
}

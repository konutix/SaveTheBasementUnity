using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseShopReward : ChooseCardReward
{
    [SerializeField] GameObject ChoosePanel;
     public Image SavedImage;

    private void OnMouseEnter()
    {
        ChoosePanel.SetActive(true);
    }

    private void OnMouseExit()
    {
        ChoosePanel.SetActive(false);
    }

    protected override void OnClick()
    {
    }

    public override void AddCardToDeck()
    {
        CardSettingShop shopSetting = (CardSettingShop)cardSetting;
        if(shopSetting.cardCost <= RunState.vampireFangs && shopSetting.shop.stopPresentingRewards)
        {
            base.AddCardToDeck();
            RunState.vampireFangs -= shopSetting.cardCost;
            shopSetting.shop.UpdateVampireFangsAmount();
            RunState.shopRewards.Remove(
                RunState.shopRewards.Find(
                    reward => 
                    reward.shopRewardID == shopSetting.cardID && 
                    reward.shopRewardCost == shopSetting.cardCost));
            shopSetting.shop.RemoveCard(gameObject);
        }
    }
    public void SaveCard()
    {
        CardSettingShop shopSetting = (CardSettingShop)cardSetting;
        shopSetting.shop.SaveReward(gameObject);
        ChoosePanel.SetActive(false);
    }
}

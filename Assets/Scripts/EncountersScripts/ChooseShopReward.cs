using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseShopReward : ChooseCardReward
{
    protected override void AddCardToDeck()
    {
        CardSettingShop shopSetting = (CardSettingShop)cardSetting;
        if(shopSetting.cardCost <= RunState.vampireFangs)
        {
            base.AddCardToDeck();
            RunState.vampireFangs -= shopSetting.cardCost;
            shopSetting.shop.UpdateVampireFangsAmount();
        }
    }
}

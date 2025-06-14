using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct SavedReward
{
    public ShopReward shopReward;
    public bool isAssigned;
    public bool isAdditionalReward;
    public int shopCardArrayIndex;

    public SavedReward(int shopRewardID, int shopRewardCost, int shopCardArrayIndex)
    {
        shopReward = new ShopReward(shopRewardID, shopRewardCost);
        isAssigned = true;
        isAdditionalReward = false;
        this.shopCardArrayIndex = shopCardArrayIndex;
    }
}

public struct ShopReward
{
    public int shopRewardID;
    public int shopRewardCost;

    public ShopReward(int shopRewardID, int shopRewardCost)
    {
        this.shopRewardID = shopRewardID;
        this.shopRewardCost = shopRewardCost;
    }
}

public static class RunState
{
    public static float maxPlayerHealth = 70;
    public static float currentPlayerHealth = 70;

    public static int Mana;

    public static List<int> deck;
    public static int cardsDrawn;
    
    public static int vampireFangs = 0;

    public static Encounter[,] currentMap;
    public static Encounter currentEncounter;
    public static Encounter previousEncounter;

    public static List<ShopReward> shopRewards;
    public static SavedReward savedReward;

    public static void ResetData()
    {
        maxPlayerHealth = 70;
        currentPlayerHealth = 70;
        Mana = 4;
        deck = null;
        cardsDrawn = 0;
        vampireFangs = 0;
        currentMap = null;
        currentEncounter = null;
        shopRewards = null;
        savedReward.isAssigned = false;
    }

}

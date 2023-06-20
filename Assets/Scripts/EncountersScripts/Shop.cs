using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] CardDictionary cardDictionary;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject shopPanel;
    [SerializeField] int cardToBuyAmount;
    [SerializeField] VampireFangs vampireFangs;

    List<GameObject> cards;
    public GameObject savedReward;
    public bool stopPresentingRewards;

    private void Awake()
    {
        cards = new List<GameObject>();
        stopPresentingRewards = false;

        SpriteRenderer shopPanelSR = shopPanel.GetComponent<SpriteRenderer>();
        if (RunState.shopRewards == null)
        {
            RunState.shopRewards = new List<ShopReward>();
            int divider = cardToBuyAmount + 1;
            if(RunState.savedReward.isAssigned && RunState.savedReward.isAdditionalReward)
            {
                divider++;
            }
            float shopPanelCardDistance = shopPanelSR.bounds.size.x / divider;
            for (int i = 0; i < cardToBuyAmount; i++)
            {
                Card card = cardDictionary.cardDefs[Random.Range(0, cardDictionary.cardDefs.Count)].card;
                cards.Add(Instantiate(cardPrefab, shopPanel.transform.position + new Vector3(shopPanelSR.bounds.min.x + (i + 1) * shopPanelCardDistance, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f))));
                cards[i].GetComponent<CardSettingShop>().SetupCard(card);
                cards[i].GetComponent<CardSettingShop>().shop = this;
            }
            if (RunState.savedReward.isAssigned && RunState.savedReward.isAdditionalReward)
            {
                Card card = cardDictionary.cardDefs[RunState.savedReward.shopReward.shopRewardID].card;
                cards.Add(Instantiate(cardPrefab, shopPanel.transform.position + new Vector3(shopPanelSR.bounds.min.x + (cardToBuyAmount + 1) * shopPanelCardDistance, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f))));
                cards[cardToBuyAmount].GetComponent<CardSettingShop>().SetupCard(card, RunState.savedReward.shopReward.shopRewardCost);
                cards[cardToBuyAmount].GetComponent<CardSettingShop>().shop = this;
                savedReward = cards[RunState.shopRewards.Count];
            }
        }
        else
        {
            int divider = RunState.shopRewards.Count + 1;
            if (RunState.savedReward.isAssigned && RunState.savedReward.isAdditionalReward)
            {
                divider++;
            }
            float shopPanelCardDistanceLoaded = shopPanelSR.bounds.size.x / divider;
            for (int i = 0; i < RunState.shopRewards.Count; i++)
            {
                Card card = cardDictionary.cardDefs[RunState.shopRewards[i].shopRewardID].card;
                cards.Add(Instantiate(cardPrefab, shopPanel.transform.position + new Vector3(shopPanelSR.bounds.min.x + (i + 1) * shopPanelCardDistanceLoaded, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f))));
                cards[i].GetComponent<CardSettingShop>().SetupCard(card, RunState.shopRewards[i].shopRewardCost);
                cards[i].GetComponent<CardSettingShop>().shop = this;
            }
            if (RunState.savedReward.isAssigned && RunState.savedReward.isAdditionalReward)
            {
                Card card = cardDictionary.cardDefs[RunState.savedReward.shopReward.shopRewardID].card;
                cards.Add(Instantiate(cardPrefab, shopPanel.transform.position + new Vector3(shopPanelSR.bounds.min.x + (RunState.shopRewards.Count + 1) * shopPanelCardDistanceLoaded, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f))));
                cards[RunState.shopRewards.Count].GetComponent<CardSettingShop>().SetupCard(card, RunState.savedReward.shopReward.shopRewardCost);
                cards[RunState.shopRewards.Count].GetComponent<CardSettingShop>().shop = this;
                cards[RunState.shopRewards.Count].GetComponent<ChooseShopReward>().SavedImage.enabled = true;
                savedReward = cards[RunState.shopRewards.Count];
            }
            else if (RunState.savedReward.isAssigned && !RunState.savedReward.isAdditionalReward)
            {
                GameObject savedCard = cards[RunState.savedReward.shopCardArrayIndex];
                if(savedCard != null)
                {
                    savedCard.GetComponent<ChooseShopReward>().SavedImage.enabled = true;
                    savedReward = savedCard;
                }
            }
        }
    }

    private void Update()
    {
        if(!stopPresentingRewards)
        {
            for (int i = 0; i < RunState.shopRewards.Count; i++)
            {
                if ((i > 0 && (cards[i - 1].transform.rotation.eulerAngles.y > 270.0f || cards[i - 1].transform.rotation.eulerAngles.y < 180.0f)) || i == 0)
                {
                    Rotate(cards[i]);
                }
            }
        }
    }

    void Rotate(GameObject go)
    {
        Transform trans = go.transform;
        Vector3 currentAngle = trans.rotation.eulerAngles;
        trans.rotation = Quaternion.Euler(currentAngle.x, currentAngle.y + 180 * Time.deltaTime, currentAngle.z);
        if (trans.rotation.eulerAngles.y < 180.0f)
        {
            trans.rotation = Quaternion.Euler(currentAngle.x, 360.0f, currentAngle.z);

            if (go == cards[RunState.shopRewards.Count-1])
            {
                stopPresentingRewards = true;
            }
            ShowPrice(go);
        }
    }

    void ShowPrice(GameObject card)
    {
        card.GetComponent<CardSettingShop>().shopCostPanel.SetActive(true);
    }

    public void UpdateVampireFangsAmount()
    {
        vampireFangs.TextLoad();
    }

    public void RemoveCard(GameObject card)
    {
        cards.Remove(card);
        if(savedReward == card)
        {
            RunState.savedReward.isAssigned = false;
        }
        else if(savedReward != null)
        {
            RunState.savedReward.shopCardArrayIndex = cards.IndexOf(savedReward);
        }
    }

    public void SaveReward(GameObject card)
    {
        if(savedReward != null)
        {
            savedReward.GetComponent<ChooseShopReward>().SavedImage.enabled = false;
            if(cards.IndexOf(savedReward) == -1)
            {
                cards.Remove(savedReward);
                Destroy(savedReward);
            }
        }
        savedReward = card;
        savedReward.GetComponent<ChooseShopReward>().SavedImage.enabled = true;
        RunState.savedReward = new SavedReward(savedReward.GetComponent<CardSettingShop>().cardID, savedReward.GetComponent<CardSettingShop>().cardCost, cards.IndexOf(card));
    }

    [ContextMenu("Reset Rewards")]
    void ResetVampireFangs()
    {
        RunState.vampireFangs = 10;
        RunState.shopRewards = null;
    }
}

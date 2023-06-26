using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EncounterReward : MonoBehaviour
{
    [SerializeField] CardDictionary cardDictionary;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject rewardPanel;
    [SerializeField] int cardsToChoose;
    [SerializeField] Button vampireButton;
    [SerializeField] Button returnButton;
    [SerializeField] TextMeshProUGUI BannerText;
    List<GameObject> cards;

    // Start is called before the first frame update
    void Start()
    {
        returnButton.interactable = false;
        BannerText.text = "You earned:"+ RunState.currentEncounter.vampireFangsReward +" <sprite index=0> \n Choose Your Reward";

        cards = new List<GameObject>();
        SpriteRenderer rewardPanelSR = rewardPanel.GetComponent<SpriteRenderer>();
        int divider = cardsToChoose + 1;
        float shopPanelCardDistance = rewardPanelSR.bounds.size.x / divider;
        for (int i = 0; i < cardsToChoose; i++)
        {
            Card card = cardDictionary.cardDefs[Random.Range(0, cardDictionary.cardDefs.Count)].card;
            cards.Add(Instantiate(cardPrefab, rewardPanel.transform.position + new Vector3(rewardPanelSR.bounds.min.x + (i + 1) * shopPanelCardDistance, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f))));
            cards[i].GetComponent<CardSetting>().SetupCard(card);
            cards[i].GetComponent<ChooseCardReward>().onRewardSet += ChangeSelection;
        }
    }

    public void GetReward()
    {
        foreach(GameObject card in cards)
        {
            ChooseCardReward reward = card.GetComponent<ChooseCardReward>();
            if (reward.selection.activeSelf)
            {
                reward.AddCardToDeck();
            }
        }

        RunState.shopRewards = null;
        RunState.currentEncounter.encounterState = EncounterStateEnum.Completed;
        RunState.vampireFangs += RunState.currentEncounter.vampireFangsReward;
        RunState.savedReward.isAdditionalReward = true;
        SceneManager.LoadScene("Map");
    }

    public void ChooseMoreFangs()
    {
        foreach (GameObject card in cards)
        {
            Destroy(card);
        }
        cards.Clear();
        RunState.vampireFangs += RunState.currentEncounter.vampireFangsReward;
        ActiveReturnButton();
        Destroy(vampireButton.gameObject);
    }

    void ChangeSelection(GameObject gameObject)
    {
        foreach (GameObject card in cards)
        {
            if(card != gameObject)
            {
                ChooseCardReward reward = card.GetComponent<ChooseCardReward>();
                if (reward.selection.activeSelf)
                {
                    reward.RemoveSelection();
                }
            }
        }
        ActiveReturnButton();
    }

    void ActiveReturnButton()
    {
        returnButton.interactable = true;
    }

}

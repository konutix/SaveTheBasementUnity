using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnouncterReward : MonoBehaviour
{
    [SerializeField] CardDictionary cardDictionary;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject rewardPanel;
    [SerializeField] int cardsToChoose;
    List<GameObject> cards;
    // Start is called before the first frame update
    void Start()
    {
        cards = new List<GameObject>();
        SpriteRenderer rewardPanelSR = rewardPanel.GetComponent<SpriteRenderer>();
        int divider = cardsToChoose + 1;
        float shopPanelCardDistance = rewardPanelSR.bounds.size.x / divider;
        for (int i = 0; i < cardsToChoose; i++)
        {
            Card card = cardDictionary.cardDefs[Random.Range(0, cardDictionary.cardDefs.Count)].card;
            cards.Add(Instantiate(cardPrefab, rewardPanel.transform.position + new Vector3(rewardPanelSR.bounds.min.x + (i + 1) * shopPanelCardDistance, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f))));
            cards[i].GetComponent<CardSetting>().SetupCard(card);
            cards[i].GetComponent<ChooseCardReward>().onRewardGet += RemoveOtherCards;
        }

    }

    void RemoveOtherCards(GameObject gameObject)
    {
        foreach(GameObject card in cards)
        {
            if(card != gameObject)
            {
                Destroy(card);
            }
        }
        cards.Clear();

        RunState.shopRewards = null;
        RunState.currentEncounter.EncounterState = EncounterStateEnum.Completed;
        RunState.vampireFangs += 3;
        RunState.savedReward.isAdditionalReward = true;
    }

}

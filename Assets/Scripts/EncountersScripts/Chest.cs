using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    [SerializeField] CardDictionary cardDictionary;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject rewardPanel;
    [SerializeField] GameObject spriteObject;
    [SerializeField] ParticleSystem particles;
    [SerializeField] int rewardsNumber;
    [SerializeField] Button returnButton;
    bool rewardTaken;
    float timePassed;

    private void Start()
    {
        returnButton.interactable = false;
        rewardTaken = false;
    }

    private void OnMouseDown()
    {
        if(!rewardTaken)
        {
            particles.Play();

            SpriteRenderer rewardPanelSR = rewardPanel.GetComponent<SpriteRenderer>();
            int divider = rewardsNumber + 1;
            float shopPanelCardDistance = rewardPanelSR.bounds.size.x / divider;
            for (int i = 0; i < rewardsNumber; i++)
            {
                Card card = cardDictionary.cardDefs[Random.Range(0, cardDictionary.cardDefs.Count)].card;
                GameObject newCard = Instantiate(cardPrefab, rewardPanel.transform.position + new Vector3(rewardPanelSR.bounds.min.x + (i + 1) * shopPanelCardDistance, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
                newCard.GetComponent<CardSetting>().SetupCard(card);
                RunState.deck.Add(card.id);
            }
            returnButton.interactable = true;
            rewardTaken = true;
            RunState.currentEncounter.encounterState = EncounterStateEnum.Completed;
        }
        
    }
    private void OnMouseEnter()
    {
        timePassed = 0;
    }

    private void OnMouseOver()
    {
        timePassed += Time.deltaTime;
        spriteObject.transform.Rotate(new Vector3(0, 0, 1), Mathf.Cos(timePassed*12));
    }

    private void OnMouseExit()
    {
        spriteObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelState
{
    inactive,

    setup,

    dealHand,

    pause,

    cardPick,
    dragging,

    placing,

    simulation
}

public struct HandCard
{
    public GameObject cardInstance;
    public Vector3 cardRotation;
}

public class CardPanelScript : MonoBehaviour
{
    //default cart template
    public GameObject CardPrefab = null;

    //object that determines deck location
    public GameObject DeckObInScene = null;

    public CardDictionary cardDictionary;

    public PanelState panelState;

    //default draw amount
    public int drawAmount;

    //border card dist from center per new card
    public float cardDist = 1.0f;
    public float cardTilt = 2.0f;
    public float bowHeight = 0.2f;

    //draw mechanics
    public float drawDelay = 0.8f;
    public float drawEaseIn = 0.02f;
    float drawTimer = 0.0f;
    int drawn;

    //pause mechanics for delays in panel actions
    public float postDrawPause = 1.0f;
    float pauseTimer = 0.0f;
    PanelState postPause;

    //where should cards be placed
    public Vector3 cardsCenter;

    //cards in hand
    public List<HandCard> cards;

    //deck card ids
    public List<int> deck;

    // Start is called before the first frame update
    void Start()
    {
        //set first state
        if (CardPrefab == null || DeckObInScene == null)
        {
            panelState = PanelState.inactive;
            Debug.Log("Game objects not set");
        }
        else
        {
            panelState = PanelState.setup;
        }

        //set default post pause state
        postPause = PanelState.inactive;

        //deck card ids
        deck = new List<int>();

        //cards in hand
        cards = new List<HandCard>();
    }

    // Update is called once per frame
    void Update()
    {
        //recalculate card positions
        int cardsCount = cards.Count;
        float leftDistance = -cardDist * (float)cardsCount + cardDist;
        float leftTilt = cardTilt * (float)cardsCount - cardTilt;

        //card transforms
        int i = 0;
        foreach(HandCard cd in cards)
        {
            //position
            Vector3 cardPlace =
                cardsCenter + new Vector3(
                        leftDistance + (float)i * cardDist * 2.0f, 
                        Mathf.Sin((float)i / ((float)cardsCount - 0.99f) * Mathf.PI) * bowHeight * cardsCount, 
                        -(float)i + 50.0f
                    );

            cd.cardInstance.transform.position += (cardPlace - cd.cardInstance.transform.position) * drawEaseIn;

            //rotation
            Vector3 targetRot = cd.cardRotation + new Vector3(0.0f, 0.0f, leftTilt - (float)i * cardTilt * 2.0f);

            Vector3 currentRot = cd.cardInstance.transform.rotation.eulerAngles;

            if (currentRot.z > 180.0f)
            {
                currentRot.z = currentRot.z - 360.0f;
            }

            currentRot += (targetRot - currentRot) * drawEaseIn;

            cd.cardInstance.transform.rotation = Quaternion.Euler(currentRot);
            i++;
        }

        //state based behaviour
        switch(panelState)
        {
            //Setup the card panel
            case PanelState.setup:

                //Load deck
                deck = new List<int>
                {
                    0, 1, 2, 1, 0,
                    0, 1, 2, 1, 0,
                    0, 1, 2, 1, 0,
                    0, 1, 2, 1, 0,
                };

                panelState = PanelState.dealHand;
                drawn = 0;
                break;

            //Setup the card panel
            case PanelState.dealHand:

                if (drawTimer > 0)
                {
                    drawTimer -= Time.deltaTime;
                }
                else
                {
                    //draw cards
                    if (drawn < drawAmount)
                    {
                        HandCard drawnCard = new HandCard
                        {
                            cardInstance = Instantiate(
                                                CardPrefab, 
                                                DeckObInScene.transform.position + new Vector3(0.0f, 0.0f, -100.0f), 
                                                Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f))
                                                ),

                            cardRotation = new Vector3(0.0f,0.0f,0.0f)
                        };

                        int drawnCardID = deck[0];
                        deck.RemoveAt(0);

                        drawnCard.cardInstance.GetComponent<CardSetting>()
                            .SetupCard(cardDictionary.cardDefs[drawnCardID]);

                        cards.Add(drawnCard);

                        drawn++;
                        drawTimer = drawDelay;
                    }
                    else
                    {
                        //set post draw delay
                        panelState = PanelState.pause;
                        postPause = PanelState.cardPick;
                        pauseTimer = postDrawPause;
                    }
                }
                break;

            //Delay in card panel actions
            case PanelState.pause:

                pauseTimer -= Time.deltaTime;
                if(pauseTimer <= 0.0f)
                {
                    pauseTimer = 0.0f;
                    panelState = postPause;
                }

                break;

            //Player is picking a card (cursor is free)
            case PanelState.cardPick:
                break;

            //Player is dragging a card
            case PanelState.dragging:
                break;

            //Player is placing an object
            case PanelState.placing:
                break;

            //During simulation
            case PanelState.simulation:
                break;
        }
    }
}

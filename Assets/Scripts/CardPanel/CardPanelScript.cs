using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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

public class HandCard
{
    public int baseId;
    public GameObject cardInstance;
    public Vector3 cardScale;
    public bool hovered;
    public bool selected;
    public Placeable effectPrefab;
    public Placeable spawnedPlaceableInstance;
    public bool played;
}

public class CardPanelScript : MonoBehaviour
{
    //default cart template
    public GameObject CardPrefab = null;

    //object that determines deck location
    public GameObject DeckObInScene = null;

    public GameObject DiscardObInScene = null;

    public CardDictionary cardDictionary;

    public ProjectileSpawner projSpawner = null;

    public PanelState panelState;

    public int hovered = -1;
    int lastSelectedIndex = -1;

    [Space]
    [Space]

    //default draw amount
    public int drawAmount;

    //border card dist from center per new card
    public float cardDist = 1.0f;
    public float cardTilt = 2.0f;
    public float bowHeight = 0.2f;

    //select properties
    public float baseScale = 1.0f;
    public float cardZoom = 1.5f;
    public float hoveredHeight = 0.0f;
    public float distFromHovered = 0.5f;
    public float hoverDistMulti = 1.35f;

    //play properties
    public float playHeight = -3.0f;

    //draw mechanics
    public float drawDelay = 0.8f;
    public float drawEaseIn = 0.02f;
    float drawTimer = 0.0f;
    int drawn;

    //simulation
    public float simulationTimer;
    bool shouldStartSimulation = false;

    //pause mechanics for delays in panel actions
    public float postDrawPause = 1.0f;
    public float simulationPause = 5.0f;
    float pauseTimer = 0.0f;
    PanelState postPause;

    //where should cards be placed
    public Vector3 cardsCenter;

    //cards in deck during play
    public List<int> currentDeck;

    //cards in hand
    public List<HandCard> cards;

    //deck card ids
    public List<int> discarded;

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

        //cards in hand
        cards = new List<HandCard>();

        //cards in hand
        discarded = new List<int>();

        projSpawner = FindObjectOfType<ProjectileSpawner>();

        simulationTimer = 0.0f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //recalculate card positions
        int cardsCount = cards.Count;
        float leftDistance = -cardDist * (float)cardsCount + cardDist;
        float leftTilt = cardTilt * (float)cardsCount - cardTilt;

        //card transforms
        int i = 0;
        foreach(HandCard cd in cards)
        {
            //default
            Vector3 cardPlace = new Vector3(0,0,0);

            if (panelState != PanelState.simulation)
            {
                //calc distance from hovered
                float hovDist = 0.0f;

                if (hovered != -1 && i != hovered)
                {
                    hovDist = ((i - hovered) * distFromHovered) / Mathf.Pow((float)Mathf.Abs(i - hovered), hoverDistMulti);
                }

                //position
                cardPlace =
                    cardsCenter + new Vector3(
                            leftDistance + (float)i * cardDist * 2.0f + hovDist,
                            Mathf.Sin((float)i / ((float)cardsCount - 0.99f) * Mathf.PI) * bowHeight * cardsCount,
                            -(float)i + 50.0f
                        );
            }
            else
            {
                cardPlace = DiscardObInScene.transform.position;
            }

            if (cd.hovered)
            {
                if (cd.selected)
                {
                    cardPlace = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    cardPlace.z = -500.0f;

                    cd.cardInstance.transform.position += 
                        (cardPlace - cd.cardInstance.transform.position) * drawEaseIn * 3.0f;
                }
                else
                {
                    cardPlace.y = hoveredHeight;
                    cardPlace.z = -500.0f;
                    cd.cardInstance.transform.position += 
                        (cardPlace - cd.cardInstance.transform.position) * drawEaseIn * 3.0f;
                }
            }
            else
            {
                if (cd.cardInstance.transform.position.z != cardPlace.z)
                {
                    Vector3 instantZ = new Vector3(
                        cd.cardInstance.transform.position.x,
                        cd.cardInstance.transform.position.y,
                        cardPlace.z);
                    cd.cardInstance.transform.position = instantZ;
                }

                cd.cardInstance.transform.position += (cardPlace - cd.cardInstance.transform.position) * drawEaseIn;
            }

            //rotation
            Vector3 targetRot = new Vector3(0.0f, 0.0f, leftTilt - (float)i * cardTilt * 2.0f);

            Vector3 currentRot = cd.cardInstance.transform.rotation.eulerAngles;

            if (currentRot.z > 180.0f)
            {
                currentRot.z = currentRot.z - 360.0f;
            }

            if (cd.hovered)
            {
                currentRot = new Vector3(0,0,0);
            }
            else
            {
                currentRot += (targetRot - currentRot) * drawEaseIn;
            }

            cd.cardInstance.transform.rotation = Quaternion.Euler(currentRot);

            //Scale
            if (cd.cardInstance.transform.localScale.magnitude <= cd.cardScale.magnitude)
            {
                cd.cardInstance.transform.localScale = cd.cardScale;
            }
            else
            {
                cd.cardInstance.transform.localScale += 
                    (cd.cardScale - cd.cardInstance.transform.localScale) * drawEaseIn;
            }

            i++;
        }

        //state based behaviour
        switch(panelState)
        {
            //Setup the card panel
            case PanelState.setup:
                if (RunState.deck == null)
                {
                    //Load deck
                    RunState.deck = new List<int>
                    {
                        0, 1, 2, 1, 0,
                        0, 1, 2, 1, 0,
                        0, 1, 2, 1, 0,
                        0, 1, 2, 1, 0,
                    };
                }
                currentDeck.AddRange(RunState.deck);

                //shuffle
                discarded.AddRange(currentDeck);
                currentDeck.Clear();

                if (currentDeck.Count <= 0)
                {
                    while (discarded.Count > 0)
                    {
                        int k = (int)Random.Range(0.0f, (float)discarded.Count - 0.001f);

                        currentDeck.Add(discarded[k]);
                        discarded.RemoveAt(k);
                    }
                }

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
                        if (currentDeck.Count <= 0)
                        {
                            while(discarded.Count > 0) 
                            {
                                int k = (int)Random.Range(0.0f, (float)discarded.Count - 0.001f);

                                currentDeck.Add(discarded[k]);
                                discarded.RemoveAt(k);
                            }
                        }

                        HandCard drawnCard = new HandCard
                        {
                            cardInstance = Instantiate(
                                                CardPrefab,
                                                DeckObInScene.transform.position + new Vector3(0.0f, 0.0f, -100.0f),
                                                Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f))
                                                ),

                            cardScale = new Vector3(1.0f, 1.0f, 1.0f),
                            hovered = false,
                            selected = false,
                            spawnedPlaceableInstance = null,
                            played = false                              
                        };

                        int drawnCardID = currentDeck[0];
                        currentDeck.RemoveAt(0);

                        drawnCard.cardInstance.GetComponent<CardSetting>()
                            .SetupCard(cardDictionary.GetCard(drawnCardID));

                        drawnCard.effectPrefab = cardDictionary.GetCard(drawnCardID).effectPrefab;

                        drawnCard.baseId = drawnCardID;

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
                        drawn = 0;
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

                bool simultaing = true;

                if(simulationTimer >= 0.0f)
                {
                    simulationTimer -= Time.deltaTime;
                }
                else
                {
                    simultaing = false;
                }

                //end turn
                if (shouldStartSimulation && !simultaing)
                {
                    shouldStartSimulation = false;
                    panelState = PanelState.simulation;

                    for (int j = 0; j < cards.Count; j++)
                    {
                        HandCard card = cards[j];
                        card.cardScale = new Vector3(baseScale, baseScale, baseScale);
                        card.hovered = false;
                        card.selected = false;
                        cards[j] = card;
                    }

                    if (projSpawner.CanLaunch())
                    {
                        projSpawner.OnLaunched();
                    }
                }

                // Set Card Layer mask
                int layerMask = 7;
                layerMask = ~layerMask;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                bool ifHit = Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);

                for (int j = 0; j < cards.Count; j++)
                {
                    if (ifHit && cards[j].cardInstance == hit.collider.gameObject)
                    {
                        HandCard card = cards[j];
                        card.cardScale = new Vector3(cardZoom, cardZoom, cardZoom);
                        card.hovered = true;
                        cards[j] = card;
                        hovered = j;
                    }
                    else
                    {
                        HandCard card = cards[j];
                        card.cardScale = new Vector3(baseScale, baseScale, baseScale);
                        card.hovered = false;
                        cards[j] = card;
                    }

                    if (cards[j].spawnedPlaceableInstance != null)
                    {
                        var highlight = cards[j].spawnedPlaceableInstance.GetComponent<Highlight>();
                        if (highlight)
                        {
                            highlight.SetHighlight(cards[j].hovered);
                        }
                    }
                }

                if (!ifHit)
                {
                    hovered = -1;
                }

                //get hovered card, select only if wasnt already played
                if (Input.GetMouseButtonDown(0) && hovered != -1 && !cards[hovered].played && !simultaing)
                {
                    HandCard card = cards[hovered];
                    card.selected = true;
                    cards[hovered] = card;
                    panelState = PanelState.dragging;
                }

                //cancel hovered, played card
                if (Input.GetMouseButtonDown(1) && hovered != -1 && cards[hovered].played && !simultaing)
                {
                    projSpawner.RemovePlaceable(cards[hovered].spawnedPlaceableInstance);
                    CancelPlayedCard(cards[hovered].spawnedPlaceableInstance);
                }

                break;

            //Player is dragging a card
            case PanelState.dragging:

                //flag to play card
                bool ifPlay = false;

                if (Input.GetMouseButtonDown(1))
                {
                    panelState = PanelState.cardPick;

                    for (int j = 0; j < cards.Count; j++)
                    {
                        HandCard card = cards[j];
                        card.cardScale = new Vector3(baseScale, baseScale, baseScale);
                        card.hovered = false;
                        card.selected = false;
                        cards[j] = card;
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < playHeight)
                    {
                        panelState = PanelState.cardPick;

                        for (int j = 0; j < cards.Count; j++)
                        {
                            HandCard card = cards[j];
                            card.cardScale = new Vector3(baseScale, baseScale, baseScale);
                            card.hovered = false;
                            card.selected = false;
                            cards[j] = card;
                        }
                    }
                    else
                    {
                        ifPlay = true;
                    }
                }

                if(Input.GetMouseButtonUp(0))
                {
                    if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > playHeight)
                    {
                        ifPlay = true;
                    }
                }

                //play card
                if (ifPlay)
                {
                    Placeable toPlace = null;

                    panelState = PanelState.placing;
                    for (int j = 0; j < cards.Count; j++)
                    {
                        HandCard card = cards[j];
                        if (card.selected)
                        {
                            card.hovered = true;
                            card.selected = false;
                            cards[j] = card;

                            toPlace = card.effectPrefab;
                            lastSelectedIndex = j;
                        }
                    }

                    projSpawner.StartPlacing(toPlace);
                }

                break;

            //Player is placing an object
            case PanelState.placing:

                // instance was placed -> release card
                if (lastSelectedIndex != -1)
                {
                    if (cards[lastSelectedIndex].spawnedPlaceableInstance != null)
                        panelState = PanelState.cardPick;
                }

                //release
                if (Input.GetMouseButtonDown(1))
                {
                    panelState = PanelState.cardPick;

                    for (int j = 0; j < cards.Count; j++)
                    {
                        HandCard card = cards[j];
                        card.cardScale = new Vector3(baseScale, baseScale, baseScale);
                        card.hovered = false;
                        card.selected = false;
                        cards[j] = card;
                    }

                    projSpawner.OnCancelPlacing();
                }

                break;

            //During simulation
            case PanelState.simulation:

                if (cards.Count > 0)
                {
                    //add cards to discard pile
                    List<HandCard> leftCards = new List<HandCard>();

                    for (int j = 0; j < cards.Count; j++)
                    {
                        HandCard card = cards[j];

                        if (card.cardInstance != null)
                        {
                            if ((card.cardInstance.transform.position -
                                DiscardObInScene.transform.position).magnitude < 0.5f)
                            {
                                discarded.Add(card.baseId);
                                Destroy(card.cardInstance);
                            }
                            else
                            {
                                leftCards.Add(card);
                            }
                        }
                    }
                    cards = leftCards;
                }
                else
                {
                    panelState = PanelState.dealHand;
                    simulationTimer = simulationPause;
                }

                break;
        }
    }
    [ContextMenu("Reset Deck")]
    void ResetDeck()
    {
        RunState.deck = null;
    }

    public void AssignInstanceToCurrentCard(Placeable placeable)
    {
        if (lastSelectedIndex == -1) return;

        var card = cards[lastSelectedIndex];
        if (card.spawnedPlaceableInstance == null)
        {
            card.spawnedPlaceableInstance = placeable;
            card.cardInstance.GetComponent<CardSetting>().SetOpacity(0.5f);
            card.played = true;
        }       
    }

    public void CancelPlayedCard(Placeable placeable)
    {
        for (int j = 0; j < cards.Count; j++)
        {
            var card = cards[j];
            if (card.spawnedPlaceableInstance == placeable)
            {
                card.spawnedPlaceableInstance = null;
                card.cardInstance.GetComponent<CardSetting>().SetOpacity(1.0f);
                card.played = false;
                
                return;
            }
        }
    }

    public void PickupPlayedCard(Placeable placeable)
    {
        for (int j = 0; j < cards.Count; j++)
        {
            var card = cards[j];
            if (card.spawnedPlaceableInstance == placeable)
            {
                lastSelectedIndex = j;
                
                card.spawnedPlaceableInstance = null;
                card.hovered = true;

                panelState = PanelState.placing;

                return;
            }
        }
    }

    public void StartSimulation()
    {
        shouldStartSimulation = true;
    }
}

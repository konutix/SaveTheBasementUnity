using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] CardDictionary cardDictionary;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject shopPanel;
    [SerializeField] int cardToBuyAmount;
    [SerializeField] VampireFangs vampireFangsGO;

    List<GameObject> cards;
    bool stopRotating;

    private void Awake()
    {
        cards = new List<GameObject>();
        stopRotating = false;

        SpriteRenderer shopPanelSR = shopPanel.GetComponent<SpriteRenderer>();
        float shopPanelCardDistance = shopPanelSR.bounds.size.x / (cardToBuyAmount+1);
        print(shopPanelSR.bounds.size.x);
        for (int i = 0; i < cardToBuyAmount; i++)
        {
            Card card = cardDictionary.cardDefs[Random.Range(0, cardDictionary.cardDefs.Count)].card;
            cards.Add(Instantiate(cardPrefab, shopPanel.transform.position + new Vector3(shopPanelSR.bounds.min.x + (i+1) * shopPanelCardDistance, 0.0f, 0.0f), Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f))));
            cardPrefab.GetComponent<CardSettingShop>().SetupCard(card);
        }
    }

    private void Update()
    {
        if(!stopRotating)
        {
            for (int i = 0; i < cardToBuyAmount; i++)
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

            if (go == cards[cardToBuyAmount-1])
            {
                stopRotating = true;
            }
            ShowPrice(go);
        }
    }

    void ShowPrice(GameObject card)
    {
        card.GetComponent<CardSettingShop>().shopCostPanel.SetActive(true);
    }
}

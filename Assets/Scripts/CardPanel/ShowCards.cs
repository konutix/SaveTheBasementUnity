using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCards : MonoBehaviour
{
    [SerializeField] CardDictionary cardDictionary;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject cardPanelScrollContent;
    [SerializeField] CardList cardList;
    [SerializeField] bool sort;


    IEnumerator Start()
    {
        yield return null;
        Show();
    }
    void Show()
    {
        RectTransform cardPanelRect = cardPanelScrollContent.GetComponent<RectTransform>();
        RectTransform cardRect = cardPrefab.GetComponent<RectTransform>();
        List<int> list = cardList.GetCards();
        int listCount = list.Count;
        int cardsInRow = Mathf.FloorToInt(cardPanelRect.rect.width / (cardRect.rect.width * cardRect.localScale.x));
        if (sort)
        {
            list.Sort();
        }

        cardPanelRect.sizeDelta = new Vector2(cardPanelRect.sizeDelta.x, (cardRect.rect.height * (listCount / cardsInRow)) * cardRect.localScale.y);
        for (int i = 0; i < listCount; i++)
        {
            Card card = cardDictionary.cardDefs[list[i]].card;
            GameObject newCard = Instantiate(
                cardPrefab,
                new Vector2(0, 0),
                Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)),
                cardPanelScrollContent.transform);
            newCard.GetComponent<RectTransform>().anchoredPosition =  new Vector2(cardPanelRect.rect.xMin, cardPanelRect.rect.yMax) +
                cardRect.localScale * new Vector2(
                    cardRect.rect.width * 0.5f + cardRect.rect.width * (i % cardsInRow),
                    -cardRect.rect.height * 0.5f - cardRect.rect.height * (i / cardsInRow)) ;
            newCard.GetComponent<CardSetting>().SetupCard(card);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSetting : MonoBehaviour
{
    public int cardID;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI type;
    public TextMeshProUGUI description;
    public TextMeshProUGUI mana;

    public Image art;

    public CanvasGroup canvasGroup;

    public virtual void SetupCard(Card card)
    {
        cardID = card.id;
        cardName.text = card.cardName;
        type.text = card.type;
        description.text = card.description;

        mana.text = card.manaCost.ToString();

        art.sprite = card.art;
    }

    public void SetOpacity(float alpha)
    {
        canvasGroup.alpha = alpha;
    }
}

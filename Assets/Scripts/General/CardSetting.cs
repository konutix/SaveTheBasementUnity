using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSetting : MonoBehaviour
{
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI type;
    public TextMeshProUGUI description;
    public TextMeshProUGUI mana;

    public Image art;

    public void SetupCard(Card card)
    {
        cardName.text = card.cardName;
        type.text = card.type;
        description.text = card.description;

        mana.text = card.manaCost.ToString();

        art.sprite = card.art;
    }
}

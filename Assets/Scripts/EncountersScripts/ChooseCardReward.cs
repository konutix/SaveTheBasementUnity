using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCardReward : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] GameObject canvasObject;
    [SerializeField] protected CardSetting cardSetting;
    [SerializeField] ParticleSystem particles;
    bool isMouseOver = false;
    private void OnMouseEnter()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

    private void OnMouseDown()
    {
        AddCardToDeck();
    }

    protected virtual void AddCardToDeck()
    {
        if (isMouseOver)
        {
            boxCollider.enabled = false;
            canvasObject.SetActive(false);
            RunState.deck.Add(cardSetting.cardID);
            particles.Play();
        }
    }
}

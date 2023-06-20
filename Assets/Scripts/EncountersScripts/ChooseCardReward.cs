using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCardReward : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] GameObject canvasObject;
    [SerializeField] protected CardSetting cardSetting;
    [SerializeField] ParticleSystem particles;

    private void OnMouseDown()
    {
        OnClick();
    }

    protected virtual void OnClick()
    {
        AddCardToDeck();
    }

    public virtual void AddCardToDeck()
    {
        boxCollider.enabled = false;
        canvasObject.SetActive(false);
        RunState.deck.Add(cardSetting.cardID);
        particles.Play();
    }
}

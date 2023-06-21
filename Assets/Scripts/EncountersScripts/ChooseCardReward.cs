using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCardReward : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] GameObject canvasObject;
    [SerializeField] protected CardSetting cardSetting;
    [SerializeField] ParticleSystem particles;

    public delegate void RewardGet(GameObject gameObject);
    public RewardGet onRewardGet;

    private void Start()
    {
        onRewardGet += AddCardToDeck;
    }

    private void OnMouseDown()
    {
        OnClick();
    }

    protected virtual void OnClick()
    {
        onRewardGet?.Invoke(gameObject);
    }

    public virtual void AddCardToDeck(GameObject gameObject)
    {
        boxCollider.enabled = false;
        canvasObject.SetActive(false);
        RunState.deck.Add(cardSetting.cardID);
        particles.Play();
    }
}

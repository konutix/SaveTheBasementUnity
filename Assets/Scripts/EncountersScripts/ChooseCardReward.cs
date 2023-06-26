using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCardReward : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] GameObject canvasObject;
    [SerializeField] protected CardSetting cardSetting;
    [SerializeField] ParticleSystem particles;
    [SerializeField] public GameObject selection;

    public delegate void RewardSet(GameObject gameObject);
    public RewardSet onRewardSet;

    private void Start()
    {
        onRewardSet += SetSelection;
    }

    private void OnMouseDown()
    {
        OnClick();
    }

    protected virtual void OnClick()
    {
        onRewardSet?.Invoke(gameObject);
    }

    public virtual void AddCardToDeck()
    {
        boxCollider.enabled = false;
        canvasObject.SetActive(false);
        RunState.deck.Add(cardSetting.cardID);
        particles.Play();
    }

    public void SetSelection(GameObject gameObject)
    {
        selection.SetActive(true);
    }

    public void RemoveSelection()
    {
        selection.SetActive(false);
    }
}

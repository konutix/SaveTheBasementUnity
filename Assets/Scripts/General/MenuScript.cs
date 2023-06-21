using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] CardDeck deck;
    [SerializeField] Button continueButton;

    private void Start()
    {
        if(RunState.currentMap == null)
        {
            continueButton.interactable = false;
        }
        else
        {
            continueButton.interactable = true;
        }
    }

    public void StartNewRun(string sceneName)
    {
        RunState.currentMap = null;
        RunState.currentEncounter = null;
        RunState.deck = deck.deck;
        RunState.vampireFangs = 10;
        RunState.shopRewards = null;

        SceneManager.LoadScene(sceneName);
    }

    public void ContinueRun(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] CardDeck deck;
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

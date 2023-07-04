using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelObjectiveManager : MonoBehaviour
{
    List<LevelObjective> levelObjectivesToComplete;
    [SerializeField] CardPanelScript cardPanel;
    int objectivesCount;

    bool alreadyCompleted = false;

    void Awake()
    {
        levelObjectivesToComplete = new List<LevelObjective>(FindObjectsOfType<LevelObjective>());
        levelObjectivesToComplete = levelObjectivesToComplete.FindAll(obj => !obj.failOnComplete);
        objectivesCount = levelObjectivesToComplete.Count;

        if (objectivesCount <= 0)
        {
            OnAllObjectivesCompleted();
        }
    }

    void Update()
    {
        // cheat code
        if (Input.GetKeyDown("0"))
        {
            OnAllObjectivesCompleted();
        }
    }

    public void OnObjectiveCompleted()
    {
        objectivesCount -= 1;
        if (objectivesCount <= 0 && !alreadyCompleted)
        {
            Invoke("OnAllObjectivesCompleted", 0.1f);
        }
    }

    public void OnObjectiveFailed()
    {
        alreadyCompleted = true;
        RunState.ResetData();
        cardPanel.panelState = PanelState.inactive;
        SceneManager.LoadScene("LoseScene", LoadSceneMode.Additive);
    }

    void OnAllObjectivesCompleted()
    {
        if (alreadyCompleted) return;

        alreadyCompleted = true;
        cardPanel.panelState = PanelState.inactive;
        if (RunState.currentEncounter.GetType() == typeof(BossEncounter))
        {
            RunState.ResetData();
            SceneManager.LoadScene("BossWinScene", LoadSceneMode.Additive);
        }
        else
        {
            RunState.currentPlayerHealth = (float)FindObjectOfType<ProjectileSpawner>().player.currentHealth;
            SceneManager.LoadScene("WinScene", LoadSceneMode.Additive);
        }
    }
}

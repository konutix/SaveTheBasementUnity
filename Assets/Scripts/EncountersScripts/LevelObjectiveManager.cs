using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelObjectiveManager : MonoBehaviour
{
    List<LevelObjective> levelObjectivesToComplete;
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

    public void OnObjectiveCompleted()
    {
        objectivesCount -= 1;
        if (objectivesCount <= 0 && !alreadyCompleted)
        {
            OnAllObjectivesCompleted();
        }
    }

    public void OnObjectiveFailed()
    {
        alreadyCompleted = true;
        SceneManager.LoadScene("Menu");
    }

    void OnAllObjectivesCompleted()
    {
        if (alreadyCompleted) return;

        alreadyCompleted = true;
        SceneManager.LoadScene("EncounterReward");
    }
}

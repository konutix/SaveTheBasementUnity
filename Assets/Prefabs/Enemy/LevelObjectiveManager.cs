using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjectiveManager : MonoBehaviour
{
    List<LevelObjective> levelObjectives;
    int objectivesCount;

    bool alreadyCompleted = false;

    void Awake()
    {
        levelObjectives = new List<LevelObjective>(FindObjectsOfType<LevelObjective>());
        objectivesCount = levelObjectives.Count;

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

    void OnAllObjectivesCompleted()
    {
        alreadyCompleted = true;
        print("FINISHED LEVEL !!!!");
    }
}

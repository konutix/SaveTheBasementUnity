using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjective : MonoBehaviour
{
    public bool failOnComplete = false;

    public void Complete()
    {
        if (failOnComplete)
        {
            FindObjectOfType<LevelObjectiveManager>().OnObjectiveFailed();
        }
        {
            FindObjectOfType<LevelObjectiveManager>().OnObjectiveCompleted();
        }
    }
}

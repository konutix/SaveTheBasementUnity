using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjective : MonoBehaviour
{
    public void Complete()
    {
        FindObjectOfType<LevelObjectiveManager>().OnObjectiveCompleted();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultipleEnemyEncounter : Encounter
{
    public override void LaunchEncounter()
    {
        base.LaunchEncounter();
        int level = Random.Range(1, 3);
        SceneManager.LoadScene("Enc_med_" + level);
    }
}

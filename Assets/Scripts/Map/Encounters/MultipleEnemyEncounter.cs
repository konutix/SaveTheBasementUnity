using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultipleEnemyEncounter : Encounter
{
    int level;

    public MultipleEnemyEncounter()
    {
        //wiêksza liczba musi byæ o 1 wiêksza od numeru ostatniego poziomu tego typu
        level = Random.Range(1, 3);
    }

    public override void LaunchEncounter()
    {
        base.LaunchEncounter();
        SceneManager.LoadScene("Enc_med_" + level);
    }
}

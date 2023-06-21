using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultipleEnemyEncounter : Encounter
{
    public override void LaunchEncounter()
    {
        base.LaunchEncounter();

        //wiêksza liczba musi byæ o 1 wiêksza od numeru ostatniego poziomu tego typu
        int level = Random.Range(1, 3);
        SceneManager.LoadScene("Enc_med_" + level);
    }
}

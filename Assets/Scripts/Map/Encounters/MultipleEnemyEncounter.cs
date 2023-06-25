using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultipleEnemyEncounter : Encounter
{
    int level;

    public MultipleEnemyEncounter()
    {
        //wi�ksza liczba musi by� o 1 wi�ksza od numeru ostatniego poziomu tego typu
        level = Random.Range(1, 3);
    }

    public override void LaunchEncounter()
    {
        base.LaunchEncounter();
        SceneManager.LoadScene("Enc_med_" + level);
    }
}

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
        level = Random.Range(1, 4);
    }

    public override void LaunchEncounter()
    {
        base.LaunchEncounter();
        level = GetLevelIndex();
        SceneManager.LoadScene("Enc_med_" + level);
    }

    static List<int> randomLevels;
    static int previousLevelIndex = -1;

    private void InitializeLevelsList(int numberOfLevels)
    {
        randomLevels = new List<int>();
        for (int i = 1; i <= numberOfLevels; i++)
        {
            randomLevels.Add(i);
        }
    }

    private int GetLevelIndex()
    {
        if (randomLevels == null || randomLevels.Count < 1)
        {
            InitializeLevelsList(3);
        }

        int i = -1, value = -1;

        do
        {
            i = Random.Range(0, randomLevels.Count);
            value = randomLevels[i];
        } while (value == previousLevelIndex);

        randomLevels.RemoveAt(i);
        previousLevelIndex = value;
        
        return value;
    }  
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEncounter : Encounter
{
    public override void LaunchEncounter()
    {
        base.LaunchEncounter();
        SceneManager.LoadScene("Enc_boss");
    }
}

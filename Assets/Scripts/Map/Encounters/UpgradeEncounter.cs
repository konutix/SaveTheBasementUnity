using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeEncounter : Encounter
{
    public override void LaunchEncounter()
    {
        base.LaunchEncounter();
        SceneManager.LoadScene("Upgrade");
    }
}

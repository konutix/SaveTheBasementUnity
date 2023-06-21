using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartSimulationButton : MonoBehaviour
{
    [SerializeField] TMP_Text buttonText;

    bool canPress = true;
    ProjectileSpawner projectileSpawner;

    void Start()
    {
        projectileSpawner = FindObjectOfType<ProjectileSpawner>();
        projectileSpawner.simulationStopEvent += OnSimulationEnd;
    }

    public void OnButtonClick()
    {
        if (canPress)
        {
            canPress = false;
            buttonText.text = "Playing...";
            projectileSpawner.StartSimulation();
        }
    }

    public void OnSimulationEnd()
    {
        canPress = true;
        buttonText.text = "Start";
    }

    private void OnDestroy() {
        projectileSpawner.simulationStopEvent -= OnSimulationEnd;
    }
}

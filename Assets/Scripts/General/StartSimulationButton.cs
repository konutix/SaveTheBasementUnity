using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartSimulationButton : MonoBehaviour
{
    [SerializeField] TMP_Text buttonText;
    public Color activeColor = Color.yellow;
    public Color inactiveColor = Color.red;

    bool canPress = true;
    ProjectileSpawner projectileSpawner;

    void Start()
    {
        projectileSpawner = FindObjectOfType<ProjectileSpawner>();
        projectileSpawner.simulationStopEvent += OnSimulationEnd;

        GetComponent<Image>().color = activeColor;
    }

    public void OnButtonClick()
    {
        if (canPress && projectileSpawner.CanLaunch())
        {
            canPress = false;
            projectileSpawner.StartSimulation();

            StartCoroutine(ButtonAnimation(true));
        }
    }

    public void OnSimulationEnd()
    {
        StartCoroutine(ButtonAnimation(false));
    }

    private void OnDestroy() {
        projectileSpawner.simulationStopEvent -= OnSimulationEnd;
    }

    IEnumerator ButtonAnimation(bool isOnSimulation)
    {
        var rectTransform = GetComponent<RectTransform>();
        var rect = rectTransform.rect;
        float height = 50.0f;

        Vector3 startPos = rectTransform.localPosition;

        float t = 0.0f;
        float speed = 5.5f;
        while (t < 1.0f)
        {
            rectTransform.localScale = new Vector3(1.0f, 1.0f - t, 1.0f);
            rectTransform.localPosition = startPos - Vector3.up * t * height * 0.5f;

            t += Time.deltaTime * speed;
            yield return null;
        }

        if (isOnSimulation)
        {
            GetComponent<Image>().color = inactiveColor;
            buttonText.text = "Playing...";
        }
        else
        {
            GetComponent<Image>().color = activeColor;
            buttonText.text = "Start";
            canPress = true;
        }

        while (t > 0.0f)
        {
            rectTransform.localScale = new Vector3(1.0f, 1.0f - t, 1.0f);
            rectTransform.localPosition = startPos - Vector3.up * t * height * 0.5f;

            t -= Time.deltaTime * speed;
            yield return null;
        }
    }
}

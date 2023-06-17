using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthController : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] TMP_Text healthText;

    float targetFillAmount = 1.0f;

    public void UpdateHealth(BattleStats stats)
    {
        targetFillAmount = (float) stats.currentHealth / stats.maxHealth;
        healthText.text = stats.currentHealth + "/" + stats.maxHealth;
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFillAmount, 6.54321f * Time.deltaTime);
    }
}

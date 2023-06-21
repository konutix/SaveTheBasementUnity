using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthController : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] TMP_Text healthText;

    [Space]
    [SerializeField] TMP_Text strengthText;
    [SerializeField] TMP_Text weakText;
    [SerializeField] TMP_Text vulnerableText;

    [Space]
    [SerializeField] ParticleSystem powerUpParticles;
    [SerializeField] ParticleSystem powerDownParticles;

    float targetFillAmount = 1.0f;

    public void UpdateHealth(BattleStats stats)
    {
        targetFillAmount = (float) stats.currentHealth / stats.maxHealth;
        healthText.text = stats.currentHealth + "/" + stats.maxHealth;
    }

    public void UpdateModifiers(BattleStats stats, ApplyStatsModifier modifier)
    {
        strengthText.text = stats.strength.ToString();
        strengthText.transform.parent.gameObject.SetActive(stats.strength != 0);

        weakText.text = stats.weak.ToString();
        weakText.transform.parent.gameObject.SetActive(stats.weak != 0);

        vulnerableText.text = stats.vulnerable.ToString();
        vulnerableText.transform.parent.gameObject.SetActive(stats.vulnerable != 0);

        if (modifier.strengthAmount > 0) powerUpParticles.Play();
        if (modifier.weakAmount > 0 || modifier.vulnerableAmount > 0) powerDownParticles.Play();
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFillAmount, 6.54321f * Time.deltaTime);
    }
}

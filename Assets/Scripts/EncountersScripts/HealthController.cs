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
    [SerializeField] TMP_Text vampirismText;

    [Space]
    [SerializeField] ParticleSystem powerUpParticles;
    [SerializeField] ParticleSystem powerDownParticles;

    float targetFillAmount = 1.0f;

    public void UpdateHealth(BattleStats stats, bool immediate = false)
    {
        targetFillAmount = (float) stats.currentHealth / stats.maxHealth;
        healthText.text = stats.currentHealth + "/" + stats.maxHealth;

        if (immediate)
        {
            healthBar.fillAmount = targetFillAmount;
        }
    }

    public void UpdateModifiers(BattleStats stats, ApplyStatsModifier modifier = null)
    {
        strengthText.transform.parent.gameObject.SetActive(stats.strength != 0);
        if (strengthText.text != stats.strength.ToString() && strengthText.transform.parent.gameObject.activeSelf) strengthText.GetComponent<Animator>().SetTrigger("DoScale");
        strengthText.text = stats.strength.ToString();

        weakText.transform.parent.gameObject.SetActive(stats.weak != 0);
        if (weakText.text != stats.weak.ToString() && weakText.transform.parent.gameObject.activeSelf) weakText.GetComponent<Animator>().SetTrigger("DoScale");
        weakText.text = stats.weak.ToString();

        vulnerableText.transform.parent.gameObject.SetActive(stats.vulnerable != 0);
        if (vulnerableText.text != stats.vulnerable.ToString() && vulnerableText.transform.parent.gameObject.activeSelf) vulnerableText.GetComponent<Animator>().SetTrigger("DoScale");
        vulnerableText.text = stats.vulnerable.ToString();

        vampirismText.transform.parent.gameObject.SetActive(stats.vampirism != 0);
        if (vampirismText.text != stats.vampirism.ToString() && vampirismText.transform.parent.gameObject.activeSelf) vampirismText.GetComponent<Animator>().SetTrigger("DoScale");
        vampirismText.text = stats.vampirism.ToString();

        if (modifier == null) return;
        if (modifier.strengthAmount > 0) powerUpParticles.Play();
        if (modifier.weakAmount > 0 || modifier.vulnerableAmount > 0) powerDownParticles.Play();
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFillAmount, 6.54321f * Time.deltaTime);
    }
}

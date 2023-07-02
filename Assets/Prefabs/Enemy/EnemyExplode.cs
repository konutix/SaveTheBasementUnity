using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyExplode : MonoBehaviour
{
    public int explodeDamage = 20;
    public int turnsToExplode = 3;

    [Space]
    [SerializeField] TMP_Text turnsRemainingText;
    public ParticleSystem explosionParticles;


    BattleStats battleStats;
    BattleStats player;

    void Start()
    {
        battleStats = GetComponent<BattleStats>();

        var projectileSpawner = FindObjectOfType<ProjectileSpawner>();
        projectileSpawner.launchEvent += OnLaunch;
        player = projectileSpawner.player;

        turnsRemainingText.transform.parent.gameObject.SetActive(true);
        turnsRemainingText.text = turnsToExplode.ToString();
    }

    void OnLaunch()
    {
        if (battleStats.currentHealth <= 0) return;

        turnsToExplode -= 1;

        if (turnsToExplode < 0)
        {
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
            var sprite = GetComponentInChildren<SpriteRenderer>();
            if (sprite) sprite.enabled = false;

            player.TakeDamage(explodeDamage);
            battleStats.TakeDamage(9999);
        }
        else
        {
            turnsRemainingText.text = turnsToExplode.ToString();
            turnsRemainingText.GetComponent<Animator>().SetTrigger("DoScale");
        }
    }

    private void OnDestroy()
    {
        var ps = FindObjectOfType<ProjectileSpawner>();
        if (ps) ps.launchEvent -= OnLaunch;
    }
}

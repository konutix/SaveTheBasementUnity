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


    BattleStats battleStats;
    BattleStats player;

    void Start()
    {
        battleStats = GetComponent<BattleStats>();

        FindObjectOfType<ProjectileSpawner>().launchEvent += OnLaunch;
        
        foreach (var stats in FindObjectsOfType<BattleStats>())
        {
            if (!stats.GetComponent<EnemyBasic>() && !stats.GetComponent<EnemyExplode>())
            {
                player = stats;
                break;
            }
        }

        turnsRemainingText.transform.parent.gameObject.SetActive(true);
        turnsRemainingText.text = turnsToExplode.ToString();
    }

    void OnLaunch()
    {
        if (battleStats.currentHealth <= 0) return;

        turnsToExplode -= 1;

        if (turnsToExplode < 0)
        {
            player.TakeDamage(explodeDamage);
            battleStats.TakeDamage(9999);
        }
        else
        {
            turnsRemainingText.text = turnsToExplode.ToString();
        }
    }

    private void OnDestroy()
    {
        var ps = FindObjectOfType<ProjectileSpawner>();
        if (ps) ps.launchEvent -= OnLaunch;
    }
}

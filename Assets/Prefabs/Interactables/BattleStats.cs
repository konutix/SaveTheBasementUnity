using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStats : MonoBehaviour, Interactable
{
    public int maxHealth = 10;
    public int currentHealth = 10;

    [Space]
    public int strength = 0;
    public int weak = 0;
    public float weakMultiplier = 0.75f;
    public int vulnerable = 0;
    public float vulnerableMultiplier = 1.5f;


    public void OnInteractWithProjectile(Projectile projectile)
    {
        if (currentHealth <= 0) return;

        TakeDamage(projectile);
        Destroy(projectile.gameObject);
    }

    void TakeDamage(Projectile projectile)
    {
        BattleStats source = projectile.GetComponent<Placeable>().owner;

        float multiplier = ((vulnerable > 0) ? vulnerableMultiplier : 1.0f) * ((source.weak > 0) ? source.weakMultiplier : 1.0f);
        int damage = (int)((projectile.damage + source.strength) * multiplier);

        currentHealth -= damage;

        print(gameObject.name + " is taking damage: " + damage);
    }
}

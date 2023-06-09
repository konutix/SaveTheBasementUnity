using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, Interactable
{
    public int maxHealth = 10;
    public int currentHealth = 10;

    public event Action deathEvent;

    public void OnInteractWithProjectile(Projectile projectile)
    {
        TakeDamage(projectile.damage);
    }

    public void TakeDamage(int damage)
    {
        print(gameObject.name + " is taking damage: " + damage);

        currentHealth -= damage;

        var renderer = GetComponent<SpriteRenderer>();
        renderer.color = Color.Lerp(Color.red, Color.green, (float)currentHealth/maxHealth);

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    void OnDeath()
    {
        print(gameObject.name + " died");
        var renderer = GetComponent<SpriteRenderer>();
        renderer.color = Color.black;

        if (deathEvent != null) deathEvent();
    }
}

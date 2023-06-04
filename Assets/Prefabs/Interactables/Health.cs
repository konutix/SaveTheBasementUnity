using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, Interactable
{
    public int health = 10;

    public void OnInteractWithProjectile(Projectile projectile)
    {
        TakeDamage(projectile.damage);
    }

    public void TakeDamage(int damage)
    {
        print(gameObject.name + " is taking damage: " + damage);

        health -= damage;

        if (health <= 0)
        {
            OnDeath();
        }
    }

    void OnDeath()
    {
        print(gameObject.name + " died");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBoulder : MonoBehaviour, Interactable
{
    public int damage = 7;

    Rigidbody2D rb;
    bool wasHit = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rb.simulated = true;
    }

    private void Update() 
    {
        if (wasHit)
        {
            FindObjectOfType<ProjectileTrajectory>().UpdateObject(gameObject);
        }
    }

    public void OnInteractWithProjectile(Projectile projectile)
    {
        wasHit = true;
        rb.isKinematic = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var health = collision.gameObject.GetComponent<Health>();
        if (health)
        {
            health.TakeDamage(damage);
        }
    }
}

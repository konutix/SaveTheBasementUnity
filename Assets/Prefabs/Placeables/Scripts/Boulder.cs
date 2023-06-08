using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    Rigidbody2D rb;

    Vector2 prevVelocity;
    float prevAngular;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        prevVelocity = rb.velocity;
        prevAngular = rb.angularVelocity;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        var destroyable = other.gameObject.GetComponent<Destroyable>();
        if (destroyable)
        {
            GetComponent<Projectile>().bouncesCount = 9999;
            Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
            rb.velocity = prevVelocity;
            rb.angularVelocity = prevAngular;
        }
    }
}

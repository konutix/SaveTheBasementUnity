using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float launchForce = 10.0f;
    public int damage = 5;
    public int evnironmentalDamage = 1;
    [HideInInspector] public int bouncesCount = 0;

    Rigidbody2D rb;

    [HideInInspector]
    public bool isSimulatingTrajectory = false;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
        // rb.Sleep(); should be set to Start Asleep in inspector
    }

    public void Init(Vector3 direction)
    {
        rb.simulated = true;
        rb.WakeUp();
        rb.AddForce(direction * launchForce, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bouncesCount += 1;

        if (isSimulatingTrajectory)
        {
            return;
        }

        try {
            collision.gameObject.GetComponent<Interactable>().OnInteractWithProjectile(this);
        } catch { }
    }
}

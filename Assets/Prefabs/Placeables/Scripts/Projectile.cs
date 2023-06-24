using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float launchForce = 10.0f;
    public bool usePullModifier = false;

    [Space]
    public int damage = 5;
    public int evnironmentalDamage = 1;

    
    [HideInInspector] public bool isSimulatingTrajectory = false;
    [Space]
    public int simulatedBouncesCount = 0;
    [HideInInspector] public int bouncesCount = 0;

    [Space]
    public bool shouldDestroyOnHit = true;

    [Space]
    public ParticleSystem ricochetParticles;

    TrailRenderer trailRenderer;

    Rigidbody2D rb;
    bool wasLaunched = false;

    BattleStats stats;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.simulated = false;
        rb.isKinematic = true;
        //rb.Sleep(); // should be set to Start Asleep in inspector

        stats = GetComponent<Placeable>().owner;

        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer)
        {
            trailRenderer.enabled = false;
        }
    }

    public void Init(Vector3 direction)
    {
        if (trailRenderer)
        {
            trailRenderer.enabled = true;
        }
        //rb.simulated = true;
        rb.isKinematic = false;
        rb.WakeUp();
        rb.AddForce(direction * launchForce * (usePullModifier ? GetComponent<Placeable>().pull : 1.0f), ForceMode2D.Impulse);

        wasLaunched = true;
    }

    public int CalculateDamage()
    {
        if (stats)
        {
            return (int)((this.damage + stats.strength) * ((stats.weak > 0) ? stats.weakMultiplier : 1.0f));
        }

        return damage;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bouncesCount += 1;

        var bs = collision.gameObject.GetComponent<BattleStats>();
        var shield = collision.gameObject.GetComponent<Shield>();
        if (bs || (shield && shield.shouldDestroyProjectile))
        {
            bouncesCount = 9999;
        } 

        if (isSimulatingTrajectory || !wasLaunched) return;

        var interaction = collision.gameObject.GetComponent<Interactable>();
        if (interaction != null)
        {
            interaction.OnInteractWithProjectile(this);
        }
        else if (ricochetParticles != null)
        {
            if (rb.velocity.magnitude < 1.618f) return;
            var particles = Instantiate(ricochetParticles, transform.position, Quaternion.identity);
            particles.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSimulatingTrajectory || !wasLaunched) return;
        
        try {
            collision.gameObject.GetComponent<Interactable>().OnInteractWithProjectile(this);
        } catch { }
    }
}

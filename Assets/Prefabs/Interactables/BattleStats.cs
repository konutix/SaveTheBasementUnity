using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStats : MonoBehaviour, Interactable
{
    public int maxHealth = 10;
    public int currentHealth = 10;
    public HealthController healthController;

    [Space]
    public int strength = 0;
    public int weak = 0;
    public float weakMultiplier = 0.75f;
    public int vulnerable = 0;
    public float vulnerableMultiplier = 1.5f;

    [Space]
    [SerializeField] ParticleSystem gettingDamageParticles;

    void Start()
    {
        if (healthController)
        {
            healthController.UpdateHealth(this);
        }
    }

    public void OnInteractWithProjectile(Projectile projectile)
    {
        if (currentHealth <= 0) return;

        if (projectile.damage > 0) TakeDamage(projectile);
        
        var statsModifier = projectile.GetComponent<ApplyStatsModifier>();
        if (statsModifier) ApplyModifiers(statsModifier);

        if (projectile.shouldDestroyOnHit) Destroy(projectile.gameObject);
    }

    void TakeDamage(Projectile projectile)
    {
        BattleStats source = projectile.GetComponent<Placeable>().owner;

        float multiplier = ((vulnerable > 0) ? vulnerableMultiplier : 1.0f) * ((source.weak > 0) ? source.weakMultiplier : 1.0f);
        int damage = (int)((projectile.damage + source.strength) * multiplier);

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        print(gameObject.name + " is taking damage: " + damage);

        if (healthController)
        {
            healthController.UpdateHealth(this);
        }

        if (gettingDamageParticles != null)
        {
            gettingDamageParticles.transform.position = projectile.transform.position;
            gettingDamageParticles.Play();
        }

        var cameraShake = FindObjectOfType<CameraShake>();
        if (cameraShake)
        {
            cameraShake.shakeDuration = 0.1f;
        }
    }

    void ApplyModifiers(ApplyStatsModifier statsModifier)
    {
        weak += statsModifier.weakAmount;
        vulnerable += statsModifier.vulnerableAmount;
        strength += statsModifier.strengthAmount;

        if (healthController)
        {
            healthController.UpdateModifiers(this, statsModifier);
        }
    }
}

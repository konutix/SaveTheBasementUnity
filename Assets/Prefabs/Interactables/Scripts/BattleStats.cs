using System;
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
    public int vampirism = 0;

    [Space]
    public bool canBeTeleported = false;

    [Space]
    [SerializeField] ParticleSystem gettingDamageParticles;

    public event Action onDeathEvent;

    ProjectileSpawner projectileSpawner;

    void Awake()
    {
        projectileSpawner = FindObjectOfType<ProjectileSpawner>();
        projectileSpawner.simulationStopEvent += OnSimulationLaunch;
    }

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
        if (gettingDamageParticles != null)
        {
            gettingDamageParticles.transform.position = projectile.transform.position;
            gettingDamageParticles.Play();
        }

        BattleStats source = projectile.GetComponent<Placeable>().owner;

        float multiplier = ((vulnerable > 0) ? vulnerableMultiplier : 1.0f) * ((source.weak > 0) ? source.weakMultiplier : 1.0f);
        int damage = (int)((projectile.damage + source.strength) * multiplier);

        source.UseVampirism(damage);

        TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        print(gameObject.name + " is taking damage: " + damage);

        if (healthController)
        {
            healthController.UpdateHealth(this);
        }

        var cameraShake = FindObjectOfType<CameraShake>();
        if (cameraShake)
        {
            cameraShake.shakeDuration = 0.1f;
        }

        // On Death
        if (currentHealth <= 0)
        {
            var traj = FindObjectOfType<TrajectoryManager>();
            if (traj) traj.RemoveObject(gameObject);

            var objective = GetComponent<LevelObjective>();
            if (objective) objective.Complete();

            if (onDeathEvent != null) onDeathEvent();

            Destroy(gameObject, 0.4f);
        }
    }

    void UseVampirism(int damageDealt)
    {
        if (vampirism < 1) return;

        currentHealth = Mathf.Clamp((int)(damageDealt * 0.5f) + currentHealth, 0, maxHealth);
        if (healthController)
        {
            healthController.UpdateHealth(this);
        }
    }

    public void ApplyModifiers(ApplyStatsModifier statsModifier)
    {
        weak += statsModifier.weakAmount;
        vulnerable += statsModifier.vulnerableAmount;
        strength += statsModifier.strengthAmount;
        vampirism += statsModifier.vampirismAmount;

        if (healthController)
        {
            healthController.UpdateModifiers(this, statsModifier);
        }
    }
    
    void OnSimulationLaunch()
    {
        if (strength > 0) strength -= 1;
        if (weak > 0) weak -= 1;
        if (vulnerable > 0) vulnerable -= 1;
        if (vampirism > 0) vampirism -= 1;

        if (healthController)
        {
            healthController.UpdateModifiers(this);
        }
    }

    private void OnDestroy()
    {
        if (projectileSpawner) projectileSpawner.simulationStopEvent -= OnSimulationLaunch;
    }
}

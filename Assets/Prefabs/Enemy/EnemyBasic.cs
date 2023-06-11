using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    public Transform projectileSpawnPoint;
    public Placeable projectilePrefab;
    public float shootAngle = 180.0f;

    Placeable currentProjectile;
    Health health;

    void Start()
    {
        health = GetComponent<Health>();
        health.deathEvent += OnDeath;

        FindObjectOfType<ProjectileSpawner>().launchEvent += OnLaunch;
        Invoke("PlaceProjectile", 0.5f);
    }

    void PlaceProjectile()
    {       
        if (!currentProjectile && health.currentHealth > 0)
        {
            currentProjectile = Instantiate(projectilePrefab, projectileSpawnPoint);
            currentProjectile.GetComponent<Projectile>().simulatedBouncesCount = 3;
            currentProjectile.OnAiming(shootAngle);
            currentProjectile.CalculateTrajectory();
            FindObjectOfType<ProjectileSpawner>().AddPlaceable(currentProjectile);
        }
    }

    void OnLaunch()
    {
        Invoke("PlaceProjectile", 6.0f);
    }

    void OnDeath()
    {
        FindObjectOfType<ProjectileSpawner>().launchEvent -= OnLaunch;
        health.deathEvent -= OnDeath;

        Destroy(currentProjectile);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    public Transform placeableSpawnPoint;
    public Placeable[] possiblePlaceablePrefabs;
    public float shootAngle = 180.0f;
    public float angleRandomSpread = 3.0f;

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
            Placeable prefab = possiblePlaceablePrefabs[Random.Range(0, possiblePlaceablePrefabs.Length)];
            currentProjectile = Instantiate(prefab, placeableSpawnPoint);

            var projectile = currentProjectile.GetComponent<Projectile>();
            if (projectile)
            {
                projectile.simulatedBouncesCount = 3;
            }
            
            currentProjectile.OnAiming(shootAngle + Random.Range(-angleRandomSpread, angleRandomSpread));
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

        Destroy(currentProjectile.gameObject);
    }
}

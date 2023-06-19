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
    BattleStats battleStats;

    void Start()
    {
        battleStats = GetComponent<BattleStats>();

        FindObjectOfType<ProjectileSpawner>().launchEvent += OnLaunch;
        Invoke("PlaceProjectile", 0.5f);
    }

    void PlaceProjectile()
    {       
        if (!currentProjectile && battleStats.currentHealth > 0)
        {
            Placeable prefab = possiblePlaceablePrefabs[Random.Range(0, possiblePlaceablePrefabs.Length)];
            currentProjectile = Instantiate(prefab, placeableSpawnPoint);
            currentProjectile.owner = battleStats;

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

    private void OnDestroy()
    {
        var ps = FindObjectOfType<ProjectileSpawner>();
        if (ps) ps.launchEvent -= OnLaunch;
    }
}

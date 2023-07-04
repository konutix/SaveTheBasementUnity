using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    public Transform shieldSpawnPoints;
    public Transform projectileSpawnPoints;
    public Placeable[] possiblePlaceablePrefabs;

    Placeable currentProjectile;
    BattleStats battleStats;

    int prefabIndex;

    void Start()
    {
        battleStats = GetComponent<BattleStats>();

        FindObjectOfType<ProjectileSpawner>().launchEvent += OnLaunch;
        FindObjectOfType<ProjectileSpawner>().simulationStopEvent += OnFinish;
        Invoke("PlaceProjectile", 0.5f);

        prefabIndex = Random.Range(0, possiblePlaceablePrefabs.Length);
    }

    void PlaceProjectile()
    {       
        if (!currentProjectile && battleStats.currentHealth > 0)
        {
            Placeable prefab = possiblePlaceablePrefabs[prefabIndex];
            prefabIndex = (prefabIndex + 1) % possiblePlaceablePrefabs.Length;

            Transform spawnPointTransform = null;
            float shootAngle = 0.0f, randomAngleSpread = 0.0f;
            if (prefab.GetComponent<Projectile>())
            {
                int randomChildIndex = Random.Range(0, projectileSpawnPoints.childCount);
                var child = projectileSpawnPoints.GetChild(randomChildIndex);
                var spawnPoint = child.GetComponent<EnemyPlaceableSpawnPoint>();
                if (spawnPoint)
                {
                    spawnPointTransform = child;
                    shootAngle = spawnPoint.angle;
                    randomAngleSpread = spawnPoint.angleRandomSpread;
                }
            }
            else if (prefab.GetComponent<Shield>())
            {
                int randomChildIndex = Random.Range(0, shieldSpawnPoints.childCount);
                var child = shieldSpawnPoints.GetChild(randomChildIndex);
                var spawnPoint = child.GetComponent<EnemyPlaceableSpawnPoint>();
                if (spawnPoint)
                {
                    spawnPointTransform = child;
                    shootAngle = spawnPoint.angle;
                    randomAngleSpread = spawnPoint.angleRandomSpread;
                }
            }

            if (spawnPointTransform == null)
            {
                print(gameObject.name + "No spawn points");
            }

            currentProjectile = Instantiate(prefab, spawnPointTransform);
            currentProjectile.owner = battleStats;

            var projectile = currentProjectile.GetComponent<Projectile>();
            if (projectile && !currentProjectile.GetComponent<SplittingProjectile>())
            {
                projectile.simulatedBouncesCount = 3;
            }
            
            currentProjectile.OnAiming(shootAngle + Random.Range(-randomAngleSpread, randomAngleSpread));
            currentProjectile.CalculateTrajectory();
            currentProjectile.OnStoppedPlacing();
            FindObjectOfType<ProjectileSpawner>().AddPlaceable(currentProjectile);
        }
    }

    void OnLaunch()
    {
        
    }

    void OnFinish()
    {
        if (currentProjectile)
        {
            Destroy(currentProjectile.gameObject);
            currentProjectile = null;
        }

        if (battleStats.currentHealth > 0)
        {
            Invoke("PlaceProjectile", 0.5f);
        }
    }

    private void OnDestroy()
    {
        var ps = FindObjectOfType<ProjectileSpawner>();
        if (ps) ps.launchEvent -= OnLaunch;
    }
}

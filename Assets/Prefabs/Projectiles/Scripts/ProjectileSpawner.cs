using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    List<GameObject> objectsToLaunch;
    GameObject currentObject;

    public ProjectileTrajectory trajectory;


    Vector3 spawnPoint;
    Vector3 spawnDir;

    bool isAiming = false;


    void Start()
    {
        objectsToLaunch = new List<GameObject>();

        print("Left click & drag: spawn object");
        print("Right click: launch projectiles");
        print("To change object type, select prefab in Projectile Spawner");
    }

    void Update()
    {
        if (!prefabToSpawn) return;

        if (isAiming)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0.0f;
            spawnDir = Vector3.Normalize(spawnPoint - mousePos);

            var projectile = currentObject.GetComponent<Projectile>();
            if (projectile)
            {
                projectile.SetDirection(spawnDir);
                trajectory.SimulateTrajectory(projectile, spawnPoint, spawnDir);
            }

            var shield = currentObject.GetComponent<Shield>();
            if (shield)
            {
                shield.SetDirection(spawnDir);
                trajectory.SimulateShield(shield);

                foreach (var go in objectsToLaunch)
                {
                    var p = go.GetComponent<Projectile>();
                    if (p)
                    {
                        trajectory.SimulateTrajectory(p, p.transform.position, p.direction);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isAiming = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                isAiming = true;

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0.0f;
                spawnPoint = mousePos;

                currentObject = Instantiate(prefabToSpawn, spawnPoint, Quaternion.identity);
                objectsToLaunch.Add(currentObject);
            }

            if (Input.GetMouseButtonDown(1))
            {
                foreach (var go in objectsToLaunch)
                {
                    var projectile = go.GetComponent<Projectile>();
                    if (projectile)
                    {
                        projectile.Init();
                    }

                    var line = go.GetComponent<LineRenderer>();
                    if (line)
                    {
                        Destroy(line);
                    }
                }

                objectsToLaunch = new List<GameObject>();
            }
        }

    }
}

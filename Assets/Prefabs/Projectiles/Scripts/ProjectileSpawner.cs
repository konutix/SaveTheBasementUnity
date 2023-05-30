using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public Projectile projectilePrefab;

    Vector3 spawnPoint;
    Vector3 spawnDir;

    bool isAiming = false;

    public ProjectileTrajectory trajectory;

    void Update()
    {
        if (!projectilePrefab) return;

        if (isAiming)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0.0f;

            spawnDir = Vector3.Normalize(spawnPoint - mousePos);

            trajectory.SimulateTrajectory(projectilePrefab, spawnPoint, spawnDir);
        }

        if (Input.GetMouseButtonDown(0))
        {
            isAiming = true;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0.0f;

            spawnPoint = mousePos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isAiming = false;

            var go = Instantiate(projectilePrefab, spawnPoint, Quaternion.identity);
            go.Init(spawnDir);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    public GameObject gfxMain;
    public GameObject gfxPlacing;

    public float lifeTime = 5.0f;

    [HideInInspector]
    public Vector3 direction;

    [HideInInspector]
    public bool isTriggerByDefault = false;

    [HideInInspector]
    public bool isMouseOver = false;

    private ProjectileSpawner projectileSpawner;

    [HideInInspector]
    public float pull = 1.0f;

    void Awake()
    {
        isTriggerByDefault = GetComponentInChildren<Collider2D>().isTrigger;
        projectileSpawner = FindObjectOfType<ProjectileSpawner>();
    }

    public void OnStartedPlacing()
    {
        gfxMain.SetActive(false);
        gfxPlacing.SetActive(true);

        var col = GetComponentInChildren<Collider2D>();
        if (col)
        {
            col.isTrigger = true;
        }
    }

    public void OnStoppedPlacing()
    {
        gfxPlacing.SetActive(false);
        gfxMain.SetActive(true);
        
        var col = GetComponentInChildren<Collider2D>();
        if (col)
        {
            col.isTrigger = isTriggerByDefault;
        }
    }

    public void OnLaunched()
    {
        var projectile = GetComponent<Projectile>();
        if (projectile)
        {
            projectile.Init(direction);
        }

        var line = GetComponent<LineRenderer>();
        if (line)
        {
            Destroy(line);
        }

        if (lifeTime > 0.0f)
        {
            Destroy(gameObject, lifeTime);
        }
    }

    public void OnAiming(Vector3 aimingPos)
    {
        direction = transform.position - aimingPos;
        CalculatePull(Vector3.Magnitude(direction));
        direction = direction.normalized;

        transform.eulerAngles = new Vector3(0, 0, Vector3.SignedAngle(Vector3.right, direction, Vector3.forward));        
    }

    public void OnAiming(float angle)
    {
        transform.eulerAngles = new Vector3(0, 0, angle);
        direction = transform.right;
    }

    private void OnMouseEnter() 
    {
        isMouseOver = true;
    }

    private void OnMouseExit() 
    {
        isMouseOver = false;
    }

    public void CalculateTrajectory()
    {
        var trajectoryManager = projectileSpawner.trajectoryManager;

        var projectile = GetComponent<Projectile>();
        if (projectile)
        {
            trajectoryManager.SimulateTrajectory(projectile, transform.position, direction, projectile.simulatedBouncesCount);
        }

        var shield = GetComponent<Shield>();
        if (shield)
        {
            trajectoryManager.SimulateShield(shield);

            foreach (var go in projectileSpawner.objectsToLaunch)
            {
                var p = go.GetComponent<Projectile>();
                if (p)
                {
                    trajectoryManager.SimulateTrajectory(p, go.transform.position, go.direction, p.simulatedBouncesCount);
                }
            }
        }
    }

    void CalculatePull(float distance)
    {
        float minPull = 0.4f;
        float minDistance = 0.5f;
        float maxDistance = 3.0f;

        float a = (minPull - 1.0f) / (minDistance - maxDistance);
        float c = 1 - maxDistance * a;

        pull = Mathf.Clamp(distance * a + c, minPull, 1.0f);
    }
}

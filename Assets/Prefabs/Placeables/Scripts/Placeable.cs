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

    [HideInInspector]
    public bool canBePickedUp = false;

    private ProjectileSpawner projectileSpawner;

    [HideInInspector]
    public float pull = 1.0f;

    float mouseDistanceThreshold = 0.1f;

    public BattleStats owner;

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
        float mag = Vector3.Magnitude(transform.position - aimingPos);
        if (mag < mouseDistanceThreshold) return;

        direction = Vector3.Normalize(transform.position - aimingPos);
        CalculatePull(mag);

        transform.eulerAngles = new Vector3(0, 0, Vector3.SignedAngle(Vector3.right, direction, Vector3.forward));        
    }

    public void OnAiming(float angle)
    {
        transform.eulerAngles = new Vector3(0, 0, angle);
        direction = transform.right;
    }

    private void OnMouseOver() 
    {
        if(!canBePickedUp) return;

        isMouseOver = true;

        var highlight = GetComponent<Highlight>();
        if (highlight)
        {
            highlight.SetHighlight(true);
        }
    }

    private void OnMouseExit() 
    {
        if(!canBePickedUp) return;

        isMouseOver = false;

        var highlight = GetComponent<Highlight>();
        if (highlight)
        {
            highlight.SetHighlight(false);
        }
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

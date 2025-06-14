using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    public GameObject gfxMain;
    public GameObject gfxPlacing;

    public float lifeTime = 5.0f;
    public bool canBeRotated = true;

    [HideInInspector]
    public Vector3 direction;

    [HideInInspector]
    public bool isTriggerByDefault = false;

    [HideInInspector]
    public bool isMouseOver = false;

    [HideInInspector]
    public bool canBePickedUp = false;

    [HideInInspector]
    public ProjectileSpawner projectileSpawner;

    [HideInInspector]
    public float pull = 1.0f;

    float mouseDistanceThreshold = 0.1f;

    public BattleStats owner;

    void Awake()
    {
        isTriggerByDefault = GetComponentInChildren<Collider2D>().isTrigger;
        projectileSpawner = FindObjectOfType<ProjectileSpawner>();
        direction = Vector3.right;
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

        if (TryGetComponent<LineRenderer>(out var lineRenderer))
        {
            var col1 = lineRenderer.startColor;
            var col2 = lineRenderer.endColor;
            col1.a = 1.0f;
            col2.a = 1.0f;
            
            lineRenderer.startColor = col1;
            lineRenderer.endColor = col2;
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

        if (TryGetComponent<LineRenderer>(out var lineRenderer))
        {
            var col1 = lineRenderer.startColor;
            var col2 = lineRenderer.endColor;
            col1.a = 0.3f;
            col2.a = 0.3f;
            
            lineRenderer.startColor = col1;
            lineRenderer.endColor = col2;
        }

        OnMouseExit();
    }

    public void OnLaunched()
    {
        canBePickedUp = false;

        if (TryGetComponent<Axe>(out var axe))
        {
            axe.Init();
        }

        if (TryGetComponent<Beam>(out var beam))
        {
            beam.Init();
        }

        var projectile = GetComponent<Projectile>();
        if (projectile)
        {
            projectile.Init(direction);
        }
        
        var buff = GetComponent<Buff>();
        if (buff)
        {
            buff.Init();
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

        if (TryGetComponent<Axe>(out var axe))
        {
            axe.SetFlip();
        }     
    }

    public void OnAiming(float angle)
    {
        transform.eulerAngles = new Vector3(0, 0, angle);
        direction = transform.right;

        if (TryGetComponent<Axe>(out var axe))
        {
            axe.SetFlip();
        }
    }

    public void OnRemove()
    {
        OnMouseExit();

        var shield = GetComponent<Shield>();
        if (shield)
        {
            shield.Remove();

            var trajectoryManager = projectileSpawner.trajectoryManager;
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

    private void OnMouseEnter()
    {
        if (!projectileSpawner.CanLaunch()) return;

        var description = GetComponent<ObjectDescription>();
        if (description != null)
        {
            description.Display(true);
        }

        if (TryGetComponent<LineRenderer>(out var lineRenderer))
        {
            var col1 = lineRenderer.startColor;
            var col2 = lineRenderer.endColor;
            col1.a = 1.0f;
            col2.a = 1.0f;
            
            lineRenderer.startColor = col1;
            lineRenderer.endColor = col2;
        }
    }

    private void OnMouseOver() 
    {
        if(!canBePickedUp || !projectileSpawner.CanLaunch()) return;

        isMouseOver = true;

        var highlight = GetComponent<Highlight>();
        if (highlight)
        {
            highlight.SetHighlightAndResize(true, 10);
        }
    }

    private void OnMouseExit() 
    {
        var description = GetComponent<ObjectDescription>();
        if (description != null)
        {
            description.Display(false);
        }

        if (!projectileSpawner.CanLaunch()) return;

        if (TryGetComponent<LineRenderer>(out var lineRenderer))
        {
            var col1 = lineRenderer.startColor;
            var col2 = lineRenderer.endColor;
            col1.a = 0.3f;
            col2.a = 0.3f;
            
            lineRenderer.startColor = col1;
            lineRenderer.endColor = col2;
        }
        
        if (!canBePickedUp) return;

        isMouseOver = false;

        var highlight = GetComponent<Highlight>();
        if (highlight)
        {
            highlight.SetHighlightAndResize(false, 10);
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

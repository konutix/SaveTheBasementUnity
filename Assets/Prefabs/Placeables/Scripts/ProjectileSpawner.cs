using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public Placeable prefabToSpawn;
    List<Placeable> objectsToLaunch;
    Placeable currentObject;

    Placeable ghost;

    Placeable tempCheckPrefabChange;


    bool isAiming = false;
    Vector3 mousePos = Vector3.zero;

    public TrajectoryManager trajectoryManager;
    public int simulatedBouncesCount = 3;

    public PlacingRange placingRange;

    void Start()
    {
        objectsToLaunch = new List<Placeable>();

        print("Left click & drag: spawn object");
        print("Right click: launch projectiles");
        print("To change object type, select prefab in Projectile Spawner");
    }

    void Update()
    {
        if (!prefabToSpawn) return;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;

        if (tempCheckPrefabChange != prefabToSpawn)
        {
            tempCheckPrefabChange = prefabToSpawn;
            if (ghost) Destroy(ghost.gameObject);

            ghost = Instantiate(prefabToSpawn);
            ghost.GetComponent<Collider2D>().enabled = false;
            ghost.OnStartedPlacing();
        }

        ghost.transform.position = mousePos;


        if (isAiming)
        {
            currentObject.OnAiming(mousePos);

            CalculateTrajectory();

            if (Input.GetMouseButtonUp(0))
            {
                OnStoppedPlacing();
            }
        }
        else
        {
            ghost.gameObject.SetActive(placingRange.isInRange);

            if (Input.GetMouseButtonDown(0) && placingRange.isInRange)
            {
                OnStartedPlacing();
            }

            if (Input.GetMouseButtonDown(1))
            {
                OnLaunched();
            }
        }
    }

    public void OnStartedPlacing()
    {
        isAiming = true;
        ghost.gameObject.SetActive(false);

        currentObject = Instantiate(prefabToSpawn, mousePos, Quaternion.identity);
        currentObject.OnStartedPlacing();
        objectsToLaunch.Add(currentObject);
    }
    
    public void OnStoppedPlacing()
    {
        isAiming = false;
        ghost.gameObject.SetActive(true);

        currentObject.OnStoppedPlacing();
    }

    public void OnLaunched()
    {
        foreach (var placeable in objectsToLaunch)
        {
            placeable.OnLaunched();
            Destroy(placeable.gameObject, 5.0f);
        }

        objectsToLaunch = new List<Placeable>();
        GetComponent<LineRenderer>().positionCount = 0;
    }

    void CalculateTrajectory()
    {
        var projectile = currentObject.GetComponent<Projectile>();
        if (projectile)
        {
            trajectoryManager.SimulateTrajectory(projectile, currentObject.transform.position, currentObject.direction, simulatedBouncesCount);
        }

        var shield = currentObject.GetComponent<Shield>();
        if (shield)
        {
            trajectoryManager.SimulateShield(shield);

            foreach (var go in objectsToLaunch)
            {
                var p = go.GetComponent<Projectile>();
                if (p)
                {
                    trajectoryManager.SimulateTrajectory(p, go.transform.position, go.direction, simulatedBouncesCount);
                }
            }
        }
    }
}

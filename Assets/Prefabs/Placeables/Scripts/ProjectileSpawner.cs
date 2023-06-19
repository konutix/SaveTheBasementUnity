using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public Placeable prefabToSpawn;
    Placeable currentObject;
    [HideInInspector]
    public List<Placeable> objectsToLaunch;

    Placeable ghost;

    Placeable tempCheckPrefabChange;


    bool isAiming  = false;
    bool isPlacing = false;
    Vector3 mousePos = Vector3.zero;

    public TrajectoryManager trajectoryManager;

    public PlacingRange placingRange;

    public BattleStats player;


    public event Action launchEvent;

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
            CreateGhost(prefabToSpawn);
        }

        ghost.transform.position = mousePos;


        if (isAiming)
        {
            currentObject.OnAiming(mousePos);
            currentObject.CalculateTrajectory();

            if (Input.GetMouseButtonUp(0))
            {
                OnStoppedPlacing();
            }
        }
        else
        {
            ghost.gameObject.SetActive(placingRange.isInRange && isPlacing);

            if (Input.GetMouseButtonDown(0))
            {
                if (placingRange.isInRange && isPlacing)
                {
                    OnStartedPlacing();
                }
                else
                {
                    TryPickupPlaced();
                }
            }
        }
    }

    public void OnStartedPlacing()
    {
        isPlacing = false;
        isAiming = true;
        ghost.gameObject.SetActive(false);

        if (!currentObject)
        {
            currentObject = Instantiate(prefabToSpawn, mousePos, Quaternion.identity);
            currentObject.owner = player;
        } 
        else
        {
            currentObject.transform.position = mousePos;
        }
        
        currentObject.canBePickedUp = true;
        currentObject.OnStartedPlacing();
        objectsToLaunch.Add(currentObject);
    }
    
    public void OnStoppedPlacing()
    {
        isAiming = false;
        ghost.gameObject.SetActive(true);

        currentObject.OnStoppedPlacing();
        currentObject = null;
    }

    public void OnLaunched()
    {
        foreach (var placeable in objectsToLaunch)
        {
            placeable.OnLaunched();
        }

        objectsToLaunch = new List<Placeable>();
        GetComponent<LineRenderer>().positionCount = 0;

        if (launchEvent != null) launchEvent();
    }

    public void AddPlaceable(Placeable placeable)
    {
        objectsToLaunch.Add(placeable);
    }

    void TryPickupPlaced()
    {
        foreach (var placeable in objectsToLaunch)
        {
            if (placeable.isMouseOver)
            {
                isPlacing = true;
                
                objectsToLaunch.Remove(placeable);
                
                currentObject = placeable;
                currentObject.transform.position = new Vector3(0.0f, 9999.9f, 0.0f);
                currentObject.CalculateTrajectory();

                CreateGhost(placeable);

                return;
            }
        }
    }

    void CreateGhost(Placeable prefab)
    {
        if (ghost) Destroy(ghost.gameObject);

        ghost = Instantiate(prefab);
        ghost.GetComponentInChildren<Collider2D>().enabled = false;
        ghost.OnStartedPlacing();
    }

    public bool CanLaunch()
    {
        return (!isAiming && !isPlacing);
    }

    public void StartPlacing(Placeable placeable)
    {
        prefabToSpawn = placeable;
        isPlacing = true;
        CreateGhost(prefabToSpawn);
    }
}

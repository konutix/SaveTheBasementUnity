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


    [HideInInspector] public bool isAiming  = false;
    [HideInInspector] public bool isPlacing = false;
    Vector3 mousePos = Vector3.zero;

    public TrajectoryManager trajectoryManager;

    public PlacingRange placingRange;

    public BattleStats player;

    CardPanelScript cardPanel;

    public event Action launchEvent;
    public event Action simulationStopEvent;
    
    bool simulationEndedEarly = false;

    void Start()
    {
        objectsToLaunch = new List<Placeable>();
        cardPanel = FindObjectOfType<CardPanelScript>();
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
        ghost.gameObject.SetActive(placingRange.isInRange && isPlacing);

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

            if (Input.GetMouseButtonDown(1))
            {
                TryCancelPlaced();
            }
        }

        if (!simulationEndedEarly && cardPanel.simulationTimer > 0.0f)
        {
            CheckEarlySimulationEnd();
        }
        else if (cardPanel.simulationTimer <= 0.0f)
        {
            simulationEndedEarly = false;
        }
    }

    public void StartPlacing(Placeable placeable)
    {
        prefabToSpawn = placeable;
        isPlacing = true;
        CreateGhost(prefabToSpawn);
    }

    public void StartSimulation()
    {
        cardPanel.StartSimulation();
    }

    public void OnStartedPlacing()
    {
        isPlacing = false;
        isAiming = true;

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

        if (cardPanel) cardPanel.AssignInstanceToCurrentCard(currentObject);

        currentObject.OnStoppedPlacing();
        currentObject = null;
    }
    
    public void OnCancelPlacing()
    {
        isPlacing = false;
        isAiming = false;

        if (currentObject)
        {
            objectsToLaunch.Remove(currentObject);
            Destroy(currentObject.gameObject);
            currentObject = null;
        }
    }

    public void OnLaunched()
    {
        foreach (var placeable in objectsToLaunch)
        {
            placeable.OnLaunched();
        }

        //objectsToLaunch = new List<Placeable>();
        GetComponent<LineRenderer>().positionCount = 0;

        if (launchEvent != null) launchEvent();
    }

    public bool CanLaunch()
    {
        return (!isAiming && !isPlacing);
    }

    public void AddPlaceable(Placeable placeable)
    {
        objectsToLaunch.Add(placeable);
    }

    public void RemovePlaceable(Placeable placeable)
    {
        objectsToLaunch.Remove(placeable);
        Destroy(placeable.gameObject);
    }
    
    void TryPickupPlaced()
    {
        if (!CanLaunch()) return;

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

                cardPanel.PickupPlayedCard(placeable);

                return;
            }
        }
    }

    void TryCancelPlaced()
    {
        if (isPlacing)
        {
            if (currentObject)
            {
                if (cardPanel != null) cardPanel.CancelPlayedCard(currentObject);
                
                objectsToLaunch.Remove(currentObject);
                Destroy(currentObject.gameObject);
                
                currentObject = null;

                isPlacing = false;
            }

            return;
        }

        foreach (var placeable in objectsToLaunch)
        {
            if (placeable.isMouseOver)
            {
                if (cardPanel != null) cardPanel.CancelPlayedCard(placeable);
                
                objectsToLaunch.Remove(placeable);
                Destroy(placeable.gameObject);
                
                currentObject = null;

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

    void CheckEarlySimulationEnd()
    {
        foreach (var placeable in objectsToLaunch)
        {
            if (placeable && placeable.GetComponent<Projectile>())
            {
                // print("im still standing");
                return;
            }
        }

        simulationEndedEarly = true;

        objectsToLaunch = new List<Placeable>();

        cardPanel.simulationTimer = 0.2f;
        if (simulationStopEvent != null) simulationStopEvent();
    }
}

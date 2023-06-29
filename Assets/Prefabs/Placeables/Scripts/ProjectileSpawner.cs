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
    Dictionary<Placeable, Arm> placeablesArms;

    Placeable ghost;

    Placeable tempCheckPrefabChange;


    [HideInInspector] public bool isAiming  = false;
    [HideInInspector] public bool isPlacing = false;
    Vector3 mousePos = Vector3.zero;
    Vector3 placingPos = Vector3.zero;

    public TrajectoryManager trajectoryManager;

    public PlacingRange placingRange;

    public BattleStats player;

    CardPanelScript cardPanel;

    [Space]
    public Arm armPrefab;
    Arm currentArm = null;

    public event Action launchEvent;
    public event Action simulationStopEvent;
    
    bool simulationEndedEarly = false;

    void Start()
    {
        objectsToLaunch = new List<Placeable>();
        placeablesArms = new Dictionary<Placeable, Arm>();
        cardPanel = FindObjectOfType<CardPanelScript>();

        player.SetupProperties((int)RunState.currentPlayerHealth, (int)RunState.maxPlayerHealth);
    }

    void Update()
    {
        if (!prefabToSpawn) return;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;

        if (currentArm != null)
        {
            if (isPlacing) currentArm.DoTheThing(mousePos);

            placingPos = currentArm.currentHookLocation;
        }
        else
        {
            placingPos = mousePos;
        }


        // sort arms for rendering
        float offset = 0.05f;
        foreach(var arm in placeablesArms.Values)
        {
            Vector3 pos = arm.transform.position;

            if (arm == currentArm)
            {
                pos.z = 0.0f;
            }
            else
            {
                pos.z = offset;
                offset += 0.05f;
            }

            arm.transform.position = pos;
        }
        

        if (tempCheckPrefabChange != prefabToSpawn)
        {
            tempCheckPrefabChange = prefabToSpawn;
            CreateGhost(prefabToSpawn);
        }

        ghost.transform.position = placingPos;
        ghost.gameObject.SetActive(/*placingRange.isInRange &&*/ isPlacing);

        if (isAiming)
        {
            currentObject.OnAiming(mousePos);
            currentObject.CalculateTrajectory();

            if (Input.GetMouseButtonUp(0))
            {
                OnStoppedPlacing();
            }
        }
        else if (cardPanel.panelState != PanelState.dragging)
        {
            // if (placingRange.isInRange && isPlacing && arm != null) arm.DoTheThing(mousePos);
            
            if (Input.GetMouseButtonDown(0))
            {
                if (/*placingRange.isInRange &&*/ isPlacing)
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

        currentArm = Instantiate(armPrefab, player.transform.position, Quaternion.identity);
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
            currentObject = Instantiate(prefabToSpawn, placingPos, Quaternion.identity);
            currentObject.owner = player;
            placeablesArms.Add(currentObject, currentArm);
        } 
        else
        {
            currentObject.transform.position = placingPos;
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
            if (placeablesArms.TryGetValue(currentObject, out currentArm))
            {
                placeablesArms.Remove(currentObject);
                currentArm.OnRemove();
                currentArm = null;
            }

            objectsToLaunch.Remove(currentObject);
            Destroy(currentObject.gameObject);
            currentObject = null;
        }
        else if (currentArm)
        {
            currentArm.OnRemove();
            currentArm = null;
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

        foreach(var arm in placeablesArms.Values)
        {
            arm.OnRemove();
        }
        placeablesArms = new Dictionary<Placeable, Arm>();
    }

    public bool CanLaunch()
    {
        return !isAiming && !isPlacing && (cardPanel.panelState != PanelState.dragging);
    }

    public void AddPlaceable(Placeable placeable)
    {
        objectsToLaunch.Add(placeable);
    }

    public void RemovePlaceable(Placeable placeable)
    {
        objectsToLaunch.Remove(placeable);
        Destroy(placeable.gameObject);

        if (placeablesArms.TryGetValue(placeable, out currentArm))
        {
            placeablesArms.Remove(placeable);
            currentArm.OnRemove();
            currentArm = null;
        }
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

                placeablesArms.TryGetValue(currentObject, out currentArm);

                return;
            }
        }
    }

    void TryCancelPlaced()
    {
        // cancel unplaced
        if (isPlacing)
        {
            if (currentObject)
            {
                if (placeablesArms.TryGetValue(currentObject, out currentArm))
                {
                    placeablesArms.Remove(currentObject);
                    currentArm.OnRemove();
                    currentArm = null;
                }

                if (cardPanel != null) cardPanel.CancelPlayedCard(currentObject);

                currentObject.OnRemove();
                objectsToLaunch.Remove(currentObject);
                Destroy(currentObject.gameObject);
                
                currentObject = null;

                isPlacing = false;
            }

            return;
        }

        // cancel already placed
        foreach (var placeable in objectsToLaunch)
        {
            if (placeable.isMouseOver)
            {
                if (placeablesArms.TryGetValue(placeable, out currentArm))
                {
                    placeablesArms.Remove(placeable);
                    currentArm.OnRemove();
                    currentArm = null;
                }

                if (cardPanel != null) cardPanel.CancelPlayedCard(placeable);
                
                placeable.OnRemove();
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
                if (placeable.GetComponent<Zap>()) continue;
                var rb = placeable.GetComponent<Rigidbody2D>();
                if (!rb || rb.simulated)
                {
                    // print("im still standing");
                    return;
                }
            }
        }

        simulationEndedEarly = true;

        foreach (var placeable in objectsToLaunch)
        {
            if (placeable && !placeable.GetComponent<Zap>())
            {
                Destroy(placeable.gameObject);
            }
        }
        objectsToLaunch = new List<Placeable>();

        cardPanel.simulationTimer = 0.2f;
        if (simulationStopEvent != null) simulationStopEvent();
    }
}

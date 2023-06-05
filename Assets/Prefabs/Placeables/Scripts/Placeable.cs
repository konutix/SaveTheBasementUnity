using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    public GameObject gfxMain;
    public GameObject gfxPlacing;

    [HideInInspector]
    public Vector3 direction;

    [HideInInspector]
    public bool isTriggerByDefault = false;

    void Awake()
    {
        isTriggerByDefault = GetComponent<Collider2D>().isTrigger;
    }

    public void OnStartedPlacing()
    {
        gfxMain.SetActive(false);
        gfxPlacing.SetActive(true);

        var col = GetComponent<Collider2D>();
        if (col)
        {
            col.isTrigger = true;
        }
    }

    public void OnStoppedPlacing()
    {
        gfxPlacing.SetActive(false);
        gfxMain.SetActive(true);
        
        var col = GetComponent<Collider2D>();
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
    }

    public void OnAiming(Vector3 aimingPos)
    {
        direction = Vector3.Normalize(transform.position - aimingPos);
        transform.eulerAngles = new Vector3(0, 0, Vector3.SignedAngle(Vector3.right, direction, Vector3.forward));
    }
}

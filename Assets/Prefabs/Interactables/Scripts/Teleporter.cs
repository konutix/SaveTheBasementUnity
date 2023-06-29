using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour, Interactable
{
    public Transform teleportPoint;
    public ParticleSystem particles;

    private static List<Teleporter> AllTeleporters;
    
    private void Start()
    {
        if (AllTeleporters == null)
        {
            AllTeleporters = new List<Teleporter>();
        }

        AllTeleporters.Add(this);
    }

    public void OnInteractWithProjectile(Projectile projectile)
    {
        if (projectile.TryGetComponent<Placeable>(out var placeable) && placeable.owner != null && placeable.owner.canBeTeleported)
        {
            placeable.owner.transform.position = teleportPoint.position;
            FindObjectOfType<TrajectoryManager>().UpdateObject(placeable.owner.gameObject);
            particles.Play();

            foreach (var tp in AllTeleporters)
            {
                if (tp != null)
                {
                    tp.GetComponent<Collider2D>().enabled = true;
                }
            }
            GetComponent<Collider2D>().enabled = false;
        }

        Destroy(projectile.gameObject);        
    }

    private void OnMouseEnter()
    {
        var description = GetComponent<ObjectDescription>();
        if (description != null)
        {
            description.Display(true);
        }
    }

    private void OnMouseExit() 
    {
        var description = GetComponent<ObjectDescription>();
        if (description != null)
        {
            description.Display(false);
        }
    }
}

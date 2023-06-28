using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour, Interactable
{
    public Transform teleportPoint;

    public void OnInteractWithProjectile(Projectile projectile)
    {
        if (projectile.TryGetComponent<Placeable>(out var placeable) && placeable.owner != null && placeable.owner.canBeTeleported)
        {
            placeable.owner.transform.position = teleportPoint.position;
            FindObjectOfType<TrajectoryManager>().UpdateObject(placeable.owner.gameObject);
        }

        Destroy(projectile.gameObject);        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour, Interactable
{
    public int evnironmentalHealth = 2;

    public void OnInteractWithProjectile(Projectile projectile)
    {
        if (evnironmentalHealth == 0) return; 

        evnironmentalHealth -= projectile.evnironmentalDamage;
        evnironmentalHealth = Mathf.Clamp(evnironmentalHealth, 0, 1230213);

        var renderer = GetComponent<SpriteRenderer>();
        renderer.color = Color.Lerp(renderer.color, Color.black, 1.0f / (evnironmentalHealth+1));

        if (evnironmentalHealth == 0)
        {
            var col = GetComponent<Collider2D>();
            col.enabled = false;

            var trajectory = FindObjectOfType<TrajectoryManager>();
            if (trajectory)
            {
                trajectory.RemoveObject(gameObject);
            }

            var cameraShake = FindObjectOfType<CameraShake>();
            if (cameraShake)
            {
                cameraShake.shakeDuration = 0.2f;
            }
        }
    }
}

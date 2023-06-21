using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, Interactable
{
    public bool shouldDestroyProjectile = false;
    public int evnironmentalHealth = 1;

    private void OnDestroy()
    {
        var trajectory = FindObjectOfType<TrajectoryManager>();
        if (trajectory)
        {
            trajectory.RemoveObject(gameObject);
        }
    }

    public void OnInteractWithProjectile(Projectile projectile)
    {
        int damage = projectile.evnironmentalDamage;
        if (shouldDestroyProjectile)
        {
            Destroy(projectile.gameObject);
        }

        if (evnironmentalHealth == 0 || damage < 1) return;

        evnironmentalHealth -= damage;
        evnironmentalHealth = Mathf.Clamp(evnironmentalHealth, 0, 1230213);

        var renderer = GetComponent<Placeable>().gfxMain.GetComponent<SpriteRenderer>();
        renderer.color = Color.Lerp(renderer.color, Color.black, 1.0f / (evnironmentalHealth+1));

        if (evnironmentalHealth == 0)
        {
            var cameraShake = FindObjectOfType<CameraShake>();
            if (cameraShake)
            {
                cameraShake.shakeDuration = 0.2f;
            }

            Destroy(gameObject);
        }
    }
}

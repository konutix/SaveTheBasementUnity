using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GetComponent<Projectile>().isSimulatingTrajectory) return;
        
        print(gameObject.name + " passing over " + other.gameObject.name);
    }
}

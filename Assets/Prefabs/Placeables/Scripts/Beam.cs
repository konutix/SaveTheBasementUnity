using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public int weakAmount = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GetComponent<Projectile>().isSimulatingTrajectory) return;
        
        
        var stats = other.GetComponent<BattleStats>();
        if (stats)
        {
            print(gameObject.name + " weakening " + other.gameObject.name);
            stats.weak += weakAmount;
        }
    }
}

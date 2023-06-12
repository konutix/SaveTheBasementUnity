using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float launchForce = 10.0f;
    public bool usePullModifier = false;

    [Space]
    public int damage = 5;
    public int evnironmentalDamage = 1;

    
    [HideInInspector] public bool isSimulatingTrajectory = false;
    [Space]
    public int simulatedBouncesCount = 0;
    [HideInInspector] public int bouncesCount = 0;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.simulated = false;
        rb.isKinematic = true;
        //rb.Sleep(); // should be set to Start Asleep in inspector
    }

    public void Init(Vector3 direction)
    {
        //rb.simulated = true;
        rb.isKinematic = false;
        rb.WakeUp();
        rb.AddForce(direction * launchForce * (usePullModifier ? GetComponent<Placeable>().pull : 1.0f), ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bouncesCount += 1;

        if (isSimulatingTrajectory)
        {
            return;
        }

        try {
            collision.gameObject.GetComponent<Interactable>().OnInteractWithProjectile(this);
        } catch { }
    }
}

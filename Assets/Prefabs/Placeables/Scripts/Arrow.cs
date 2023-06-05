using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform handle;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        if (rb.simulated)
        {
            handle.transform.eulerAngles = new Vector3(0, 0, Vector3.SignedAngle(Vector3.right, rb.velocity.normalized, Vector3.forward));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            GetComponent<Projectile>().bouncesCount = 9999;
            rb.simulated = false;
        }
    }
}

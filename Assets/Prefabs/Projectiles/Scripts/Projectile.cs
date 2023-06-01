using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10.0f;
    [HideInInspector] public Vector3 direction = Vector3.right;
    [HideInInspector] public int bouncesCount = 0;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // rb.Sleep(); should be set to Start Asleep in inspector
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void Init(Vector3 direction)
    {
        this.direction = direction;
        Init();
    }

    public void Init()
    {
        rb.WakeUp();
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bouncesCount += 1;
    }
}

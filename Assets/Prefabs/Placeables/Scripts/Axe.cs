using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public int bladeDamage = 10;
    public int handleDamage = 3;

    [Space]
    public float rotationSpeed = 13.14f;
    float flip = 1.0f;

    [Space]
    public Transform handle;
    Rigidbody2D rb;

    public Collider2D mainCollider;
    public Collider2D bladeCollider;
    public Collider2D handleCollider;

    public void Init()
    {
        mainCollider.enabled = false;
        bladeCollider.enabled = true;
        handleCollider.enabled = true;
    }

    public void SetFlip()
    {
        flip = (Vector3.Dot(GetComponent<Placeable>().direction, Vector3.right) > 0.0f) ? 1.0f : -1.0f;
        handle.localScale = new Vector3(1.0f,  flip, 1.0f);
    }

    public int GetDamage(Collision2D col)
    {
        return (col.otherCollider == bladeCollider) ? bladeDamage : handleDamage;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        if (rb.simulated && !rb.isKinematic)
        {
            handle.transform.eulerAngles = new Vector3(0, 0, handle.transform.eulerAngles.z - Time.deltaTime * rotationSpeed * flip);
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

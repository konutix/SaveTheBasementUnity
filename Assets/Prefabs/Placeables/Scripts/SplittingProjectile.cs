using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplittingProjectile : MonoBehaviour
{
    public Projectile childPrefab;
    public float angle = 15.0f;
    public float childLifeTime = 3.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetComponent<Projectile>().isSimulatingTrajectory) return;

        Vector2 velocity = GetComponent<Rigidbody2D>().velocity.normalized;
        Vector2 normal = collision.GetContact(0).normal;

        Vector2 ref0 = Vector2.Reflect(velocity, normal);
        Vector2 ref1 = Quaternion.AngleAxis(-angle, Vector3.forward) * ref0;
        Vector2 ref2 = Quaternion.AngleAxis( angle, Vector3.forward) * ref0;

        var p0 = Instantiate(childPrefab, transform.position, Quaternion.identity);
        var p1 = Instantiate(childPrefab, transform.position, Quaternion.identity);
        var p2 = Instantiate(childPrefab, transform.position, Quaternion.identity);

        var col0 = p0.GetComponent<Collider2D>();
        var col1 = p1.GetComponent<Collider2D>();
        var col2 = p2.GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(col0, col1);
        Physics2D.IgnoreCollision(col0, col2);
        Physics2D.IgnoreCollision(col1, col2);

        p0.Init(ref0);
        p1.Init(ref1);
        p2.Init(ref2);

        Destroy(p0.gameObject, childLifeTime);
        Destroy(p1.gameObject, childLifeTime);
        Destroy(p2.gameObject, childLifeTime);

        Destroy(gameObject);
    }
}

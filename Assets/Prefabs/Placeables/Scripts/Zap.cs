using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zap : MonoBehaviour
{
    public int damage = 5;

    public LayerMask objectsToDamage;

    static List<Zap> AllZaps;

    void Start()
    {
        if (AllZaps == null)
        {
            AllZaps = new List<Zap>();
        }
    }

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ZappityZap()
    {
        if (AllZaps.Count > 0)
        {
            foreach (var zap in AllZaps)
            {
                if (!zap) continue;
                // Debug.DrawLine(transform.position, zap.transform.position, Color.yellow, 1.0f);
                StartCoroutine(zap.AnimateLine(transform.position));

                Vector2 dir = zap.transform.position - transform.position;
                foreach (var hit in Physics2D.RaycastAll(transform.position, dir, dir.magnitude, objectsToDamage))
                {
                    if (hit.transform.TryGetComponent<BattleStats>(out var health))
                    {
                        health.TakeDamage(damage);
                    }
                }
            }
        }

        AllZaps.Add(this);
    }

    public LineRenderer line;

    IEnumerator AnimateLine(Vector3 source)
    {
        line.positionCount = 2;
        line.SetPosition(0, source);
        line.SetPosition(1, source);

        Vector3 dir = transform.position - source;
        dir = dir.normalized;

        float speed = 33.0f;

        float currentMag = 1000.0f;
        float prevMag = 1001.0f;

        while (currentMag > 0.1f && prevMag > currentMag)
        {
            prevMag = currentMag;

            line.SetPosition(1, line.GetPosition(1) + dir * Time.deltaTime * speed);

            currentMag = Vector3.Magnitude(transform.position - line.GetPosition(1));

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        line.positionCount = 0;
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

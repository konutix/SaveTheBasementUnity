using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAnimator : MonoBehaviour
{
    public float lifeTime = 0.5f;
    float t = 0.0f;

    SpriteRenderer renderer;
    Color finalColor;
    Color initialColor;
    
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

        initialColor = renderer.color;
        Color tmp = initialColor;
        tmp.a = 0.0f;
        finalColor = tmp;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        t += Time.deltaTime / lifeTime;

        renderer.color = Color.Lerp(initialColor, finalColor, t);

        transform.localScale = new Vector3(1.0f, 0.6f + t * 0.5f, 1.0f);
    }
}

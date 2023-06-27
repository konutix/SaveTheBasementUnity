using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public BeamAnimator singleBeamPrefab;
    float speed;
    Vector3 dir;

    Vector3 initPosition;
    Quaternion initRotation;

    public void Init()
    {
        var go = GetComponent<Placeable>().gfxMain;
        go.SetActive(false);
        initPosition = go.transform.position;
        initRotation = go.transform.rotation;

        speed = GetComponent<Projectile>().launchForce;
        dir = GetComponent<Placeable>().direction;
        StartCoroutine(BeamAnimation());
    }

    IEnumerator BeamAnimation()
    {
        float distancePerPrefab = 0.5f;
        float currentDistance = 0.0f;
        float totalDistance = 0.0f;

        float time = GetComponent<Placeable>().lifeTime;
        while (time > 0.0f)
        {
            time -= Time.deltaTime;

            currentDistance += Time.deltaTime * speed;

            if (currentDistance >= distancePerPrefab)
            {
                totalDistance += currentDistance;
                currentDistance = 0.0f;

                Instantiate(singleBeamPrefab, initPosition + totalDistance * dir, initRotation);
            }

            yield return null;
        }
    }
}

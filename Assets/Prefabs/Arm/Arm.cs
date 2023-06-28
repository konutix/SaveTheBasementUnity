using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    public Transform anchor;
    public Transform head;

    public float anchorLength = 1.8f;
    public float headLength = 1.2f;

    [Space]
    public LayerMask ignoreCollision;

    [HideInInspector] public Vector3 currentHookLocation = Vector3.zero;

    Vector3 targetPos;
    float invert = 1.0f;
    
    float smoothFactor = 974.3f;

    Quaternion targetAnchorRotation = Quaternion.identity;

    void Start()
    {
        DoTheThing(anchor.position + new Vector3(2.0f, 1.0f, 0.0f));
    }

    void Update()
    {
        anchor.rotation = Quaternion.RotateTowards(anchor.rotation, targetAnchorRotation, smoothFactor * Time.deltaTime);

        Vector3 target = anchor.position + anchor.right * anchorLength;
        head.position = target;
        Vector3 relative = targetPos - target;
        head.eulerAngles = new Vector3(0.0f, 0.0f, Vector3.SignedAngle(Vector3.right, relative, Vector3.forward));

        currentHookLocation = head.position + head.right * headLength;


        if (targetPos.x < transform.position.x) invert = -1.0f;
        else invert = 1.0f;
    }

    public void DoTheThing(Vector3 mouse)
    {
        Vector3 dir = mouse - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dir.magnitude, ~ignoreCollision);
        if (hit.collider != null)
        {
            print(hit.collider.gameObject);
            targetPos = hit.point;
        }
        else
        {
            targetPos = mouse;
        }

        ElbowPos(anchorLength, headLength, targetPos - transform.position);
    }

    void ElbowPos(float l1, float l2, Vector3 target)
    {
        float num = l1*l1 + target.x*target.x + target.y*target.y - l2*l2;
        float den = 2*l1 * Mathf.Sqrt(target.x*target.x + target.y*target.y);
        float angle = Mathf.Acos(num / den);

        if (angle != angle) angle = 0.0f;

        float newAngle = invert * Mathf.Rad2Deg * angle + Vector3.SignedAngle(Vector3.right, target, Vector3.forward);
        targetAnchorRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, newAngle));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    public Transform anchor;
    public Transform head;

    Vector3 targetPos;
    float invert = 1.0f;

    
    Vector3 desiredAnchorAngle = Vector3.zero;
    Vector3 desiredHeadAngle = Vector3.zero;
    Vector3 desiredHeadPos = Vector3.zero;
    float desiiredAnglle = 0.0f;

    float smoothFactor = 974.3f;

    Quaternion targetAnchorRotation = Quaternion.identity;

    void Start()
    {
        DoTheThing(anchor.position + new Vector3(2.0f, 1.0f, 0.0f));
    }

    void Update()
    {
        // anchor.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(anchor.eulerAngles.z, desiiredAnglle, smoothFactor*Time.deltaTime));
        // anchor.eulerAngles = Vector3.Lerp(anchor.eulerAngles, desiredAnchorAngle, smoothFactor*Time.deltaTime);
        // head.eulerAngles = Vector3.Lerp(head.eulerAngles, desiredHeadAngle, smoothFactor*Time.deltaTime);
        // head.position = Vector3.Lerp(head.position, desiredHeadPos, smoothFactor*Time.deltaTime);

        anchor.rotation = Quaternion.RotateTowards(anchor.rotation, targetAnchorRotation, smoothFactor * Time.deltaTime);

        Vector3 target = anchor.position + anchor.right * 1.8f;
        head.position = target;
        Vector3 relative = targetPos - target;
        // desiredHeadAngle = new Vector3(0.0f, 0.0f, Vector3.SignedAngle(Vector3.right, relative, Vector3.forward));
        head.eulerAngles = new Vector3(0.0f, 0.0f, Vector3.SignedAngle(Vector3.right, relative, Vector3.forward));
        if (targetPos.x < transform.position.x) invert = -1.0f;
        else invert = 1.0f;

        

        
        // Follow(anchor, targetPos);
        // Follow(head, anchor.position);
        //FixedFollow(head, anchor.position, transform.position);
    }

    public void DoTheThing(Vector3 mouse)
    {
        targetPos = mouse;
        targetPos.z = 0.0f;

        ElbowPos(1.8f, 1.2f, targetPos - transform.position);

        

        // Vector3 target = anchor.position + anchor.right * 2.0f;
        // // desiredHeadPos = target;
        // head.position = target;
        // Vector3 relative = targetPos - target;
        // // desiredHeadAngle = new Vector3(0.0f, 0.0f, Vector3.SignedAngle(Vector3.right, relative, Vector3.forward));
        // head.eulerAngles = new Vector3(0.0f, 0.0f, Vector3.SignedAngle(Vector3.right, relative, Vector3.forward));
    }

    void Follow(Transform t, Vector3 target)
    {
        Vector3 relative = target - t.position;
        t.eulerAngles = new Vector3(0.0f, 0.0f, Vector3.SignedAngle(Vector3.right, relative, Vector3.forward));
        t.position = t.position + t.right * (relative.magnitude - 2.0f);
    }

    void FixedFollow(Transform t, Vector3 target, Vector3 fixedPosition)
    {
        t.position = fixedPosition;
        Vector3 relative = target - t.position;
        t.eulerAngles = new Vector3(0.0f, 0.0f, Vector3.SignedAngle(Vector3.right, relative, Vector3.forward));        
    }

    void ElbowPos(float l1, float l2, Vector3 target)
    {
        float num = l1*l1 + target.x*target.x + target.y*target.y - l2*l2;
        float den = 2*l1 * Mathf.Sqrt(target.x*target.x + target.y*target.y);
        float angle = Mathf.Acos(num / den);

        if (angle != angle) angle = 0.0f;

        float newAngle = invert * Mathf.Rad2Deg * angle + Vector3.SignedAngle(Vector3.right, target, Vector3.forward);
        targetAnchorRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, newAngle));
        // desiiredAnglle = newAngle;
        // anchor.eulerAngles = new Vector3(0.0f, 0.0f, newAngle);
        // desiredAnchorAngle = new Vector3(0.0f, 0.0f, newAngle);
    }
}

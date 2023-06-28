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

    [Space]
    public Transform hook;
    [HideInInspector] public Vector3 currentHookLocation = Vector3.zero;

    Vector3 targetPos;
    float invert = 1.0f;
    
    float smoothFactor = 974.3f;

    Quaternion targetAnchorRotation = Quaternion.identity;
    bool isFinishRemoveAnimation = false;

    void Start()
    {
        DoTheThing(anchor.position + new Vector3(2.0f, 1.0f, 0.0f));
        StartCoroutine(IdleAnimation());
        StartCoroutine(HookAnimation());
    }

    void Update()
    {
        anchor.rotation = Quaternion.RotateTowards(anchor.rotation, targetAnchorRotation, smoothFactor * Time.deltaTime);

        Vector3 target = anchor.position + anchor.right * anchorLength;
        head.position = target;

        if (isFinishRemoveAnimation)
        {
            head.rotation = Quaternion.RotateTowards(head.rotation, Quaternion.Euler(new Vector3(0.0f, 0.0f, -90.0f)), smoothFactor * Time.deltaTime);

            anchor.position = Vector3.MoveTowards(anchor.position, targetPos, 0.01f * smoothFactor * Time.deltaTime);
            return;
        }
        
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
            targetPos = hit.point;
        }
        else
        {
            targetPos = mouse;
        }

        smoothFactor = 974.3f;
        ElbowPos(anchorLength, headLength, targetPos - transform.position);
    }

    public void OnRemove()
    {
        StartCoroutine(RemoveAnimation());
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

    IEnumerator IdleAnimation()
    {
        float a = 1.7f;
        float test = 1.0f;
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            
            smoothFactor = 0.4f;
            targetAnchorRotation *= Quaternion.AngleAxis(a * test, Vector3.forward);

            yield return new WaitForSeconds(3.0f);
            
            test = 2.0f;
            a *= -1.0f;
            smoothFactor = 0.4f;
            targetAnchorRotation *= Quaternion.AngleAxis(a * test, Vector3.forward);
            a *= -1.0f;
        }
    }
    
    IEnumerator HookAnimation()
    {
        float a = 0.0f;
        float timer = 0.0f;
        float speed = 0.5f;
        while (true)
        {
            hook.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, a));

            yield return null;

            timer += Time.deltaTime;
            if (timer >= 3.0f)
            {
                timer = 0.0f;
                speed *= -1.0f;
            }

            a += speed * Time.deltaTime;
        }
    }

    IEnumerator RemoveAnimation()
    {
        StopCoroutine(IdleAnimation());

        Vector3 point = Vector3.up * 5.0f;
        // targetPos = transform.position + point;
        ElbowPos(anchorLength, headLength, point);
        smoothFactor = 300.0f;

        yield return new WaitForSeconds(0.15f);
        isFinishRemoveAnimation = true;
        targetPos = transform.position;

        yield return new WaitForSeconds(0.2f);
        targetPos = transform.position + Vector3.down * 1.0f;

        yield return new WaitForSeconds(0.1f);
        hook.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        Destroy(gameObject);
    }
}

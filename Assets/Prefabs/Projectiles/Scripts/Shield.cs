using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [HideInInspector]
    public Vector3 direction = Vector3.right;

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
        transform.eulerAngles = new Vector3(0, 0, Vector3.SignedAngle(Vector3.right, direction, Vector3.forward));
    }
}

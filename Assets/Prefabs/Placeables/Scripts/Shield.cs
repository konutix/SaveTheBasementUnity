using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnDestroy()
    {
        var trajectory = FindObjectOfType<TrajectoryManager>();
        if (trajectory)
        {
            trajectory.RemoveObject(gameObject);
        }
    }
}

using UnityEngine;
using System.Collections;

public class RaycastExample : MonoBehaviour {

    void Update () {
        int layerMask = 7;
        layerMask = ~layerMask;

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log ("Name = " + hit.collider.name);
                Debug.Log ("Tag = " + hit.collider.tag);
                Debug.Log ("Hit Point = " + hit.point);
                Debug.Log ("Object position = " + hit.collider.gameObject.transform.position);
                Debug.Log ("--------------");
            }
        }
    }
}
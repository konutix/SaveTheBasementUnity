using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Cursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos =
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);

        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }
}

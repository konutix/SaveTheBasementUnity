using UnityEngine;
using System.Collections;

/// <summary>
/// This script should be attached to the card game object to display card`s rotation correctly.
/// </summary>

[ExecuteInEditMode]
public class BetterCardRotation : MonoBehaviour
{

    // parent game object for all the card face graphics
    public RectTransform CardFront;

    // parent game object for all the card back graphics
    public RectTransform CardBack;

    // an empty game object that is placed a bit above the face of the card, in the center of the card
    public Transform targetFacePoint;

    // type of camera
    public bool ortoCamera = true;

    // if this is true, our players currently see the card Back
    private bool showingBack = false;

    // Update is called once per frame
    void Update()
    {
        bool lookingAtBack = false;

        if (ortoCamera)
        {
            Vector3 cardNormal = targetFacePoint.transform.position - transform.position;
            lookingAtBack = (Vector3.Dot(cardNormal, new Vector3(0, 0, -1.0f)) >= 0.0f);
        }
        else
        {
            Vector3 cardNormal = targetFacePoint.transform.position - transform.position;
            Vector3 camVec = transform.position - Camera.main.transform.position;
            lookingAtBack = (Vector3.Dot(cardNormal, camVec) >= 0.0f);
        }

        if (lookingAtBack != showingBack)
        {
            // something changed
            showingBack = lookingAtBack;
            if (showingBack)
            {
                // show the back side
                CardFront.gameObject.SetActive(false);
                CardBack.gameObject.SetActive(true);
            }
            else
            {
                // show the front side
                CardFront.gameObject.SetActive(true);
                CardBack.gameObject.SetActive(false);
            }

        }

    }
}
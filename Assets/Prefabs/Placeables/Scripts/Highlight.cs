using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public SpriteRenderer rendererToHighlight;
    public Color highlightColor = Color.yellow;

    Color defaultColor;
    Vector3 defaultScale;

    void Start()
    {
        defaultColor = rendererToHighlight.color;
        defaultScale = rendererToHighlight.transform.localScale;
    }

    public void SetHighlight(bool highlight)
    {
        rendererToHighlight.color = highlight ? highlightColor : defaultColor;
        rendererToHighlight.transform.localScale = defaultScale * (highlight ? 1.5f : 1.0f);
    }
}

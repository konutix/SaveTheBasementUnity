using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public SpriteRenderer rendererToHighlight;
    public Color highlightColor = Color.yellow;

    Color defaultColor;
    Vector3 defaultScale;

    int maxPriority = 0;

    void Start()
    {
        defaultColor = rendererToHighlight.color;
        defaultScale = rendererToHighlight.transform.localScale;
    }

    void LateUpdate()
    {
        maxPriority = 0;
    }

    public void SetHighlight(bool highlight, int priority = 0)
    {   
        if (priority < maxPriority) return;
        maxPriority = priority;

        rendererToHighlight.color = highlight ? highlightColor : defaultColor;
    }

    public void SetHighlightAndResize(bool highlight, int priority = 0)
    {
        if (priority < maxPriority) return;
        maxPriority = priority;

        rendererToHighlight.color = highlight ? highlightColor : defaultColor;
        rendererToHighlight.transform.localScale = defaultScale * (highlight ? 1.4f : 1.0f);
    }
}

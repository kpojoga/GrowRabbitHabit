using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scale animation
public class UIAnimScale : UIAnimations
{
    [Header("Scale")]
    [Tooltip("Initial scale")]
    public Vector3 StartScale;
    [Tooltip("Final scale")]
    public Vector3 EndScale;


    protected override void SmoothUIAnimation()
    {
        base.SmoothUIAnimation();
        
        transform.localScale = Vector3.Lerp(StartScale, EndScale, tk);
    }

    protected override void SetStart()
    {
        base.SetStart();

        transform.localScale = StartScale;
    }

    protected override void EndAnimation()
    {
        base.EndAnimation();

        transform.localScale = EndScale;
    }
}

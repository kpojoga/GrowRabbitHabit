using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Animation scale pulse
public class UIAnimScaleJump : UIAnimations
{
    [Header("Scale pulse")]
    [Tooltip("Initial scale")]
    public Vector3 StartScale;
    [Tooltip("Final scale")]
    public Vector3 EndScale;
    [Tooltip("true - double impulse; false - single impulse")]
    public bool isDouble = false;


    protected override void SmoothUIAnimation()
    {
        base.SmoothUIAnimation();

        transform.localScale = Vector3.Lerp(StartScale, EndScale
            , isDouble ? Mathf.PingPong(tk, 0.25f) * 4 : Mathf.PingPong(tk, 0.5f) * 2);
    }

    protected override void SetStart()
    {
        base.SetStart();

        transform.localScale = StartScale;
    }

    protected override void EndAnimation()
    {
        base.EndAnimation();

        transform.localScale = StartScale;
    }
}

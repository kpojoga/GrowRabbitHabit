using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Motion animation
public class UIAnimMove : UIAnimations
{
    [Header("Move")]
    [Tooltip("Start position")]
    public Vector3 StartPos;
    [Tooltip("Stop position")]
    public Vector3 EndPos;

    private RectTransform handleTransform = null;


    protected override void Start()
    {
        if (handleTransform == null)
            handleTransform = GetComponent<RectTransform>();

        base.Start();
    }

    protected override void SmoothUIAnimation()
    {
        base.SmoothUIAnimation();

        handleTransform.anchoredPosition3D = Vector3.Lerp(StartPos, EndPos, tk);
    }

    protected override void SetStart()
    {
        base.SetStart();

        if (handleTransform == null)
            handleTransform = GetComponent<RectTransform>();
        handleTransform.anchoredPosition3D = StartPos;
    }

    protected override void EndAnimation()
    {
        base.EndAnimation();

        handleTransform.anchoredPosition3D = EndPos;
    }
}

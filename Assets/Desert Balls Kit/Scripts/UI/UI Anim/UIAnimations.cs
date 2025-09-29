using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIAnimations : MonoBehaviour
{
    [Tooltip("Downtime before animation (in seconds)")]
    public float StartAfterTime = 0.0f;
    [Tooltip("Animation Run Time (in seconds)")]
    public float AnimationTime = 1;
    [Tooltip("Repeat animation")]
    public bool Loop = false;
    [Tooltip("Installation (position, markup, etc.) at startup")]
    public bool SetOnStart = true;
    [Tooltip("Is animation turned on")]
    public bool isOn = false;
    [HideInInspector]
    public UnityEvent onEndAnimation;

    protected float tk
    {
        get
        {
            return t / AnimationTime;
        }
    }
    private float t = 0.0f;
    private float dt = 0.0f;

    private bool isAnimation = false;


    protected virtual void OnValidate()
    {
        StartAfterTime = StartAfterTime < 0 ? 0 : StartAfterTime;
        AnimationTime = AnimationTime < 0 ? 0 : AnimationTime;
    }

    protected virtual void Start()
    {
        if (SetOnStart)
            SetStart();
    }

    protected virtual void Update()
    {
        if (isOn && !isAnimation)
        {
            dt += Time.deltaTime;
            if (dt >= StartAfterTime)
            {
                OnStartAfterTime();
            }
        }

        if (isAnimation)
        {
            SmoothUIAnimation();
            StopAnimation();
        }
    }

    protected virtual void SmoothUIAnimation()
    {
        t += Time.deltaTime;
    }
    
    public virtual void StartAnimation()
    {
        isAnimation = false;
        isOn = true;
        t = 0.0f;
        dt = 0.0f;

        if (SetOnStart)
            SetStart();
    }

    protected virtual void EndAnimation()
    {
        if (Loop)
        {
            StartAnimation();
        }
        else
        {
            isAnimation = false;
            isOn = false;
        }
    }

    protected virtual void OnStartAfterTime()
    {
        isAnimation = true;
        t = 0.0f;

        SetStart();
    }

    protected virtual void SetStart() { }

    void StopAnimation()
    {
        if (t >= AnimationTime)
        {
            EndAnimation();
            onEndAnimation.Invoke();
        }
    }
}

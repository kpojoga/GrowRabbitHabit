using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// animation from one color to another
public class UIAnimColor : UIAnimations
{
    [Header("Color")]
    [Tooltip("Initial color")]
    public Color StartColor;
    [Tooltip("Final color")]
    public Color EndColor;
    [Tooltip("true - returns to the initial color")]
    public bool isPingPong = false;

    private Image _image = null;


    protected override void Start()
    {
        if (_image == null)
            _image = GetComponent<Image>();

        base.Start();
    }
    protected override void SmoothUIAnimation()
    {
        base.SmoothUIAnimation();

        _image.color = Color.Lerp(StartColor, EndColor
            , !isPingPong ? tk : Mathf.PingPong(tk, 0.5f) * 2);
    }

    protected override void SetStart()
    {
        base.SetStart();

        if (_image == null)
            _image = GetComponent<Image>();

        _image.color = StartColor;
    }

    protected override void EndAnimation()
    {
        base.EndAnimation();

        _image.color = !isPingPong ? EndColor : StartColor;
    }
}

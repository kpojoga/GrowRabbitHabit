using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Sprite animation
public class UIAnimSprite : UIAnimations
{
    [Header("Sprite")]
    public List<Sprite> MoveSprite;

    private Image img = null;
    private float hT = 0;
    private float _t = 0;


    protected override void Start()
    {
        if (img == null)
            img = GetComponent<Image>();

        base.Start();
    }

    protected override void SmoothUIAnimation()
    {
        base.SmoothUIAnimation();

        _t += Time.deltaTime;
        if (hT > 0)
        {
            int id = (int)(_t / hT) + 1;
            img.sprite = MoveSprite[id >= MoveSprite.Count ? MoveSprite.Count - 1 : id];
        }
    }

    protected override void OnStartAfterTime()
    {
        base.OnStartAfterTime();

        if (MoveSprite.Count > 0)
        {
            img.sprite = MoveSprite[0];

            if (MoveSprite.Count > 1)
            {
                hT = AnimationTime / (MoveSprite.Count - 1);
            }
            else
                hT = 0;
            _t = 0;
        }
    }

    protected override void SetStart()
    {
        base.SetStart();

        if (img == null)
            img = GetComponent<Image>();

        img.sprite = MoveSprite[0];
    }

    protected override void EndAnimation()
    {
        base.EndAnimation();

        if (MoveSprite.Count > 0)
            img.sprite = MoveSprite[MoveSprite.Count - 1];
    }
}

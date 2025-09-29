using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElLevelRectangleBreaking : BaseElLevel
{
    public float Width;
    public float BreakForce;
    [HideInInspector]
    public bool isBreak=false;

    private float t = 0;
    private float tmax = 5;

    protected override void Start()
    {
        isBreak = false;
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (isBreak)
        {
            t += Time.deltaTime;
            if (t >= tmax / 2)
            {
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, (t - tmax / 2)/(tmax / 2));
            }
            if (t >= tmax)
            {
                isBreak = false;
                Destroy(gameObject);
            }
        }
    }

    public override void Draw()
    {
        float h = Width / 9;
        float _x = -(Width - h) / 2;

        foreach (RectangleDrawObject3D r3d in gameObject.GetComponentsInChildren<RectangleDrawObject3D>())
        {
            r3d.Point1.x = -h / 2;
            r3d.Point2.x = h / 2;
            r3d.transform.parent.localPosition = new Vector3(_x, 0, 0);
            _x += h;
        }

        foreach (CircleDrawObject3D r3d in gameObject.GetComponentsInChildren<CircleDrawObject3D>())
        {
            r3d.transform.localPosition = new Vector3(h / 2, 0, 0);
        }

        foreach (FixedJoint2D fj2d in gameObject.GetComponentsInChildren<FixedJoint2D>())
        {
            fj2d.breakForce = BreakForce;
            fj2d.anchor = new Vector2(h / 2, 0);
            fj2d.connectedAnchor = new Vector2(-h / 2, 0);
        }

        base.Draw();
    }
}

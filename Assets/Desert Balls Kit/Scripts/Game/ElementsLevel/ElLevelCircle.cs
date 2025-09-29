using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelCircle : BaseElLevel
{
    public float Radius = 1;

    public override void Draw()
    {
        CircleDrawObject3D be = GetComponentInChildren<CircleDrawObject3D>();
        be.Radius = Radius;

        base.Draw();
    }
}

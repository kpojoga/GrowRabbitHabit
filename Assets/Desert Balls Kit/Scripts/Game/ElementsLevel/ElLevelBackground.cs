using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElLevelBackground : BaseElLevel
{
    public Vector2 Size;

    public override void Draw()
    {
        RectangleDrawObject3D be = GetComponentInChildren<RectangleDrawObject3D>();
        be.Point1 = new Vector2(Size.x / 2, Size.y / 2);
        be.Point2 = new Vector2(-Size.x / 2, -Size.y / 2);

        base.Draw();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelRectangleDynamic : BaseElLevel
{
    public Vector2 Point1;
    public Vector2 Point2;


    public override void Draw()
    {
        RectangleDrawObject3D be = GetComponentInChildren<RectangleDrawObject3D>();
        be.Point1 = Point1;
        be.Point2 = Point2;

        base.Draw();
    }
}

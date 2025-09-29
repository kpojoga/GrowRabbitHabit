using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// create a rectangle
public class RectangleDrawObject3D : BaseDrawObject3D
{
    public Vector2 Point1;
    public Vector2 Point2;
    public float Z = 0;


    public override void Draw()
    {
        points.Clear();
        points.Add(new List<Vector3>());
        points.Last().Add(new Vector3(Point1.x, Point1.y, Z));
        points.Last().Add(new Vector3(Point1.x, Point2.y, Z));
        points.Last().Add(new Vector3(Point2.x, Point2.y, Z));
        points.Last().Add(new Vector3(Point2.x, Point1.y, Z));

        base.Draw();
    }
}

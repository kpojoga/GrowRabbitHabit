using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// cylinder making
public class CircleDrawObject3D : BaseDrawObject3D
{
    public float Radius;
    public float Z = 0;


    public override void Draw()
    {
        points.Clear();
        points.Add(new List<Vector3>());
        for (int a = 0; a < 360; a += 15)
        {
            Vector2 v2 = Expantions.Round((Vector2)(Quaternion.Euler(0, 0, a) * new Vector2(Radius, 0)));
            points.Last().Add((Vector3)v2 + new Vector3(0, 0, Z));
        }

        base.Draw();
    }
}

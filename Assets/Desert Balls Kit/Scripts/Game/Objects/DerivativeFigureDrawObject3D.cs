using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// create an arbitrary shape
public class DerivativeFigureDrawObject3D : BaseDrawObject3D
{
    public List<Vector2> Points = new List<Vector2>();
    public float Z = 0;


    public override void Draw()
    {
        points.Clear();
        points.Add(new List<Vector3>());
        points.Last().AddRange(Points.Select(v => new Vector3(v.x, v.y, Z)));

        base.Draw();
    }
}

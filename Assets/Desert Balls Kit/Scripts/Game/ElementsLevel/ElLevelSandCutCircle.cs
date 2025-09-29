using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelSandCutCircle : BaseElLevel
{
    public float Radius;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 _R = new Vector2(Radius, 0);
        for (int a = 0; a < 360; a += 18)
        {
            Gizmos.DrawLine(Position + Quaternion.Euler(0, 0, a - 4.5f) * _R, Position + Quaternion.Euler(0, 0, a + 4.5f) * _R);
        }
    }
}

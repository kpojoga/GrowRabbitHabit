using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class Expantions
{
    public static float Eps = 0.001f;

    // Rounding points
    public static float Round(float f)
    {
        return Mathf.RoundToInt(f / Eps) * Eps;
    }

    public static Vector2 Round(Vector2 f)
    {
        f.x = Round(f.x);
        f.y = Round(f.y);
        return f;
    }

    public static Vector3 Round(Vector3 f)
    {
        f.x = Round(f.x);
        f.y = Round(f.y);
        f.z = Round(f.z);
        return f;
    }

    // Clockwise bypass
    public static bool HasСlockwise(List<Vector3> con)
    {
        bool _b = false;

        if (con.Count > 0)
        {
            Vector3 min = con[0];
            int id_min = 0;

            int i = 0;
            foreach (Vector3 v3 in con)
            {
                if (v3.x < min.x)
                {
                    min = v3;
                    id_min = i;
                }
                i++;
            }

            int id_prev = (id_min - 1 < 0) ? con.Count - 1 : id_min - 1;
            int id_next = (id_min + 1 >= con.Count) ? 0 : id_min + 1;

            if ((con[id_prev].x - con[id_min].x) * (con[id_next].y - con[id_min].y)
                - (con[id_prev].y - con[id_min].y) * (con[id_next].x - con[id_min].x) > 0)
                _b = true;
        }

        return _b;
    }

    // Check if the points lie on the segment (with the thickness of the segment)
    public static bool HasPointLies(Vector3 point_start, Vector3 point_end, Vector3 point)
    {
        return Mathf.Abs(Vector2.Distance(point_start, point_end) - Vector2.Distance(point_start, point)
            - Vector2.Distance(point_end, point)) <= Eps/10;
    }

    // Line intersection point
    public static Vector3 getPointOfIntersection(Vector2 _p1, Vector2 _p2, Vector2 _p3, Vector2 _p4)
    {
        Vector2 p1 = new Vector2(Round(_p1.x), Round(_p1.y));
        Vector2 p2 = new Vector2(Round(_p2.x), Round(_p2.y));

        Vector2 p3 = new Vector2(Round(_p3.x), Round(_p3.y));
        Vector2 p4 = new Vector2(Round(_p4.x), Round(_p4.y));

        Vector2 pos = Vector2.zero;

        float denominator = (p4.y - p3.y) * (p2.x - p1.x) - (p4.x - p3.x) * (p2.y - p1.y);
        if (denominator == 0)
            // lines are parallel
            // If both the numerator and denominator are equal to zero, then the lines coincide.
            return new Vector3(0, 0, -1);
        else
        {
            float u12 = ((p4.x - p3.x) * (p1.y - p3.y) - (p4.y - p3.y) * (p1.x - p3.x)) / denominator;
            float u34 = ((p2.x - p1.x) * (p1.y - p3.y) - (p2.y - p1.y) * (p1.x - p3.x)) / denominator;
            pos = p1 + (p2 - p1) * u12;

            // If u12 and u34 are on the interval [0,1], then the segments have an intersection point
            if (u12 < 0 || u12 > 1)
                //the intersection point is not on the segment p1, p2
                return new Vector3(0, 0, -1);
            if (u34 < 0 || u34 > 1)
                //the intersection point is not on the segment p3, p4
                return new Vector3(0, 0, -1);
        }
        return pos;
    }

    // Check if the points are equal
    public static bool Equals(Vector3 A, Vector3 B)
    {
        return Vector2.Distance(A, B) < Eps/10;
    }
}

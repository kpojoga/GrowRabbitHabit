using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelSandCutRectangle : BaseElLevel
{
    public Vector2 Point1;
    public Vector2 Point2;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Quaternion rot = Quaternion.Euler(0, 0, Rotate);
        int d1 = 10;
        int d2 = 10;
        if(Mathf.Abs(Point1.x - Point2.x) < Mathf.Abs(Point1.y - Point2.y))
            d2 = Mathf.RoundToInt(d1 * Mathf.Abs(Point1.y - Point2.y) / Mathf.Abs(Point1.x - Point2.x));
        else
            d1 = Mathf.RoundToInt(d2 * Mathf.Abs(Point1.x - Point2.x) / Mathf.Abs(Point1.y - Point2.y));
        DottedLine(Position, new Vector2(Point1.x, Point1.y), new Vector2(Point2.x, Point1.y), rot, d1);
        DottedLine(Position, new Vector2(Point2.x, Point1.y), new Vector2(Point2.x, Point2.y), rot, d2);
        DottedLine(Position, new Vector2(Point2.x, Point2.y), new Vector2(Point1.x, Point2.y), rot, d1);
        DottedLine(Position, new Vector2(Point1.x, Point2.y), new Vector2(Point1.x, Point1.y), rot, d2);
    }

    void DottedLine(Vector3 pos, Vector3 p1, Vector3 p2, Quaternion rot, int dot)
    {
        dot += dot % 2;
        float d = Vector2.Distance(p1, p2);
        Vector3 h = (p2 - p1) / dot;
        for (int i = 0; i <= dot; i += 2)
        {
            Vector3 n1 = p1 + h * i - h / 2;
            Vector3 n2 = p1 + h * (i + 1) - h / 2;
            if (Vector2.Distance(n1, p2) > d)
                n1 = p1;
            if (Vector2.Distance(n2, p1) > d)
                n2 = p2;
            Gizmos.DrawLine(pos + rot * n1, pos + rot * n2);
        }
    }
}

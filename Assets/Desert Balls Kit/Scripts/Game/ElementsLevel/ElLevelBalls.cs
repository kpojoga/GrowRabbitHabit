using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelBalls : BaseElLevel
{
    public float Radius;
    public bool Sleeping;

    public static float R = 0.1f;

    public override void Draw()
    {
        if (Application.isPlaying)
        {
            List<Vector2> p = GetPoints();

            GameObject go = (GameObject)Resources.Load("Game Objects/GameBall");
            foreach (Vector2 v2 in p)
            {
                GameBall b = Instantiate(go, v2, Quaternion.identity).GetComponent<GameBall>();
                b.R = R * Random.Range(0.7f, 1f);
                b.Sleeping = Sleeping;
            }
        }

        base.Draw();
    }

    List<Vector2> GetPoints()
    {
        List<Vector2> p = new List<Vector2>();

        Vector2 pv2 = Position;

        float D = R * 2;
        int H = (int)(Radius / D);

        for (int k = 0; k < H; k++)
        {
            Vector2 rHv2 = new Vector2(-D * k - R, 0);
            int rH = (int)Mathf.Abs((Mathf.PI * rHv2.x) / D);
            float ang = rH > 0 ? (180.0f / rH) : 0;

            for (int i = 0; i <= rH; i++)
            {
                p.Add(pv2 + (Vector2)(Quaternion.Euler(0, 0, ang * i) * rHv2));
            }
        }

        return p;
    }

    private void OnDrawGizmos()
    {
        List<Vector2> p = GetPoints();

        Gizmos.color = Color.blue;
        foreach (Vector2 v2 in p)
        {
            Gizmos.DrawSphere(v2, R);
        }
    }
}

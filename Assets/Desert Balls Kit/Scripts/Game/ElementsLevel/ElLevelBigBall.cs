using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelBigBall : BaseElLevel
{
    public bool Sleeping;

    public static float R = 0.4f;

    public override void Draw()
    {
        if (Application.isPlaying)
        {
            GameObject go = (GameObject)Resources.Load("Game Objects/GameBallBig");
            GameBall b = go.GetComponent<GameBall>();
            b.R = R;
            b.Sleeping = Sleeping;
            Instantiate(go, Position, Quaternion.identity);
        }

        base.Draw();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Position, R);
    }
}

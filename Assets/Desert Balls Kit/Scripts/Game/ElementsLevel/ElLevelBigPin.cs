using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelBigPin : BaseElLevel
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BigBall")
        {
            GameBall gb = collision.GetComponent<GameBall>();
            if (gb != null)
            {
                gb.AddBalls();
            }
        }
    }
}

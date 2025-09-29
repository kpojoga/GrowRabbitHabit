using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelFanKill : BaseElLevel
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ball" || collision.tag == "BigBall")
        {
            GameBall gb = collision.GetComponent<GameBall>();
            if (gb != null)
            {
                gb.Kill();
            }
        }
    }
}

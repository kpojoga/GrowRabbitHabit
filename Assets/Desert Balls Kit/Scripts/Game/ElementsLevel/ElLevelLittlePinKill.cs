using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelLittlePinKill : BaseElLevel
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ball" || collision.collider.tag == "BigBall")
        {
            GameBall gb = collision.collider.GetComponent<GameBall>();
            if (gb != null)
            {
                gb.Kill();
            }
        }
    }
}

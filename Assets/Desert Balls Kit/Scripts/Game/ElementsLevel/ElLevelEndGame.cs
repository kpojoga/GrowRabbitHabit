using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelEndGame : BaseElLevel
{
    private ElLevelEndGameTrigger centerTr = null;
    private int countBalls = 0;
    private bool isTrigger = false;
    private bool isEnd = false;
    private float t = 0;
    private float tMax = 3;


    protected override void Start()
    {
        base.Start();
        countBalls = 0;
        isTrigger = false;
        isEnd = false;
        t = 0;
        centerTr = GetComponentInChildren<ElLevelEndGameTrigger>();
        if (centerTr != null)
        {
            centerTr.onTriggerEnter2D.RemoveAllListeners();
            centerTr.onTriggerEnter2D.AddListener(onTriggerEnter2D);
        }
    }

    private void FixedUpdate()
    {
        if (isTrigger && !isEnd)
        {
            t += Time.deltaTime;
            if (t >= tMax)
            {
                isEnd = true;
                GameManager.instance.AddLevelDiamonds(countBalls);
                GameManager.instance.EndGame();
            }
        }
    }

    void onTriggerEnter2D(Collider2D collision)
    {
        if (!isEnd)
        {
            if (collision.tag == "Ball")
            {
                if (!isTrigger)
                {
                    Menus.instance.HideGame();
                }
                isTrigger = true;
                countBalls++;
                t = 0;
                collision.GetComponent<GameBall>().Kill();
            }
        }
    }
}

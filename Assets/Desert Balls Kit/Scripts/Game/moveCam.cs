using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Camera movement behind the lowest ball
public class moveCam : MonoBehaviour
{
    public static moveCam instance = null;

    Vector3 newPos = Vector3.zero;
    Vector3 offset = new Vector3(0, -5, -10);
    BoxCollider2D bc2D = null;


    void Awake()
    {
        try
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message + " Stack: " + e.StackTrace);
        }

        bc2D = GetComponent<BoxCollider2D>();
    }


    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * 2);
    }

    public void Restart()
    {
        newPos = Vector3.zero + offset;
        transform.position = newPos;

        bc2D.size = GameManager.instance.GetSizeField();
        bc2D.offset = new Vector2(0, -GameManager.instance.GetSizeField().y / 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            updPos(collision.transform.position);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            updPos(collision.transform.position);
        }
    }

    void updPos(Vector2 v2)
    {
        if (newPos.y > v2.y)
            newPos = new Vector3(0, v2.y, offset.z);
        if(newPos.y < -GameManager.instance.GetSizeField().y)
            newPos = new Vector3(0, -GameManager.instance.GetSizeField().y, offset.z);
    }
}

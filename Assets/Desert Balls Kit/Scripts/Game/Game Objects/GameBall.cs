using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klas with the ball, and all its interactions with surrounding objects
public class GameBall : MonoBehaviour
{
    public GameObject PS_collision; // particle system in the collision of a ball with sand
    public GameObject S_collision_pipe; // sound when the ball hits the sand
    public GameObject S_collision_ball; // sound when the ball hits the ball
    public GameObject S_collision_metal; // sound when the ball collides with other objects
    public bool HasBig = false; // if the ball is big
    [Space]
    public Transform ParentMesh; // where is the ball
    [HideInInspector]
    public float R = 0.05f; // radius of regular balls
    [HideInInspector]
    public bool Sleeping = false; // Whether in sleep mode

    private Rigidbody2D rb2D;
    private CircleCollider2D cc2D;
    private MeshRenderer mr;
    private bool isKill = false; // if the ball is destroyed
    private float t = 0;
    private float tmax = .5f; // time until the end of the destruction animation


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        if (Sleeping)
            rb2D.bodyType = RigidbodyType2D.Static;
        cc2D = GetComponent<CircleCollider2D>();
        cc2D.radius = R;
        ParentMesh.localScale = new Vector3(R, R, R) * 2;

        mr = GetComponentInChildren<MeshRenderer>();
        if (Sleeping)
            mr.material = GameSettings.instance.mSleepBall;
        else
            mr.material = GameSettings.instance.GetMaterialSelectBall();
    }

    void Update()
    {
        if (isKill)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t / tmax);
            if (t >= tmax)
            {
                isKill = false;
                Destroy(gameObject);
            }
        }
    }

    public void Kill()
    {
        if (!isKill)
        {
            t = 0;
            isKill = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ball")
        {
            if (Sleeping)
            {
                Sleeping = false;
                rb2D.bodyType = RigidbodyType2D.Dynamic;
                mr.material = GameSettings.instance.GetMaterialSelectBall();
            }
            if (collision.contactCount > 0)
            {
                Instantiate(S_collision_ball, collision.contacts[0].point, Quaternion.identity);
            }
        }
        else if (collision.collider.tag == "Sand")
        {
            if (collision.contactCount > 0) {
                Instantiate(PS_collision, collision.contacts[0].point, Quaternion.identity);
                Instantiate(S_collision_pipe, collision.contacts[0].point, Quaternion.identity);
            }
        }
        else if (collision.collider.tag == "BigBall")
        {
            if (collision.contactCount > 0)
            {
                Instantiate(S_collision_ball, collision.contacts[0].point, Quaternion.identity);
            }
        }
        else
        {
            if (collision.contactCount > 0)
            {
                Instantiate(S_collision_metal, collision.contacts[0].point, Quaternion.identity);
            }
        }
    }

    // create regular balls (only for large balls)
    public void AddBalls()
    {
        List<Vector2> p = GetPoints(ElLevelBalls.R/2);

        GameObject go = (GameObject)Resources.Load("Game Objects/GameBall");
        foreach (Vector2 v2 in p)
        {
            Instantiate(go, v2, Quaternion.identity).GetComponent<GameBall>().R = ElLevelBalls.R * Random.Range(0.7f, 1f);
        }
        Destroy(gameObject);
    }

    List<Vector2> GetPoints(float _R)
    {
        List<Vector2> p = new List<Vector2>();

        Vector2 pv2 = transform.position;

        float D = _R * 2;
        int H = (int)(R / D);

        for (int k = 0; k < H; k++)
        {
            Vector2 rHv2 = new Vector2(-D * k - _R, 0);
            int rH = (int)Mathf.Abs((Mathf.PI * rHv2.x) / D);
            float ang = rH > 0 ? (180.0f / rH) : 0;

            for (int i = 0; i <= rH; i++)
            {
                p.Add(pv2 + (Vector2)(Quaternion.Euler(0, 0, ang * i) * rHv2));
            }
        }

        return p;
    }
}

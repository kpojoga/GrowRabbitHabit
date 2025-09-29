using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelBomb : BaseElLevel
{
    public float RadiusBoom;

    public GameObject PS_boom;
    public GameObject S_boom;
    public Material MBoom;
    private Material MOld;
    private MeshRenderer mr;

    private bool isBoom = false;
    float t = 0;


    protected override void Update()
    {
        base.Update();

        if (isBoom)
        {
            t += Time.deltaTime;
            float pp = Mathf.PingPong(t, 0.5f);
            if (pp <= 0.01f)
                mr.material = MOld;
            if (pp >= 0.49f)
                mr.material = MBoom;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, t);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBoom && (collision.tag == "Ball" || collision.tag == "BigBall"))
        {
            isBoom = true;
            mr = transform.Find("Bomb/Sphere").GetComponent<MeshRenderer>();
            MOld = mr.material;
            Invoke("Detonate", 2);
        }
    }

    void Detonate()
    {
        Vector2 pos = transform.position;

        Instantiate(PS_boom, pos, Quaternion.identity);
        Instantiate(S_boom, pos, Quaternion.identity);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, RadiusBoom);
        foreach(Collider2D hit in colliders)
        {
            ElLevelSand ells = hit.GetComponent<ElLevelSand>();
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            ElLevelCircle elc = hit.GetComponent<ElLevelCircle>();
            ElLevelRectangle elr = hit.GetComponent<ElLevelRectangle>();

            if (ells != null)
                ells.AddContour(pos, pos, RadiusBoom);
            if (rb != null)
                AddExplosionForce(rb, 0.5f, pos, RadiusBoom, 5f, ForceMode2D.Impulse);
            if (elc != null)
                Destroy(elc.gameObject);
            if (elr != null)
                Destroy(elr.gameObject);
        }
        Destroy(gameObject);
    }

    void AddExplosionForce(Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Force)
    {
        var explosionDir = rb.position - explosionPosition;
        var explosionDistance = explosionDir.magnitude;

        // Normalize without computing magnitude again
        if (upwardsModifier == 0)
            explosionDir /= explosionDistance;
        else
        {
            // From Rigidbody.AddExplosionForce doc:
            // If you pass a non-zero value for the upwardsModifier parameter, the direction
            // will be modified by subtracting that value from the Y component of the centre point.
            explosionDir.y += upwardsModifier;
            explosionDir.Normalize();
        }

        rb.AddForce(Mathf.Lerp(0, explosionForce, (1 - explosionDistance)) * explosionDir, mode);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelTeleport : BaseElLevel
{
    public Vector2 PositionOut;
    public float RoatateOut;

    private ElLevelTeleport_In _in;
    private Transform tr_in = null;
    private Transform tr_out = null;

    protected override void Start()
    {
        _in = GetComponentInChildren<ElLevelTeleport_In>();
        _in.action.RemoveAllListeners();
        _in.action.AddListener(triggerIN);

        GET();

        base.Start();
    }

    public override void Draw()
    {
        GET();
        tr_out.localPosition = PositionOut;
        tr_out.localRotation = Quaternion.Euler(0, 0, RoatateOut);

        base.Draw();
    }

    void triggerIN(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            collision.transform.position = tr_out.position;
        }
    }

    void GET()
    {
        if (tr_in == null)
        {
            tr_in = transform.Find("Teleport In");
            tr_in.localPosition = Vector3.zero;
        }
        if (tr_out == null)
            tr_out = transform.Find("Teleport Out");
    }
}

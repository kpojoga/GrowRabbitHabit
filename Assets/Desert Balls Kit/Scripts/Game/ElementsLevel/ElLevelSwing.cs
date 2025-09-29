using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelSwing : BaseElLevel
{
    public float AngularDrag = 3;

    public override void Draw()
    {
        Rigidbody2D rg2d = GetComponent<Rigidbody2D>();
        if (rg2d)
        {
            rg2d.angularDamping = AngularDrag;

            rg2d.centerOfMass = Vector2.zero;
        }

        base.Draw();
    }
}

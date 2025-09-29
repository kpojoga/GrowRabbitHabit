using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElLevelMill : BaseElLevel
{
    public float Speed = -100;


    public override void Draw()
    {
        if (GetComponent<HingeJoint2D>() != null)
        {
            JointMotor2D motor = GetComponent<HingeJoint2D>().motor;
            motor.motorSpeed = Speed;
            GetComponent<HingeJoint2D>().motor = motor;

            GetComponent<Rigidbody2D>().centerOfMass = Vector2.zero;
        }

        base.Draw();
    }
}

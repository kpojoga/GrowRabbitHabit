using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// coupling between objects (may collapse)
public class JointBreak2D : MonoBehaviour
{
    void OnJointBreak2D(Joint2D brokenJoint)
    {
        transform.parent.GetComponent<ElLevelRectangleBreaking>().isBreak = true;

        foreach (CircleDrawObject3D r3d in brokenJoint.GetComponentsInChildren<CircleDrawObject3D>())
        {
            Destroy(r3d.gameObject);
        }
    }
}

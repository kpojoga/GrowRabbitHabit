using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[ExecuteInEditMode]
public class BaseElLevel : MonoBehaviour
{
    public Vector3 Position = Vector3.zero;
    public float Rotate = 0;


    protected virtual void OnValidate()
    {
        transform.localPosition = Position;
        transform.localRotation = Quaternion.Euler(0, 0, Rotate);
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            Position = transform.localPosition;
            Rotate = transform.localRotation.eulerAngles.z;
            Rotate = Rotate - ((Rotate > 180) ? 360 : 0);
            transform.localRotation = Quaternion.Euler(0, 0, Rotate);

            Draw();
        }
        else
        {

        }
    }
    
    public virtual void Draw()
    {
        transform.localPosition = Position;
        transform.localRotation = Quaternion.Euler(0, 0, Rotate);
        
        foreach (BaseDrawObject3D be in GetComponentsInChildren<BaseDrawObject3D>().ToList())
        {
            be.Draw();
        }
    }
}

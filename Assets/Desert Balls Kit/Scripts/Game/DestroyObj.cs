using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Delete an object after a specified time
public class DestroyObj : MonoBehaviour
{
    public float TimeDel = 1;


    void Start()
    {
        Invoke("Del", TimeDel);
    }

    void Del()
    {
        Destroy(gameObject);
    }
}

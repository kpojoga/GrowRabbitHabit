using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// end game twitter (balls hit it)
public class ElLevelEndGameTrigger : MonoBehaviour
{
    [System.Serializable]
    public class OnTrEnt2D : UnityEvent<Collider2D> { };
    [HideInInspector]
    public OnTrEnt2D onTriggerEnter2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(onTriggerEnter2D != null)
            onTriggerEnter2D.Invoke(collision);
    }
}

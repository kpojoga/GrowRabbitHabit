using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// twitter for teleport (input)
public class ElLevelTeleport_In : MonoBehaviour
{
    [System.Serializable]
    public class OnTrigger : UnityEvent<Collider2D> { };
    [HideInInspector]
    public OnTrigger action;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (action != null)
        {
            action.Invoke(collision);
        }
    }
}

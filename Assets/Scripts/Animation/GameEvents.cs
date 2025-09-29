using System;
using UnityEngine;

public enum AnimAction { Jump, Water, Dance }
public class GameEvents : MonoBehaviour
{
    public static event Action<AnimAction> OnAnimRequested;

    // Чистим статики при перезапуске Play без Domain Reload
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics() => OnAnimRequested = null;

    public static void RequestAnim(AnimAction action) => OnAnimRequested?.Invoke(action);
}

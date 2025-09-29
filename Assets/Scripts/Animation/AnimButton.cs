using UnityEngine;

public class AnimButton : MonoBehaviour
{
    // Список доступных действий
    private static readonly AnimAction[] Pool =
    {
        AnimAction.Jump,
        AnimAction.Water,
        AnimAction.Dance
    };

    // Вешаем этот метод на Button.onClick
    public void Raise()
    {
        var action = Pool[Random.Range(0, Pool.Length)];
        GameEvents.RequestAnim(action);
    }
}
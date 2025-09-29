using UnityEngine;

public class AnimatorEventListener : MonoBehaviour
{
    [SerializeField] private Animator animator;

    static readonly int Jump  = Animator.StringToHash("Jump");
    static readonly int Water = Animator.StringToHash("Water");
    static readonly int Dance = Animator.StringToHash("Dance");

    void OnEnable()  => GameEvents.OnAnimRequested += Handle;
    void OnDisable() => GameEvents.OnAnimRequested -= Handle;

    void Handle(AnimAction action)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
        {
            Debug.LogWarning("Animator is missing or has no Controller", this);
            return;
        }

        // Сбрасываем прочие триггеры, чтобы не залипали переходы
        animator.ResetTrigger(Jump);
        animator.ResetTrigger(Water);
        animator.ResetTrigger(Dance);

        switch (action)
        {
            case AnimAction.Jump:  animator.SetTrigger(Jump);  break;
            case AnimAction.Water: animator.SetTrigger(Water); break;
            case AnimAction.Dance: animator.SetTrigger(Dance); break;
        }
    }
}
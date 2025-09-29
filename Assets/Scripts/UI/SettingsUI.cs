using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Toggle soundToggle;

    private void OnEnable()
    {
        if (SoundManager.Instance != null && soundToggle != null)
        {
            // Не триггерим коллбек при инициализации
            soundToggle.SetIsOnWithoutNotify(SoundManager.Instance.IsSoundEnabled);
            soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
        }
    }

    private void OnDisable()
    {
        if (soundToggle != null)
            soundToggle.onValueChanged.RemoveListener(OnSoundToggleChanged);
    }

    private void OnSoundToggleChanged(bool isEnabled)
    {
        SoundManager.Instance?.SetSoundEnabled(isEnabled);
    }
}
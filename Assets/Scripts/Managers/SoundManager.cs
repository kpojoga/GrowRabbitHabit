using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    const string SOUND_KEY = "SoundEnabled";
    public bool IsSoundEnabled { get; private set; }
    public event System.Action<bool> SoundEnabledChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        IsSoundEnabled = PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
        Apply(IsSoundEnabled); // <- применяем при старте, чтобы музыка не играла, если выключено
    }

    public void SetSoundEnabled(bool isEnabled)
    {
        if (IsSoundEnabled == isEnabled) return;
        IsSoundEnabled = isEnabled;
        PlayerPrefs.SetInt(SOUND_KEY, isEnabled ? 1 : 0);
        PlayerPrefs.Save();

        Apply(isEnabled);            // <- применяем НЕМЕДЛЕННО
        SoundEnabledChanged?.Invoke(isEnabled);
    }

    void Apply(bool on)
    {
        AudioListener.pause = !on;   // Пауза всем AudioSource
        // Альтернатива: AudioListener.volume = on ? 1f : 0f;
    }
}

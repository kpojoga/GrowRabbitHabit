using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopItem[] foodItems;
    public ShopItem[] waterItems;
    public ChickenManager rabbitManager;
    public AudioSource audioSource;
    public AudioSource musicSource;
    public AudioClip purchaseSound;
    public AudioClip music;

    private void OnEnable()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.SoundEnabledChanged += ApplyMusicState;
    }
    
    private void Start()
    {
        // стартовая синхронизация
        ApplyMusicState(SoundManager.Instance != null && SoundManager.Instance.IsSoundEnabled);
    }
    
    private void OnDisable()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.SoundEnabledChanged -= ApplyMusicState;
    }
    
    private void ApplyMusicState(bool on)
    {
        if (musicSource == null || music == null) return;

        if (on)
        {
            if (musicSource.clip != music) musicSource.clip = music;
            musicSource.loop = true;
            if (!musicSource.isPlaying) musicSource.Play();
        }
        else
        {
            if (musicSource.isPlaying) musicSource.Stop();
        }
    }

    public bool BuyItem(int index, ShopItemType type)
    {
        ShopItem item = null;
        if (type == ShopItemType.Food && index >= 0 && index < foodItems.Length)
            item = foodItems[index];
        else if (type == ShopItemType.Water && index >= 0 && index < waterItems.Length)
            item = waterItems[index];
        if (item == null) return false;
        if (Wallet.Instance != null && Wallet.Instance.TryPurchase(item.price))
        {
            if (SoundManager.Instance != null && SoundManager.Instance.IsSoundEnabled && audioSource != null && purchaseSound != null)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(purchaseSound);
            }
            if (type == ShopItemType.Food)
                rabbitManager.FeedChicken(item.amount);
            else if (type == ShopItemType.Water)
                rabbitManager.GiveWaterToChicken(item.amount);
            return true;
        }
        return false;
    }
} 
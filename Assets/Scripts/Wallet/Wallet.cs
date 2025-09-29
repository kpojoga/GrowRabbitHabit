using UnityEngine;

public class Wallet : MonoBehaviour
{
    public static Wallet Instance { get; private set; }
    public int coins = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        Save();
    }

    public bool SubtractCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            Save();
            return true;
        }
        return false;
    }

    public bool TryPurchase(int price)
    {
        return SubtractCoins(price);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("wallet_coins", coins);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        coins = PlayerPrefs.GetInt("wallet_coins", 0);
    }
} 
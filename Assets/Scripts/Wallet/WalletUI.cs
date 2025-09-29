using UnityEngine;
using TMPro;

public class WalletUI : MonoBehaviour
{
    public TextMeshProUGUI coinsText;

    void Update()
    {
        if (Wallet.Instance != null)
            coinsText.text = $" {Wallet.Instance.coins}";
    }
} 
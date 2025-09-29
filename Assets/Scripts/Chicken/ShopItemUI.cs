using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image iconImage;
    public Button buyButton;
    public TextMeshProUGUI priceText;

    private ShopItem item;
    private ShopManager shopManager;
    private int itemIndex;
    private ShopItemType itemType;

    public void Setup(ShopItem item, int index, ShopItemType type, ShopManager manager)
    {
        this.item = item;
        this.itemIndex = index;
        this.itemType = type;
        this.shopManager = manager;
        nameText.text = item.itemName;
        iconImage.sprite = item.icon;
        string plusText = $"+{item.amount}";
        descriptionText.text = plusText;
        priceText.text = $"{item.price}";
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(Buy);
    }

    private void Buy()
    {
        shopManager.BuyItem(itemIndex, itemType);
    }
}
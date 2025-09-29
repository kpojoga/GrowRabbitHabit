using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public ShopManager shopManager;
    public Transform foodPanel;
    public Transform waterPanel;
    public GameObject shopItemPrefab;

    void Start()
    {
        // Создаем элементы для еды
        for (int i = 0; i < shopManager.foodItems.Length; i++)
        {
            var item = shopManager.foodItems[i];
            var itemObj = Instantiate(shopItemPrefab, foodPanel);
            var itemUI = itemObj.GetComponent<ShopItemUI>();
            itemUI.Setup(item, i, ShopItemType.Food, shopManager);
        }
        // Создаем элементы для воды
        for (int i = 0; i < shopManager.waterItems.Length; i++)
        {
            var item = shopManager.waterItems[i];
            var itemObj = Instantiate(shopItemPrefab, waterPanel);
            var itemUI = itemObj.GetComponent<ShopItemUI>();
            itemUI.Setup(item, i, ShopItemType.Water, shopManager);
        }
    }
} 
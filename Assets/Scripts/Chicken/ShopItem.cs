using UnityEngine;

public enum ShopItemType { Food, Water }

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public Sprite icon;
    public ShopItemType itemType;
    public int price;
    public int amount;
} 
using UnityEngine;

[System.Serializable]
public class Chicken : MonoBehaviour
{
    public ChickenData data = new ChickenData();

    [Header("Параметры уменьшения")]
    public float foodDecreaseInterval = 5f;
    public float waterDecreaseInterval = 4f;
    public int foodDecreaseAmount = 1;
    public int waterDecreaseAmount = 1;

    private float foodTimer = 0f;
    private float waterTimer = 0f;

    public void Feed(int amount)
    {
        if (data.foodPoints + amount > data.maxFoodPoints)
            data.foodPoints = data.maxFoodPoints;
        else
            data.foodPoints += amount;
    }

    public void GiveWater(int amount)
    {
        if (data.waterPoints + amount > data.maxWaterPoints)
            data.waterPoints = data.maxWaterPoints;
        else
            data.waterPoints += amount;
    }

    public bool UseFood(int amount)
    {
        if (data.foodPoints >= amount)
        {
            data.foodPoints -= amount;
            return true;
        }
        return false;
    }

    public bool UseWater(int amount)
    {
        if (data.waterPoints >= amount)
        {
            data.waterPoints -= amount;
            return true;
        }
        return false;
    }

    void Update()
    {
        foodTimer += Time.deltaTime;
        waterTimer += Time.deltaTime;

        if (foodTimer >= foodDecreaseInterval)
        {
            UseFood(foodDecreaseAmount);
            foodTimer = 0f;
        }
        if (waterTimer >= waterDecreaseInterval)
        {
            UseWater(waterDecreaseAmount);
            waterTimer = 0f;
        }
    }
} 
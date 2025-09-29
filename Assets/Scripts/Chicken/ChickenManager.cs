using UnityEngine;

public class ChickenManager : MonoBehaviour
{
    public Chicken rabbit;

    public void FeedChicken(int amount)
    {
        rabbit.Feed(amount);
    }

    public void GiveWaterToChicken(int amount)
    {
        rabbit.GiveWater(amount);
    }

    public bool TryUseFood(int amount)
    {
        return rabbit.UseFood(amount);
    }

    public bool TryUseWater(int amount)
    {
        return rabbit.UseWater(amount);
    }

    public void SaveChicken()
    {
        PlayerPrefs.SetInt("foodPoints", rabbit.data.foodPoints);
        PlayerPrefs.SetInt("waterPoints", rabbit.data.waterPoints);
        PlayerPrefs.SetInt("maxFoodPoints", rabbit.data.maxFoodPoints);
        PlayerPrefs.SetInt("maxWaterPoints", rabbit.data.maxWaterPoints);
        PlayerPrefs.Save();
    }

    public void LoadChicken()
    {
        rabbit.data.foodPoints = PlayerPrefs.GetInt("foodPoints", rabbit.data.foodPoints);
        rabbit.data.waterPoints = PlayerPrefs.GetInt("waterPoints", rabbit.data.waterPoints);
        rabbit.data.maxFoodPoints = PlayerPrefs.GetInt("maxFoodPoints", rabbit.data.maxFoodPoints);
        rabbit.data.maxWaterPoints = PlayerPrefs.GetInt("maxWaterPoints", rabbit.data.maxWaterPoints);
    }
} 
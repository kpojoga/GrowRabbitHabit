using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChickenUI : MonoBehaviour
{
    public ChickenManager rabbitManager;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI waterText;
    //public Button feedButton;
    //public Button waterButton;
    //public Button saveButton;
    //public Button loadButton;

    void Start()
    {
        // feedButton.onClick.AddListener(() => chickenManager.FeedChicken(1));
        // waterButton.onClick.AddListener(() => chickenManager.GiveWaterToChicken(1));
        // saveButton.onClick.AddListener(() => chickenManager.SaveChicken());
        // loadButton.onClick.AddListener(() => chickenManager.LoadChicken());
    }

    void Update()
    {
        foodText.text = $"Feed: {rabbitManager.rabbit.data.foodPoints}/{rabbitManager.rabbit.data.maxFoodPoints}";
        waterText.text = $"Water: {rabbitManager.rabbit.data.waterPoints}/{rabbitManager.rabbit.data.maxWaterPoints}";
    }
} 
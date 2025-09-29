using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TaskItemUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private Toggle completionToggle;
    [SerializeField] private Image backgroundImage;
    
    [Header("Colors")]
    [SerializeField] private Color normalColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    [SerializeField] private Color completedColor = new Color(0.2f, 0.4f, 0.2f, 0.8f);
    
    private string taskId;
    private bool isCompleted;
    
    public UnityEvent<TaskItemUI> OnTaskCompleted = new UnityEvent<TaskItemUI>();
    
    public void Initialize(Task task)
    {
        taskId = task.id;
        isCompleted = task.isCompleted;
        
        // Set UI elements
        titleText.text = task.title;
        descriptionText.text = task.description;
        rewardText.text = $"+{task.coinReward} coins";
        
        // Set toggle state
        completionToggle.isOn = isCompleted;
        completionToggle.interactable = !isCompleted;
        
        // Update visual state
        UpdateVisuals();
        
        // Add listener for toggle changes
        completionToggle.onValueChanged.AddListener(OnToggleChanged);
    }
    
    private void UpdateVisuals()
    {
        if (isCompleted)
        {
            titleText.fontStyle = FontStyles.Strikethrough;
            descriptionText.fontStyle = FontStyles.Strikethrough;
            backgroundImage.color = completedColor;
            completionToggle.interactable = false;
        }
        else
        {
            titleText.fontStyle = FontStyles.Normal;
            descriptionText.fontStyle = FontStyles.Normal;
            backgroundImage.color = normalColor;
            completionToggle.interactable = true;
        }
    }
    
    private void OnToggleChanged(bool isOn)
    {
        if (isOn && !isCompleted)
        {
            isCompleted = true;
            UpdateVisuals();
            OnTaskCompleted?.Invoke(this);
        }
    }
    
    public string GetTaskId() => taskId;
    
    private void OnDestroy()
    {
        // Clean up listeners
        if (completionToggle != null)
        {
            completionToggle.onValueChanged.RemoveListener(OnToggleChanged);
        }
    }
}

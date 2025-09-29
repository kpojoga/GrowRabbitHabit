using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject addTaskPanel; // Панель добавления задачи
    [SerializeField] private TaskItemUI taskItemPrefab;
    [SerializeField] private Transform taskListContent;
    [SerializeField] private TMP_InputField taskTitleInput;
    [SerializeField] private TMP_InputField taskDescriptionInput;
    [SerializeField] private TMP_InputField rewardInput;
    [SerializeField] private Button addTaskButton;
    [SerializeField] private Button closeButton;
    
    [Header("Settings")]
    [SerializeField] private KeyCode toggleKey = KeyCode.T;
    
    private List<TaskItemUI> taskItems = new List<TaskItemUI>();
    
    private void Start()
    {
        // Initialize UI elements
        addTaskButton.onClick.AddListener(OnAddTaskClicked);
        closeButton?.onClick.AddListener(() => addTaskPanel.SetActive(false));
        
        // Hide panel on start
        addTaskPanel.SetActive(false);
        
        // Load any existing tasks
        LoadTasks();
    }
    

    public void ToggleTaskPanel()
    {
        addTaskPanel.SetActive(true);

    }
    
    private void LoadTasks()
    {
        ClearTaskItems();
        
        // Get all tasks and create UI elements for them
        var tasks = TaskManager.Instance.GetAllTasks();
        
        foreach (var task in tasks)
        {
            CreateTaskItem(task);
        }
    }
    
    private void ClearTaskItems()
    {
        // Destroy all task items and clear the list
        foreach (var item in taskItems)
        {
            if (item != null)
            Destroy(item.gameObject);
        }
        taskItems.Clear();
    }
    
    private void CreateTaskItem(Task task)
    {
        // Instantiate and initialize the task item
        TaskItemUI taskItem = Instantiate(taskItemPrefab, taskListContent);
        taskItem.Initialize(task);
        
        // Subscribe to the completion event
        taskItem.OnTaskCompleted.AddListener(OnTaskCompleted);
        
        // Add to the tracking list
        taskItems.Add(taskItem);
    }
    
    private void OnTaskCompleted(TaskItemUI taskItem)
    {
        // Complete the task and refresh the list
        TaskManager.Instance.CompleteTask(taskItem.GetTaskId());
        
        // Optional: Play a sound or show an effect here
        Debug.Log($"Task completed: {taskItem.GetTaskId()}");
    }
    
    private void OnAddTaskClicked()
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(taskTitleInput.text) || 
            string.IsNullOrWhiteSpace(rewardInput.text))
        {
            Debug.LogWarning("Please fill in all required fields");
            return;
        }
        
        if (!int.TryParse(rewardInput.text, out int reward) || reward <= 0)
        {
            Debug.LogWarning("Please enter a valid positive number for the reward");
            return;
        }
        
        // Create and add the new task
        Task newTask = new Task(
            taskTitleInput.text.Trim(),
            taskDescriptionInput.text.Trim(),
            reward
        );
        
        TaskManager.Instance.AddTask(newTask);
        
        // Clear input fields
        taskTitleInput.text = "";
        taskDescriptionInput.text = "";
        rewardInput.text = "";
        
        // Refresh the task list
        LoadTasks();
        
        // Hide the add task panel
        if (addTaskPanel != null) 
        {
            addTaskPanel.SetActive(false);
        }
    }
    
    private void OnDestroy()
    {
        // Clean up event listeners
        addTaskButton.onClick.RemoveListener(OnAddTaskClicked);
        
        if (closeButton != null)
            closeButton.onClick.RemoveAllListeners();
            
        foreach (var item in taskItems)
        {
            if (item != null)
                item.OnTaskCompleted.RemoveAllListeners();
        }
    }
}

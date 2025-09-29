using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }
    
    [SerializeField] private List<Task> tasks = new List<Task>();
    private const string TASKS_SAVE_KEY = "player_tasks";
    public AudioSource audioSource;
    public AudioClip taskCompleteSound;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadTasks();
        
        // Add some default tasks if none exist
        if (tasks.Count == 0)
        {
            CreateDefaultTasks();
        }
    }
    
    private void CreateDefaultTasks()
    {
        // AddTask(new Task("First Steps", "Complete your first task", 10));
        // AddTask(new Task("Champion", "Complete 5 tasks", 50));
        // AddTask(new Task("Master of Tasks", "Complete 10 tasks", 100));
        SaveTasks();
    }
    
    public void AddTask(Task task)
    {
        if (!tasks.Exists(t => t.id == task.id))
        {
            tasks.Add(task);
            SaveTasks();
        }
    }
    
    public bool CompleteTask(string taskId)
    {
        Task task = tasks.Find(t => t.id == taskId);
        if (task != null && !task.isCompleted)
        {
            task.Complete();
            if (SoundManager.Instance != null && SoundManager.Instance.IsSoundEnabled && audioSource != null && taskCompleteSound != null)
            {
                audioSource.PlayOneShot(taskCompleteSound);
            }
            SaveTasks();
            return true;
        }
        return false;
    }
    
    public List<Task> GetAllTasks()
    {
        return new List<Task>(tasks);
    }
    
    public List<Task> GetIncompleteTasks()
    {
        return tasks.FindAll(task => !task.isCompleted);
    }
    
    public List<Task> GetCompletedTasks()
    {
        return tasks.FindAll(task => task.isCompleted);
    }
    
    private void SaveTasks()
    {
        string json = JsonUtility.ToJson(new TaskListWrapper(tasks));
        PlayerPrefs.SetString(TASKS_SAVE_KEY, json);
        PlayerPrefs.Save();
    }
    
    private void LoadTasks()
    {
        if (PlayerPrefs.HasKey(TASKS_SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(TASKS_SAVE_KEY);
            TaskListWrapper wrapper = JsonUtility.FromJson<TaskListWrapper>(json);
            if (wrapper != null)
            {
                tasks = wrapper.tasks;
            }
        }
    }
    
    [System.Serializable]
    private class TaskListWrapper
    {
        public List<Task> tasks;
        
        public TaskListWrapper(List<Task> tasks)
        {
            this.tasks = tasks;
        }
    }
}

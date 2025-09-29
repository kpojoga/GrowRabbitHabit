# Task System

## Overview
This system allows players to create and manage tasks in the game. When a task is marked as completed, the player receives a specified number of coins as a reward.

## Key Components

### 1. Task
- Represents a single task with a title, description, and coin reward
- Handles the completion logic and coin reward distribution

### 2. TaskManager
- Singleton that manages all tasks in the game
- Handles saving and loading tasks using PlayerPrefs
- Provides methods to add, complete, and query tasks

### 3. TaskUI
- Manages the task panel UI
- Allows players to view, add, and complete tasks
- Shows task details and rewards

## How to Use

### Creating a New Task
1. Press 'T' to open the task panel
2. Enter a title and description for the task
3. Set the coin reward amount
4. Click "Add Task"

### Completing a Task
1. Open the task panel ('T' key)
2. Toggle the checkbox next to a task to mark it as complete
3. The reward will be automatically added to the player's wallet

### Accessing Tasks Programmatically
```csharp
// Get all tasks
List<Task> allTasks = TaskManager.Instance.GetAllTasks();

// Get incomplete tasks
List<Task> incompleteTasks = TaskManager.Instance.GetIncompleteTasks();

// Complete a task by ID
TaskManager.Instance.CompleteTask(taskId);
```

## UI Setup
1. Create a Canvas if one doesn't exist
2. Create a panel for the task UI
3. Add the TaskUI script to the panel
4. Set up the required references in the inspector:
   - Task Panel (the main container)
   - Task Item Prefab (create using the "Task Item" menu item)
   - Content Transform (where tasks will be displayed)
   - Input fields for title, description, and reward
   - Add Task button

## Notes
- Tasks are automatically saved to PlayerPrefs when modified
- The system includes some default tasks if none exist
- Press 'T' to toggle the task panel

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateTaskUIPrefab : Editor
{
    [MenuItem("GameObject/UI/Task Item")]
    static void CreateTaskItem()
    {
        // Create a new GameObject with a Vertical Layout Group
        GameObject taskItem = new GameObject("TaskItem", typeof(RectTransform), typeof(Image), typeof(VerticalLayoutGroup), 
            typeof(ContentSizeFitter), typeof(LayoutElement), typeof(TaskItemUI));
        
        // Set up RectTransform
        RectTransform rectTransform = taskItem.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(400, 100);
        
        // Set up Image component for background
        Image background = taskItem.GetComponent<Image>();
        background.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        
        // Set up Vertical Layout Group
        VerticalLayoutGroup layout = taskItem.GetComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(15, 15, 10, 10);
        layout.spacing = 8f;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;
        
        // Set up Content Size Fitter
        ContentSizeFitter sizeFitter = taskItem.GetComponent<ContentSizeFitter>();
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        // Set up Layout Element
        LayoutElement layoutElement = taskItem.GetComponent<LayoutElement>();
        layoutElement.minHeight = 90f;
        layoutElement.preferredHeight = 110f;
        layoutElement.flexibleWidth = 1f;
        
        // Create Title Text
        TextMeshProUGUI titleText = CreateText("Title", taskItem.transform, "New Task", 22, FontStyles.Bold);
        titleText.alignment = TextAlignmentOptions.Left;
        
        // Create Description Text
        TextMeshProUGUI descriptionText = CreateText("Description", taskItem.transform, 
            "Task description goes here. This can be a longer description of what needs to be done.", 16);
        descriptionText.color = new Color(0.9f, 0.9f, 0.9f, 0.9f);
        
        // Create Bottom Row (Reward + Toggle)
        GameObject bottomRow = new GameObject("BottomRow", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        bottomRow.transform.SetParent(taskItem.transform, false);
        
        // Set up Bottom Row Layout
        HorizontalLayoutGroup hLayout = bottomRow.GetComponent<HorizontalLayoutGroup>();
        hLayout.childAlignment = TextAnchor.MiddleLeft;
        hLayout.childForceExpandWidth = true;
        hLayout.childForceExpandHeight = true;
        hLayout.childControlWidth = false;
        hLayout.childControlHeight = false;
        
        // Create Reward Text
        TextMeshProUGUI rewardText = CreateText("Reward", bottomRow.transform, "+10 coins", 16, FontStyles.Italic);
        rewardText.color = new Color(0.9f, 0.8f, 0.3f);
        
        // Create Spacer
        GameObject spacer = new GameObject("Spacer", typeof(RectTransform), typeof(LayoutElement));
        spacer.transform.SetParent(bottomRow.transform, false);
        var layoutElementSpacer = spacer.GetComponent<LayoutElement>();
        layoutElementSpacer.flexibleWidth = 1f;
        
        // Create Complete Toggle
        GameObject toggleObj = new GameObject("CompleteToggle", typeof(RectTransform), 
            typeof(Toggle), typeof(Image), typeof(LayoutElement));
        toggleObj.transform.SetParent(bottomRow.transform, false);
        
        // Set up Toggle component
        Toggle toggle = toggleObj.GetComponent<Toggle>();
        
        // Set up Toggle Background
        GameObject backgroundObj = new GameObject("Background", typeof(RectTransform), typeof(Image));
        backgroundObj.transform.SetParent(toggleObj.transform, false);
        
        // Set up Background Image
        Image bgImage = backgroundObj.GetComponent<Image>();
        bgImage.color = new Color(0.25f, 0.25f, 0.25f, 1f);
        
        // Set up Checkmark
        GameObject checkmark = new GameObject("Checkmark", typeof(RectTransform), typeof(Image));
        checkmark.transform.SetParent(backgroundObj.transform, false);
        
        // Set up Checkmark Image
        Image checkmarkImage = checkmark.GetComponent<Image>();
        checkmarkImage.color = new Color(0.2f, 0.8f, 0.2f, 1f);
        
        // Set up Toggle properties
        toggle.graphic = checkmarkImage;
        toggle.targetGraphic = bgImage;
        
        // Set up RectTransforms
        RectTransform toggleRect = toggleObj.GetComponent<RectTransform>();
        toggleRect.sizeDelta = new Vector2(24, 24);
        
        RectTransform bgRect = backgroundObj.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        
        RectTransform checkRect = checkmark.GetComponent<RectTransform>();
        checkRect.anchorMin = new Vector2(0.2f, 0.2f);
        checkRect.anchorMax = new Vector2(0.8f, 0.8f);
        checkRect.offsetMin = Vector2.zero;
        checkRect.offsetMax = Vector2.zero;
        
        // Set up TaskItemUI references
        TaskItemUI taskItemUI = taskItem.GetComponent<TaskItemUI>();
        
        // Create serialized object to set private fields
        SerializedObject serializedTaskItem = new SerializedObject(taskItemUI);
        serializedTaskItem.FindProperty("titleText").objectReferenceValue = titleText;
        serializedTaskItem.FindProperty("descriptionText").objectReferenceValue = descriptionText;
        serializedTaskItem.FindProperty("rewardText").objectReferenceValue = rewardText;
        serializedTaskItem.FindProperty("completionToggle").objectReferenceValue = toggle;
        serializedTaskItem.FindProperty("backgroundImage").objectReferenceValue = background;
        serializedTaskItem.ApplyModifiedProperties();
        
        // Make sure the prefab directory exists
        string prefabDir = "Assets/Prefabs/UI";
        if (!System.IO.Directory.Exists(prefabDir))
        {
            System.IO.Directory.CreateDirectory(prefabDir);
            AssetDatabase.Refresh();
        }
        
        // Create the prefab
        string prefabPath = $"{prefabDir}/TaskItem.prefab";
        bool success;
        PrefabUtility.SaveAsPrefabAsset(taskItem, prefabPath, out success);
        
        if (success)
        {
            Debug.Log($"Task Item prefab created successfully at: {prefabPath}");
            
            // Select the created prefab
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);
        }
        else
        {
            Debug.LogError("Failed to create Task Item prefab");
        }
        
        // Clean up
        DestroyImmediate(taskItem);
    }
    
    static TextMeshProUGUI CreateText(string name, Transform parent, string text, int fontSize, FontStyles style = FontStyles.Normal)
    {
        GameObject textObj = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
        textObj.transform.SetParent(parent, false);
        
        TextMeshProUGUI textComponent = textObj.GetComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = fontSize;
        textComponent.color = Color.white;
        textComponent.fontStyle = style;
        textComponent.enableWordWrapping = true;
        textComponent.overflowMode = TextOverflowModes.Ellipsis;
        textComponent.alignment = TextAlignmentOptions.Left;
        
        // Set up Layout Element
        LayoutElement layoutElement = textObj.GetComponent<LayoutElement>();
        layoutElement.preferredHeight = fontSize + 4;
        layoutElement.flexibleWidth = 1;
        
        // Set up RectTransform
        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0.5f, 1);
        rect.sizeDelta = new Vector2(0, fontSize + 4);
        
        return textComponent;
    }
}
#endif

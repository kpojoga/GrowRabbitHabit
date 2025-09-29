using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

// Elements for level window in which elements for level are displayed
public class LevelsManagerEditorWindow : EditorWindow
{
    public static LevelsManagerEditorWindow instance = null;

    static float WH = 76;
    static int _X = 6;
    static int _Y = 3;

    public static void Init()
    {
        if (instance == null)
        {
            instance = EditorWindow.GetWindowWithRect<LevelsManagerEditorWindow>(new Rect(0, 0, (WH + 4) * _X + 18, WH * _Y + 2), false, "Elements for level");
        }
        else
            instance.Focus();

        instance.Reset();
    }

    struct loadElTypeElement
    {
        public ElTypeElement ElTypeElements;
        public Texture2D icon;
    }

    List<loadElTypeElement> loadElTypeElements;
    ElTypeElement TypeElement = ElTypeElement.NONE;
    GUIStyle styleOn;
    Vector2 scroll;


    private void OnFocus()
    {
        loadElTypeElements = ForEnum.GetList().Select(v => { return new loadElTypeElement() { ElTypeElements = v, icon = ForEnum.GetIcon(v) }; }).ToList();
    }

    void OnGUI()
    {
        InitStyle();
        
        scroll = GUILayout.BeginScrollView(scroll, false, true);
        int _c = Mathf.Clamp((int)((position.width - 18) / (WH + 4)), 1, int.MaxValue);
        GUILayout.BeginHorizontal();
        for (int i = 0; i < loadElTypeElements.Count; i++)
        {
            bool _select = TypeElement == loadElTypeElements[i].ElTypeElements;

            if (GUILayout.Button(new GUIContent(loadElTypeElements[i].icon, ForEnum.GetTypeName(loadElTypeElements[i].ElTypeElements))
                , _select ? styleOn : GUI.skin.button
                , new GUILayoutOption[] { GUILayout.Width(WH), GUILayout.Height(WH) }))
            {
                if (_select)
                    TypeElement = ElTypeElement.NONE;
                else
                    TypeElement = loadElTypeElements[i].ElTypeElements;
            }

            if (i % _c == (_c - 1))
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
    }

    private void InitStyle()
    {
        if (styleOn != null)
            return;

        styleOn = new GUIStyle(GUI.skin.button);
        styleOn.normal = styleOn.active;
    }

    public void Reset()
    {
        TypeElement = ElTypeElement.NONE;

        Repaint();
    }

    public ElTypeElement GetElTypeElement()
    {
        return TypeElement;
    }
}

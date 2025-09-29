using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using UnityEditorInternal;

[CustomEditor(typeof(LevelsManager))]
public class LevelsManagerEditor : Editor
{
    int nowLevel;
    private LevelsManager _LevelsManager;
    private Level _Level;

    System.DateTime lastSaveTime; // 
    bool wasChange = false; // Do I need to save the level (were there any changes)

    //*************** Parameters for items *******************
    private Vector2 _Pos;
    private Quaternion _Rot;
    private float _R;
    private Vector2 _P1;
    private Vector2 _P2;
    private Quaternion _Rot1;
    private Quaternion _Rot2;
    private TypeSB _TypeSB;
    private TypeLR _TypeLR;
    private float _Float1;
    private int _Int1;
    private bool _Bool1;
    //********************************************************

    private ListFoldout _ListFoldout; // list with items added per level

    private Vector2 min_field;
    private Vector2 max_field;




    public void OnEnable()
    {
        EditorApplication.update += AutoSave;

        if (Application.isEditor && !Application.isPlaying)
        {
            LevelsManagerEditorWindow.Init();

            _LevelsManager = target as LevelsManager;
            LoadLevel();
        }
    }

    public void OnDisable()
    {
        EditorApplication.update -= AutoSave;

        if (Application.isEditor && !Application.isPlaying)
        {
            if (_LevelsManager != null)
                _LevelsManager.DropDrawLoadLevel(); 
        }
    }

    public void OnDestroy()
    {
        SaveLevel();
    }

    public override void OnInspectorGUI()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"), true);
            GUI.enabled = true;

            ShowLevelManipulation();
            ShowElementManipulation();
        }
    }

    // Level Control Buttons
    void ShowLevelManipulation()
    {
        int _EndLevel = _LevelsManager.CountLevels;
        nowLevel = MaxMin(nowLevel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Now Level:", EditorStyles.wordWrappedLabel, GUILayout.MinWidth(70));
        EditorGUILayout.LabelField(nowLevel.ToString(), EditorStyles.wordWrappedLabel);
        if (GUILayout.Button(new GUIContent("X", "Delete level"), GUILayout.MinWidth(20)))
        {
            DeleteLevel();
            LoadLevel();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("|<", "First level"), GUILayout.MinWidth(30)))
        {
            nowLevel = 0;
            LoadLevel();
        }
        if (GUILayout.Button(new GUIContent("<<", "Previous -10 level"), GUILayout.MinWidth(30)))
        {
            nowLevel = MaxMin(nowLevel - 10);
            LoadLevel();
        }
        if (GUILayout.Button(new GUIContent("<", "Previous level"), GUILayout.MinWidth(20)))
        {
            nowLevel = MaxMin(nowLevel - 1);
            LoadLevel();
        }
        if (GUILayout.Button(new GUIContent(">", "Next level"), GUILayout.MinWidth(20)))
        {
            nowLevel = MaxMin(nowLevel + 1);
            LoadLevel();
        }
        if (GUILayout.Button(new GUIContent(">>", "Next +10 level"), GUILayout.MinWidth(30)))
        {
            nowLevel = MaxMin(nowLevel + 10);
            LoadLevel();
        }
        if (GUILayout.Button(new GUIContent(">|", "End level"), GUILayout.MinWidth(30)))
        {
            nowLevel = _EndLevel;
            LoadLevel();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("+ before", "Add level before current level"), GUILayout.MinWidth(60)))
        {
            AddLevel((nowLevel <= 0) ? 1 : nowLevel);
            LoadLevel();
        }
        if (GUILayout.Button(new GUIContent("+ after", "Add level after current level"), GUILayout.MinWidth(50)))
        {
            AddLevel((nowLevel <= 0) ? 1 : nowLevel + 1);
            LoadLevel();
        }
        if (GUILayout.Button(new GUIContent("+ end", "Add level to end"), GUILayout.MinWidth(40)))
        {
            AddLevel(_EndLevel + 1);
            LoadLevel();
        }
        EditorGUILayout.EndHorizontal();
    }

    // Level Control
    void ShowElementManipulation()
    {
        if (nowLevel > 0)
        {
            GuiLine();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();
            float _labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 15;
            EditorGUILayout.LabelField("Size", GUILayout.MinWidth(60));
            float _LevelSizeX = EditorGUILayout.FloatField("W", _Level.Size.x, GUILayout.MinWidth(60));
            float _LevelSizeY = EditorGUILayout.FloatField("H", _Level.Size.y, GUILayout.MinWidth(60));
            EditorGUIUtility.labelWidth = _labelWidth;
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                _Level.Size.x = _LevelSizeX;
                _Level.Size.y = _LevelSizeY;

                _LevelsManager.RepaintLevel(_Level.Size);

                updateMinMax();

                wasChange = true;
            }

            _ListFoldout.DrawLayout();

            if (_ListFoldout.index>=0)
            {
                EditorGUI.BeginChangeCheck();

                BaseElLevel_XML item = _Level.Elements[_ListFoldout.index];

                Vector2 _Pos2 = RangePos(EditorGUILayout.Vector2Field(item.GetNameVal(TypeVal.Position), _Pos));
                bool _edit = false;
                if (_Pos2 != _Pos)
                {
                    _Pos = _Pos2;
                    _edit = true;
                }

                _Rot.eulerAngles = new Vector3(0, 0, EditorGUILayout.FloatField(item.GetNameVal(TypeVal.Rotate), _Rot.eulerAngles.z));
                if (item.HasVal(TypeVal.typeSB))
                {
                    _TypeSB = (TypeSB)EditorGUILayout.EnumPopup(item.GetNameVal(TypeVal.typeSB), _TypeSB);
                }
                if (item.HasVal(TypeVal.typeLR))
                {
                    _TypeLR = (TypeLR)EditorGUILayout.EnumPopup(item.GetNameVal(TypeVal.typeLR), _TypeLR);
                }
                if (item.HasVal(TypeVal.Point1))
                {
                    _P1 = EditorGUILayout.Vector2Field(item.GetNameVal(TypeVal.Point1), _P1);
                }
                if (item.HasVal(TypeVal.Rotate1))
                {
                    _Rot1.eulerAngles = new Vector3(0, 0, EditorGUILayout.FloatField(item.GetNameVal(TypeVal.Rotate1), _Rot1.eulerAngles.z));
                }
                if (item.HasVal(TypeVal.Point2))
                {
                    _P2 = EditorGUILayout.Vector2Field(item.GetNameVal(TypeVal.Point2), _P2);
                }
                if (item.HasVal(TypeVal.Rotate2))
                {
                    _Rot2.eulerAngles = new Vector3(0, 0, EditorGUILayout.FloatField(item.GetNameVal(TypeVal.Rotate2), _Rot2.eulerAngles.z));
                }
                if (item.HasVal(TypeVal.Radius))
                {
                    _R = EditorGUILayout.FloatField(item.GetNameVal(TypeVal.Radius), _R);
                }
                if (item.HasVal(TypeVal.Float1))
                {
                    _Float1 = EditorGUILayout.FloatField(item.GetNameVal(TypeVal.Float1), _Float1);
                }
                if (item.HasVal(TypeVal.Int1))
                {
                    _Int1 = EditorGUILayout.IntField(item.GetNameVal(TypeVal.Int1), _Int1);
                }
                if (item.HasVal(TypeVal.Bool1))
                {
                    _Bool1 = EditorGUILayout.ToggleLeft(item.GetNameVal(TypeVal.Bool1), _Bool1);
                }

                if (EditorGUI.EndChangeCheck() || _edit)
                {
                    RefreshValuesNowElement();
                }
            }

            GuiLine();
        }
        else
        {
            _LevelsManager.DropDrawLoadLevel();
        }

        
    }

    public void OnSceneGUI()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            if (_Level != null)
            {
                foreach (BaseElLevel_XML lbe in _Level.Elements)
                {
                    if (_ListFoldout.index >= 0 && lbe._BaseElLevel == _Level.Elements[_ListFoldout.index]._BaseElLevel)
                    {
                        EditorGUI.BeginChangeCheck();

                        Vector2 _Pos2 = RangePos(Handles.PositionHandle(_Pos, Quaternion.identity));
                        bool _edit = false;
                        if(_Pos2 != _Pos)
                        {
                            _Pos = _Pos2;
                            _edit = true;
                        }
                        
                        Handles.color = Handles.zAxisColor;
                        _Rot = Handles.Disc(_Rot, _Pos, Vector3.forward, HandleUtility.GetHandleSize(_Pos) * 1.2f, false, 0.01f);

                        if (lbe.HasVal(TypeVal.Point1))
                        {
                            Handles.color = Color.green;
                            var fmh_283_125_638947612047204670 = Quaternion.identity; _P1 = Quaternion.Inverse(_Rot) * ((Vector2)Handles.FreeMoveHandle(_Pos + (Vector2)(_Rot * _P1), HandleUtility.GetHandleSize(_Pos + (Vector2)(_Rot * _P1)) / 5, Vector3.one * 0.1f, Handles.CubeHandleCap) - _Pos);
                        }
                        if (lbe.HasVal(TypeVal.Point2))
                        {
                            Handles.color = Color.green;
                            var fmh_288_125_638947612047212560 = Quaternion.identity; _P2 = Quaternion.Inverse(_Rot) * ((Vector2)Handles.FreeMoveHandle(_Pos + (Vector2)(_Rot * _P2), HandleUtility.GetHandleSize(_Pos + (Vector2)(_Rot * _P2)) / 5, Vector3.one * 0.1f, Handles.CubeHandleCap) - _Pos);
                        }
                        if (lbe.HasVal(TypeVal.Rotate1))
                        {
                            Handles.color = Handles.zAxisColor;
                            _Rot1 = Handles.Disc(_Rot1, _Pos + (Vector2)(_Rot * _P1), Vector3.forward, HandleUtility.GetHandleSize(_Pos + (Vector2)(_Rot * _P1)) / 2, false, 0.01f);
                        }
                        if (lbe.HasVal(TypeVal.Rotate2))
                        {
                            Handles.color = Handles.zAxisColor;
                            _Rot2 = Handles.Disc(_Rot2, _Pos + (Vector2)(_Rot * _P2), Vector3.forward, HandleUtility.GetHandleSize(_Pos + (Vector2)(_Rot * _P2)) / 2, false, 0.01f);
                        }
                        if (lbe.HasVal(TypeVal.Radius))
                        {
                            Handles.color = Color.green;
                            Vector3 RadiusVector = new Vector3(0f, _R, 0f);
                            for (int i = 0; i < 4; i++)
                            {
                                Vector3 oldPoint = (Vector3)_Pos + Quaternion.Euler(0f, 0f, 90f * i) * RadiusVector;
                                var fmh_307_85_638947612047214870 = Quaternion.identity; Vector3 newPoint = Handles.FreeMoveHandle(oldPoint, HandleUtility.GetHandleSize(oldPoint) / 5, Vector3.one * 0.1f, Handles.SphereHandleCap);
                                if (oldPoint != newPoint)
                                {
                                    _R = Vector3.Magnitude(newPoint - (Vector3)_Pos);
                                }
                            }
                        }
                        if (EditorGUI.EndChangeCheck() || _edit)
                        {
                            RefreshValuesNowElement();
                            Repaint();
                        }
                    }
                    else
                    {
                        Handles.color = Color.red;
                        Vector2 ___pos = RangePos(lbe.Position);
                        if(___pos != lbe.Position)
                        {
                            lbe.SetVal(TypeVal.Position, ___pos);
                            lbe.SetValuesGameObject();
                            lbe._BaseElLevel.Draw();

                            lastSaveTime = System.DateTime.Now;
                            wasChange = true;
                        }

                        if (Handles.Button(lbe.Position, Quaternion.identity, HandleUtility.GetHandleSize(lbe.Position) / 5, 0.1f, Handles.SphereHandleCap))
                        {
                            int ind_sel = -1;
                            int i = 0;
                            foreach (BaseElLevel_XML tmp_lbe in _Level.Elements)
                            {
                                if (tmp_lbe._BaseElLevel == lbe._BaseElLevel)
                                {
                                    ind_sel = i;
                                    break;
                                }
                                i++;
                            }
                            if (ind_sel >= 0)
                            {
                                SelectElement(ind_sel);

                                Repaint();
                            }
                        }
                    }
                }


                Event current = Event.current;
                int controlID = GUIUtility.GetControlID(FocusType.Passive);

                switch (current.type)
                {
                    case EventType.MouseUp:
                        if (current.button == 0)
                        {
                            Vector3 spawnPosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
                            AddElement(spawnPosition);

                            GUI.changed = true;
                            current.Use();
                        }
                        break;

                    case EventType.Layout:
                        HandleUtility.AddDefaultControl(controlID);
                        break;
                }
            }
        }
    }

    void AddLevel(int newLevel)
    {
        for (int i = _LevelsManager.CountLevels; i >= newLevel; i--)
        {
            File.Move(_LevelsManager.getPath(i), _LevelsManager.getPath(i + 1));
        }
        nowLevel = newLevel;
        DefaultNewLevel();
        SaveLevel();
    }

    void DeleteLevel()
    {
        int _count = _LevelsManager.CountLevels;

        if (_count > 0)
        {
            File.Delete(_LevelsManager.getPath(nowLevel)); // Delete now Level

            for (int i = nowLevel + 1; i <= _count; i++)
            {
                File.Move(_LevelsManager.getPath(i), _LevelsManager.getPath(i - 1));
            }
            nowLevel = MaxMin(nowLevel);
            AssetDatabase.Refresh();
        }
    }

    // Default items for a new level (these items cannot be deleted)
    void DefaultNewLevel()
    {
        _Level = new Level();

        _Level.Size = new Vector2(4, 10);

        _Level.Elements = new List<BaseElLevel_XML>();
        _Level.Elements.Add(new ElLevelSandCutCircle_XML() { Position = new Vector2(0, 0), Radius = 1.1f, CanRemove = false });
        _Level.Elements.Add(new ElLevelBalls_XML() { Position = new Vector2(0, 0), Radius = 1, CanRemove = false });
        _Level.Elements.Add(new ElLevelSand_XML() { Position = new Vector2(0, -2), Point1 = new Vector2(2, 2), CanRemove = false });
    }

    void AddElement(Vector2 pos)
    {
        BaseElLevel_XML item = ForEnum.GetTypeXML(LevelsManagerEditorWindow.instance.GetElTypeElement());

        if (item != null)
        {
            item.Default();
            item.SetVal(TypeVal.Position, pos);
            _LevelsManager.AddElementGameObject(item);
            _Level.Elements.Add(item);

            _ListFoldout.index = _ListFoldout.count - 1;

            GetValuesNowElement();
        }
    }

    void AutoSave()
    {
        if (wasChange)
        {
            System.TimeSpan timeDifference = System.DateTime.Now.Subtract(lastSaveTime);

            if (timeDifference.Seconds > 1)
            {
                lastSaveTime = System.DateTime.Now;
                wasChange = false;

                SaveLevel();
            }
        }
    }

    void SaveLevel()
    {
        if (Application.isEditor && !Application.isPlaying
            && _Level != null)
        {
            string file_name = _LevelsManager.getPath(nowLevel);
            if (File.Exists(file_name))
                File.Delete(file_name);

            XmlSerializer serializer = new XmlSerializer(_Level.GetType());
            FileStream stream = new FileStream(file_name, FileMode.Create);
            TextWriter textWriter = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(textWriter, _Level);
            textWriter.Close();
            stream.Dispose();
            stream.Close();

            AssetDatabase.Refresh();
        }
    }

    int MaxMin(int new_level)
    {
        int _count = _LevelsManager.CountLevels;
        int _l = Mathf.Clamp(new_level, 1, _count);
        _l = (_l <= 0 || _count == 0) ? 0 : _l;

        return _l;
    }

    void GuiLine(int i_height = 1)
    {
        EditorGUILayout.Separator();
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        EditorGUILayout.Separator();
    }

    void LoadLevel()
    {
        wasChange = false;
        nowLevel = MaxMin(nowLevel);
        if (nowLevel > 0)
        {
            _Level = _LevelsManager.LoadLevel(nowLevel);

            _ListFoldout = new ListFoldout(_Level.Elements, "Elements");

            _ListFoldout.onSelectCallback = (int id) =>
            {
                GetValuesNowElement(); 
            };

            _ListFoldout.onCanRemoveCallback = (int id) =>
            {
                return (id >= _Level.Elements.Count) ? false : _Level.Elements[id].CanRemove;
            };

            _ListFoldout.onRemoveCallback = (int id) =>
            {
                if (id < _Level.Elements.Count)
                {
                    BaseElLevel_XML item = _Level.Elements[id];
                    if (item._BaseElLevel)
                        DestroyImmediate(item._BaseElLevel.gameObject);

                    _Level.Elements.RemoveAt(id);

                    GetValuesNowElement();
                    wasChange = true;
                }
            };
        }
        else
        {
            _Level = null;
        }
    }

    void SelectElement(int ind)
    {
        _ListFoldout.index = ind;
        GetValuesNowElement();
    }

    void updateMinMax()
    {
        max_field = new Vector2(_Level.Size.x / 2, 0);
        min_field = new Vector2(-_Level.Size.x / 2, -_Level.Size.y);
    }

    Vector2 RangePos(Vector2 v2)
    {
        updateMinMax();

        float new_x = v2.x;
        float new_y = v2.y;

        if (new_x < min_field.x)
            new_x = min_field.x;
        if (new_x > max_field.x)
            new_x = max_field.x;

        if (new_y < min_field.y)
            new_y = min_field.y;
        if (new_y > max_field.y)
            new_y = max_field.y;

        return new Vector2(new_x, new_y);
    }

    // We update all the necessary parameters for the selected item.
    void RefreshValuesNowElement()
    {
        if (_ListFoldout.index >= 0)
        {
            BaseElLevel_XML item = _Level.Elements[_ListFoldout.index];
            item.SetVal(TypeVal.Position, _Pos);
            item.SetVal(TypeVal.Rotate, _Rot.eulerAngles.z);
            item.SetVal(TypeVal.Point1, _P1);
            item.SetVal(TypeVal.Point2, _P2);
            item.SetVal(TypeVal.Rotate1, _Rot1.eulerAngles.z);
            item.SetVal(TypeVal.Rotate2, _Rot2.eulerAngles.z);
            item.SetVal(TypeVal.Radius, _R);
            item.SetVal(TypeVal.typeSB, _TypeSB);
            item.SetVal(TypeVal.typeLR, _TypeLR);
            item.SetVal(TypeVal.Float1, _Float1);
            item.SetVal(TypeVal.Int1, _Int1);
            item.SetVal(TypeVal.Bool1, _Bool1);
            item.SetValuesGameObject();
            item._BaseElLevel.Draw();


            lastSaveTime = System.DateTime.Now;
            wasChange = true;
        }
    }

    // Returns all available parameters for the selected item.
    void GetValuesNowElement()
    {
        if (_ListFoldout.index >= 0)
        {
            BaseElLevel_XML item = _Level.Elements[_ListFoldout.index];
            _Pos = item.HasVal(TypeVal.Position) ? (Vector2)item.GetVal(TypeVal.Position) : Vector2.zero;
            _Rot = Quaternion.Euler(0, 0, item.HasVal(TypeVal.Rotate) ? (float)item.GetVal(TypeVal.Rotate) : 0);
            _P1 = item.HasVal(TypeVal.Point1) ? (Vector2)item.GetVal(TypeVal.Point1) : Vector2.zero;
            _P2 = item.HasVal(TypeVal.Point2) ? (Vector2)item.GetVal(TypeVal.Point2) : Vector2.zero;
            _Rot1 = Quaternion.Euler(0, 0, item.HasVal(TypeVal.Rotate1) ? (float)item.GetVal(TypeVal.Rotate1) : 0);
            _Rot2 = Quaternion.Euler(0, 0, item.HasVal(TypeVal.Rotate2) ? (float)item.GetVal(TypeVal.Rotate2) : 0);
            _R = item.HasVal(TypeVal.Radius) ? (float)item.GetVal(TypeVal.Radius) : 0;
            _TypeSB = item.HasVal(TypeVal.typeSB) ? (TypeSB)item.GetVal(TypeVal.typeSB) : TypeSB.SMALL;
            _TypeLR = item.HasVal(TypeVal.typeLR) ? (TypeLR)item.GetVal(TypeVal.typeLR) : TypeLR.RIGHT;
            _Float1 = item.HasVal(TypeVal.Float1) ? (float)item.GetVal(TypeVal.Float1) : 0;
            _Int1 = item.HasVal(TypeVal.Int1) ? (int)item.GetVal(TypeVal.Int1) : 0;
            _Bool1 = item.HasVal(TypeVal.Bool1) ? (bool)item.GetVal(TypeVal.Bool1) : false;

            item.SetValuesGameObject();
            item._BaseElLevel.Draw();
        }
    }
}

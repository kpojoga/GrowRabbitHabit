using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

// Creates custom list in the inspector
public class ListFoldout
{
    class str_ElTypeElement
    {
        public ElTypeElement _ElTypeElement;
        public bool isExpanded;
    }
    List<str_ElTypeElement> _ElTypeElement;
    List<BaseElLevel_XML> _BaseElLevel_XML;
    Dictionary<ElTypeElement, List<int>> _list;
    string _NameHeader = "";

    int _index;
    public int index
    {
        set
        {
            float _y = 0;
            foreach (str_ElTypeElement _t in _ElTypeElement)
            {
                if (_list[_t._ElTypeElement].Count > 0)
                {
                    _y += rowHeight;
                    if (!_t.isExpanded && _list[_t._ElTypeElement].Contains(value))
                    {
                        _t.isExpanded = true;
                    }
                    if (_t.isExpanded)
                    {
                        if (_list[_t._ElTypeElement].Contains(value))
                        {
                            _y += _list[_t._ElTypeElement].IndexOf(value) * rowHeight;
                            break;
                        }
                    }
                }
            }

            _scroll_v2 = new Vector2(_scroll_v2.x, _y);

            _index = value;
        }
        get
        {
            if (_index >= count)
                _index = count - 1;
            return _index;
        }
    }

    int _count
    {
        get
        {
            int _c = 0;

            foreach (str_ElTypeElement _e in _ElTypeElement)
            {
                _c += _list[_e._ElTypeElement].Count;
            }

            return _c;
        }
    }
    public int count
    {
        get
        {
            updateList();

            return _BaseElLevel_XML.Count;
        }
    }

    private float _drawx;
    private float _drawWidth;
    private float _drawY;
    private float actHeight = 205;
    private int _controlID;
    private Vector2 _scroll_v2;
    private Texture2D _PrefabOverlayRemoved;

    private int tmp_id;
    private bool isSelect;
    private bool isRemove;

    float rowHeight
    {
        get { return EditorGUIUtility.singleLineHeight + 1; }
    }


    public CanRemoveCallbackDelegate onCanRemoveCallback;
    public RemoveCallbackDelegate onRemoveCallback;
    public SelectCallbackDelegate onSelectCallback;

    public delegate bool CanRemoveCallbackDelegate(int id);
    public delegate void RemoveCallbackDelegate(int id);
    public delegate void SelectCallbackDelegate(int id);



    public ListFoldout(List<BaseElLevel_XML> baseElLevel_XML, string NameHeader)
    {
        _ElTypeElement = ForEnum.GetList().Select(v => { return new str_ElTypeElement() { _ElTypeElement = v, isExpanded = false }; }).ToList();
        _BaseElLevel_XML = baseElLevel_XML;
        _NameHeader = NameHeader;

        _PrefabOverlayRemoved = (Texture2D)EditorGUIUtility.Load("Toolbar Minus");

        _list = new Dictionary<ElTypeElement, List<int>>();
        foreach (str_ElTypeElement _e in _ElTypeElement)
        {
            _list.Add(_e._ElTypeElement, new List<int>());
        }
        
        for (int i = 0; i < _BaseElLevel_XML.Count; i++)
        {
            _list[_BaseElLevel_XML[i].TypeElement].Add(i);
        }

        _index = -1;

        onCanRemoveCallback = (int id) => { return true; };
        onRemoveCallback = (int id) => {  };
        onSelectCallback = (int id) => {  };
    }

    void updateList()
    {
        if (_BaseElLevel_XML.Count != _count)
        {
            foreach (str_ElTypeElement _e in _ElTypeElement)
            {
                _list[_e._ElTypeElement].Clear();
            }

            for (int i = 0; i < _BaseElLevel_XML.Count; i++)
            {
                _list[_BaseElLevel_XML[i].TypeElement].Add(i);
            }
        }
    }

    public void DrawLayout()
    {
        updateList();

        isSelect = false;
        isRemove = false;
        tmp_id = -1;

        EditorGUILayout.Separator();

        float _height = 0;

        foreach (str_ElTypeElement _t in _ElTypeElement)
        {
            if (_list[_t._ElTypeElement].Count > 0)
            {
                _height += rowHeight;
                if (_t.isExpanded)
                {
                    _height += _list[_t._ElTypeElement].Count * rowHeight;
                }
            }
        }

        Rect _controlRect = EditorGUILayout.GetControlRect(true, actHeight);

        EditorGUI.LabelField(_controlRect, _NameHeader, GUI.skin.window);
        _controlRect.x += 1;
        _controlRect.width -= 1;
        _controlRect.y += rowHeight;
        _controlRect.height -= rowHeight + 1;

        _drawx = _controlRect.x;
        _drawWidth = _controlRect.width - _controlRect.x;
        _drawY = _controlRect.y;

        _scroll_v2 = GUI.BeginScrollView(_controlRect, _scroll_v2, new Rect(_drawx, _drawY, _drawWidth, _height), false, true);

        _controlID = GUIUtility.GetControlID(FocusType.Passive, new Rect(_drawx, _drawY, _drawWidth, actHeight));

        foreach (str_ElTypeElement _t in _ElTypeElement)
        {
            if (_list[_t._ElTypeElement].Count > 0)
            {
                Rect rowRect = new Rect(_drawx, _drawY, _drawWidth, rowHeight - 1);
                Rect indentRect = new Rect(_drawx + 12, _drawY, _drawWidth - 12, rowHeight - 1);

                EditorGUI.DrawRect(indentRect, _t.isExpanded ? Color.white : Color.gray);
                _t.isExpanded = EditorGUI.Foldout(indentRect, _t.isExpanded, GUIContent.none, true);
                EditorGUI.LabelField(indentRect, ForEnum.GetTypeName(_t._ElTypeElement), _t.isExpanded ? EditorStyles.boldLabel : EditorStyles.whiteBoldLabel);

                _drawY += rowHeight;

                if (_t.isExpanded)
                {
                    foreach (int id in _list[_t._ElTypeElement])
                    {
                        OnDrawRow(id);
                    }
                }
            }
        }

        GUI.EndScrollView();

        EditorGUILayout.Separator();

        if (isSelect)
        {
            Select(tmp_id);
        }
        if (isRemove)
        {
            Remove(tmp_id);
        }
    }

    void OnDrawRow(int id)
    {
        float rowIn = 14;
        bool canRemove = onCanRemoveCallback.Invoke(id);

        Rect rowRect = new Rect(_drawx, _drawY, _drawWidth, rowHeight- 1);
        Rect indentRect = new Rect(_drawx + rowIn, _drawY, _drawWidth - rowIn - (canRemove ? 25 : 0), rowHeight);
        Rect indentRectRemove = new Rect(indentRect.x + indentRect.width, _drawY, (canRemove ? 25 : 0), rowHeight - 1);

        if (_index == id)
        {
            EditorGUI.DrawRect(rowRect, new Color(0.24609375f, 0.48828125f, 0.90234375f));
        }

        EditorGUI.LabelField(indentRect, new GUIContent("id:" + id + " - " + ForEnum.GetTypeName(_BaseElLevel_XML[id].TypeElement)), _index == id ? EditorStyles.whiteLabel : EditorStyles.label);
        if (canRemove)
        {
            GUI.DrawTexture(indentRectRemove, _PrefabOverlayRemoved, ScaleMode.ScaleToFit, true, 0);
        }

        EventType eventType = Event.current.GetTypeForControl(_controlID);
        if (eventType == EventType.MouseUp && indentRect.Contains(Event.current.mousePosition))
        {
            isSelect = true;
            tmp_id = id;

            GUI.changed = true;
            Event.current.Use();
        }
        if (eventType == EventType.MouseUp && indentRectRemove.Contains(Event.current.mousePosition))
        {
            isRemove = true;
            tmp_id = id;

            GUI.changed = true;
            Event.current.Use();
        }

        _drawY += rowHeight;
    }


    void Select(int id)
    {
        _index = id;

        onSelectCallback.Invoke(id);
    }

    void Remove(int id)
    {
        _list[_BaseElLevel_XML[id].TypeElement].Remove(id);

        foreach (str_ElTypeElement _t in _ElTypeElement)
        {
            for (int i = 0; i < _list[_t._ElTypeElement].Count; i++)
            {
                if (_list[_t._ElTypeElement][i] > id)
                    _list[_t._ElTypeElement][i] -= 1;
            }
        }

        if (onCanRemoveCallback.Invoke(id))
        {
            onRemoveCallback.Invoke(id);
        }

        if (_index >= _count)
            _index = _count - 1;
    }
}

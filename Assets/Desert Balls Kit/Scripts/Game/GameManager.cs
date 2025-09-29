using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject PS_sand; // particle system with sand
    public GameObject S_dig_sand; // Sand Digging Sound

    bool isMouseStart = false; // clamped the mouse / finger on the screen
    Vector3 oldXY; // old position
    float R = 0.5f; // sand cutting radius

    private bool isEndGame; // Is the game over
    private bool isStartGame; // Was the game started

    private float tRestartLevel;
    private float max_tRestartLevel = 15; // time after which the message about the level restart appears if you idle in the game for a long time (in seconds)

    private int LevelDiamonds = 0; // the number of diamonds earned per game

    private List<ElLevelSand> Sands = new List<ElLevelSand>(); // sand list
    private Vector2 SizeField = Vector2.zero;

    private GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;
    private EventSystem m_EventSystem;


    void Awake()
    {
        try
        {
            if (!instance)
            {
                instance = this;
                DontDestroyOnLoad(instance);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message + " Stack: " + e.StackTrace);
        }

        isEndGame = false;
        isStartGame = false;
        tRestartLevel = 0;
        LoadLevel(LevelsManager.instance.LoadLevel(GameSettings.getNowLevel()));

        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        m_EventSystem = FindObjectOfType<EventSystem>();
    }

    private void FixedUpdate()
    {
        if (isStartGame && !isEndGame)
        {
            if (Input.GetMouseButton(0) && !HasUI(Input.mousePosition)) // here we determine the position of the mouse in space
            {
                Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float _d = Mathf.Abs(Vector3.Dot(transform.position, transform.forward));
                float _a = (-_d - Vector3.Dot(transform.forward, cam_ray.origin)) / Vector3.Dot(transform.forward, cam_ray.direction);
                Vector3 XY = Expantions.Round(cam_ray.origin + cam_ray.direction * _a);
                Vector3 startXY;
                if (oldXY != XY)
                {
                    if (isMouseStart)
                    {
                        float dist = Vector3.Distance(oldXY, XY);
                        if (dist > R / 5)
                            startXY = oldXY;
                        else
                            startXY = XY;
                    }
                    else
                        startXY = XY;
                    bool isEdit = false;
                    foreach (ElLevelSand ob in Sands)
                    {
                        if (ob.AddContour(startXY, XY, R))
                            isEdit = true;
                    }
                    if (isEdit)
                    {
                        Instantiate(PS_sand, startXY, Quaternion.identity);
                        Instantiate(S_dig_sand, startXY, Quaternion.identity);
                    }
                    oldXY = XY;
                }

                isMouseStart = true;
            }
            else
                isMouseStart = false;
        }
    }

    void Update()
    {
        if (isStartGame && !isEndGame)
        {
            tRestartLevel += Time.deltaTime;
            if (tRestartLevel >= max_tRestartLevel)
            {
                tRestartLevel = 0;
                Menus.instance.ShowRestart();
            }
        }

        if (Input.anyKey || Input.touchCount > 0 || Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            tRestartLevel = 0;
        }
    }

    bool HasUI(Vector2 pos)
    {
        bool b = false;

        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = pos;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        if (results.Count > 0)
            b = true;

        return b;
    }

    public void StartGame()
    {
        isEndGame = false;
        isStartGame = true;
        tRestartLevel = 0;
        LevelDiamonds = 0;
    }

    public void RestartGame()
    {
        isEndGame = false;
        isStartGame = false;
        tRestartLevel = 0;
        LevelDiamonds = 0;
        LoadLevel(LevelsManager.instance.LoadLevel(GameSettings.getNowLevel()));
    }

    public void NextLevel()
    {
        int lvl = GameSettings.getNowLevel() + 1;
        if (lvl > LevelsManager.instance.CountLevels)
            lvl = 1;
        GameSettings.setNowLevel(lvl);
    }

    public void AddLevelDiamonds(int d)
    {
        LevelDiamonds += d;
    }

    public void EndGame()
    {
        isEndGame = true;
        Menus.instance.ShowEndGame(LevelDiamonds);
    }

    void LoadLevel(Level _L)
    {
        SizeField = _L.Size;
        moveCam.instance.Restart();

        Sands.Clear();
        foreach (BaseElLevel_XML _e in _L.Elements)
        {
            if (_e.TypeElement == ElTypeElement.Sand)
                Sands.Add(_e._BaseElLevel.GetComponentInChildren<ElLevelSand>());
        }
    }

    public Vector2 GetSizeField()
    {
        return SizeField;
    }
}

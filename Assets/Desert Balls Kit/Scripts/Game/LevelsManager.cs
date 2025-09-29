using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Text;

public class Level
{
    public Vector2 Size;
    public List<BaseElLevel_XML> Elements;
}

public class LevelsManager : MonoBehaviour
{
    public static LevelsManager instance = null;
    
    private ElLevelBackground eb_all;
    private ElLevelBackground eb;
    private ElLevelEndGame eeg;
    private ElLevelBoard ebL;
    private ElLevelBoard ebR;

    private List<ElLevelSand> Sands = new List<ElLevelSand>();


    public int CountLevels // the number of levels created in the game
    {
        get
        {
            TextAsset mapText = null;
            for (int i = 1; i < int.MaxValue; i++)
            {
                mapText = Resources.Load("Levels/" + i) as TextAsset;
                if (mapText == null)
                {
                    return i - 1;
                }
            }
            return 0;
        }
    }



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
    }

    // path to file with level
    public string getPath(int i)
    {
        string activeDirLevels = Application.dataPath + @"/Desert Balls Kit/Resources/Levels/";
        if (!Directory.Exists(activeDirLevels))
        {
            Directory.CreateDirectory(activeDirLevels);
        }
        return Path.Combine(activeDirLevels, i + ".txt");
    }

    // Level loading from file
    public Level LoadLevel(int nowLevel)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Level));
        TextAsset textAsset = (TextAsset)Resources.Load("Levels/" + nowLevel);
        StringReader stread = new StringReader(textAsset.text);
        Level _Level = (Level)serializer.Deserialize(stread);
        stread.Close();

        DrawLoadLevel(_Level);

        return _Level;
    }

    // We remove all the elements of the old level
    public void DropDrawLoadLevel()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (Application.isEditor && !Application.isPlaying)
                DestroyImmediate(transform.GetChild(i).gameObject);
            else
                Destroy(transform.GetChild(i).gameObject);
        }

        foreach (GameBall gb in FindObjectsOfType<GameBall>())
        {
            if (Application.isEditor && !Application.isPlaying)
                DestroyImmediate(gb.gameObject);
            else
                Destroy(gb.gameObject);
        }
    }

    // Level loading
    public void DrawLoadLevel(Level _Level)
    {
        DropDrawLoadLevel();

        CreateBackground(_Level.Size);

        CreateElements(_Level);
    }

    // create all the elements of the backhandle level
    void CreateBackground(Vector2 size)
    {
        GameObject go = (GameObject)Resources.Load("ElementsLevel/ElLevelBackgroundAll");
        eb_all = Instantiate(go, transform).GetComponent<ElLevelBackground>();

        go = (GameObject)Resources.Load("ElementsLevel/ElLevelBackground");
        eb = Instantiate(go, transform).GetComponent<ElLevelBackground>();

        go = (GameObject)Resources.Load("ElementsLevel/ElLevelEndGame");
        eeg = Instantiate(go, transform).GetComponent<ElLevelEndGame>();

        go = (GameObject)Resources.Load("ElementsLevel/ElLevelBoard");
        ebL = Instantiate(go, transform).GetComponent<ElLevelBoard>();
        ebR = Instantiate(go, transform).GetComponent<ElLevelBoard>();

        RepaintLevel(size);
    }

    // draw the created elements of the back handicap level
    public void RepaintLevel(Vector2 size)
    {
        eb_all.Position = new Vector3(0, -(size.y - 5.5f) / 2, 0);
        eb_all.Size = (size + new Vector2(0, 5.5f)) * 3;
        eb_all.Draw();

        eb.Position = new Vector3(0, -(size.y - 5.5f) / 2, 0);
        eb.Size = size + new Vector2(0, 5.5f);
        eb.Draw();

        eeg.Position = new Vector3(0, -size.y + 0.25f, 0);
        eeg.Draw();

        Vector2 sizeBoard = new Vector2(1, size.y + 6);

        ebL.Position = new Vector3(-(size.x + sizeBoard.x) / 2, -(size.y - 5.5f) / 2, 0);
        ebL.Size = sizeBoard;
        ebL.Draw();

        ebR.Position = new Vector3((size.x + sizeBoard.x) / 2, -(size.y - 5.5f) / 2, 0);
        ebR.Size = sizeBoard;
        ebR.Draw();
    }

    // Level elements
    void CreateElements(Level _Level)
    {
        Sands.Clear();
        foreach (BaseElLevel_XML lbe in _Level.Elements)
        {
            AddElementGameObject(lbe);
            if (Application.isPlaying && lbe.TypeElement == ElTypeElement.Sand) // list all sand elements (only in the game)
            {
                Sands.Add(lbe._BaseElLevel.GetComponentInChildren<ElLevelSand>());
            }
        }

        if (Application.isPlaying) // make cutouts in the sand (only in the game)
        {
            foreach (BaseElLevel_XML _e in _Level.Elements)
            {
                if (_e.TypeElement == ElTypeElement.SandCutCircle)
                {
                    foreach (ElLevelSand ob in Sands)
                    {
                        ob.AddContour(_e.Position, _e.Position, ((ElLevelSandCutCircle)_e._BaseElLevel).Radius);
                    }
                }
                else if (_e.TypeElement == ElTypeElement.SandCutRectangle)
                {
                    foreach (ElLevelSand ob in Sands)
                    {
                        ElLevelSandCutRectangle e0 = (ElLevelSandCutRectangle)_e._BaseElLevel;
                        //ob.AddContour(_e.Position + (Vector2)(Quaternion.Euler(0, 0, e0.Rotate) * e0.Point1), _e.Position + (Vector2)(Quaternion.Euler(0, 0, e0.Rotate) * e0.Point2));
                        ob.AddContour(_e.Position + (Vector2)(Quaternion.Euler(0, 0, e0.Rotate) * e0.Point1)
                            , _e.Position + (Vector2)(Quaternion.Euler(0, 0, e0.Rotate) * new Vector2(e0.Point1.x, e0.Point2.y))
                            , _e.Position + (Vector2)(Quaternion.Euler(0, 0, e0.Rotate) * e0.Point2)
                            , _e.Position + (Vector2)(Quaternion.Euler(0, 0, e0.Rotate) * new Vector2(e0.Point2.x, e0.Point1.y)));
                    }
                }
            }
        }
    }

    // creating and drawing a level element
    public void AddElementGameObject(BaseElLevel_XML lbe)
    {
        GameObject go = ForEnum.GetAsset(lbe.TypeElement);
        lbe._BaseElLevel = Instantiate(go, transform).GetComponent<BaseElLevel>();
        lbe.SetValuesGameObject();
        lbe._BaseElLevel.Draw();
    }
}

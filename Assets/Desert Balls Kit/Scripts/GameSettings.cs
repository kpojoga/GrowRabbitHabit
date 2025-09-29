using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSettings : MonoBehaviour
{
    public static GameSettings instance = null;

    public Material mSleepBall; // sleeping ball texture
    public List<Material> mColorsBall;

    public static int CountCellsInGrid = 9; // number of cells in a field with balls and chests

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
        catch (Exception e)
        {
            Debug.LogError(e.Message + " Stack: " + e.StackTrace);
        }

        Input.multiTouchEnabled = false;
    }

    public static void addDiamonds(int d)
    {
        PlayerPrefs.SetInt("Diamonds", getDiamonds() + d);
    }

    public static int getDiamonds()
    {
        return PlayerPrefs.GetInt("Diamonds", 0);
    }

    public static void setSound(bool b)
    {
        PlayerPrefs.SetInt("Sound", b ? 1 : 0);
    }

    public static bool getSound()
    {
        return PlayerPrefs.GetInt("Sound", 1) == 1;
    }

    // return the texture of the selected ball
    public Material GetMaterialSelectBall()
    {
        return mColorsBall[UnityEngine.Random.Range(0, mColorsBall.Count)];
    }

    public static void setNowLevel(int lvl)
    {
        lvl = Mathf.Clamp(lvl, 1, LevelsManager.instance.CountLevels);
        PlayerPrefs.SetInt("NowLevel", lvl);
    }

    public static int getNowLevel()
    {
        int lvl = PlayerPrefs.GetInt("NowLevel", 1);
        return Mathf.Clamp(lvl, 1, LevelsManager.instance.CountLevels);
    }
}

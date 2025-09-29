using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

// In this class, actions are performed with menu items for the game.
public class Menus : MonoBehaviour
{
    public static Menus instance = null;

    public GameObject Home;
    public GameObject Game;
    public GameObject EndGame;
    public GameObject Restart;
    public GameObject Black;

    [Header("Home")]
    public Text Home_Level;
    public ToggleController Home_TSound;

    [Header("Game")]
    public Text Game_Level;

    [Header("End Game")]
    public Text EndGame_Level;
    public GameObject EndGame_Reward;
    public GameObject EndGame_Collect;
    public Text EndGame_rewardDiamonds;
    public ParticleSystem EndGame_animDiamonds2;
    public Text EndGame_AllDiamonds;

    private int _rewardDiamonds = 0;
    private int _oldDiamonds = 0;

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

        #region Home
        Home_TSound.isOn = GameSettings.getSound();
        Home_TSound.onToggle.RemoveAllListeners();
        Home_TSound.onToggle.AddListener(SetSound);
        #endregion



        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        m_EventSystem = FindObjectOfType<EventSystem>();
    }

    void Start()
    {
        ShowHome();
    }

    private void Update()
    {
        if (Home.activeSelf && Input.GetMouseButton(0) && !HasUI(Input.mousePosition))
        {
            ShowGame();
            GameManager.instance.StartGame();
            ManagerADs.instance.ShowVideo(ButtonVideoADs.Game);
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


    public void ShowADsBalls()
    {
        ManagerADs.instance.ShowVideo(ButtonVideoADs.Balls);
    }

    public void ShowADsGame()
    {
        ManagerADs.instance.ShowVideo(ButtonVideoADs.Game);
    }

    public void ShowADsNext()
    {
        ManagerADs.instance.ShowVideo(ButtonVideoADs.Next);
    }

    public void ShowADsRestart()
    {
        ManagerADs.instance.ShowVideo(ButtonVideoADs.Restart);
    }

    public void ShowADsSettings()
    {
        ManagerADs.instance.ShowVideo(ButtonVideoADs.Settings);
    }


    public void ShowHome()
    {
        Home.SetActive(true);
        Game.SetActive(false);
        EndGame.SetActive(false);
        Restart.SetActive(false);
        Black.SetActive(false);

        Home_Level.text = "LEVEL " + GameSettings.getNowLevel();
        Game_Level.text = "LEVEL " + GameSettings.getNowLevel();
        EndGame_Level.text = "LEVEL " + GameSettings.getNowLevel() + "\nCOMPLETED!";

        foreach (UIAnimations an in Home.GetComponentsInChildren<UIAnimations>())
        {
            an.StartAnimation();
        }
    }

    IEnumerator ShowBlack(float AnimationTime, UnityAction actEnd)
    {
        Black.SetActive(true);
        yield return null;
        Black.GetComponent<UIAnimations>().AnimationTime = AnimationTime;
        Black.GetComponent<UIAnimations>().StartAnimation();
        yield return new WaitForSeconds(AnimationTime / 2f);
        actEnd.Invoke();
        yield return new WaitForSeconds(AnimationTime / 2f);
        Black.SetActive(false);
        yield return null;
    }


    void SetSound(bool b)
    {
        GameSettings.setSound(b);
    }


#region Game

    public void ShowGame()
    {
        if (!Game.activeSelf)
        {
            Home.SetActive(false);
            Game.SetActive(true);

            foreach (UIAnimations an in Game.GetComponentsInChildren<UIAnimations>())
            {
                an.StartAnimation();
            }
        }
    }

    public void HideGame()
    {
        if (Game.activeSelf)
        {
            Game.SetActive(false);
        }
    }

    public void RestartGame()
    {
        StartCoroutine(ShowBlack(3, () =>
        {
            GameManager.instance.RestartGame();
        }));
    }

    public void Skip()
    {
        ManagerADs.instance.ShowRewardedVideo(() =>
        {
            StartCoroutine(ShowBlack(3, () =>
            {
                GameManager.instance.NextLevel();
                ShowHome();
            }));
        });
    }

#endregion


#region EndGame

    public void ShowEndGame(int rewardDiamonds)
    {
        if (!EndGame.activeSelf)
        {
            GameManager.instance.NextLevel();

            Game.SetActive(false);
            _rewardDiamonds = rewardDiamonds;

            StartCoroutine(ShowBlack(1, () =>
            {
                EndGame.SetActive(true);

                EndGame_Reward.SetActive(false);
                EndGame_Collect.SetActive(false);

                foreach (UIAnimations an in EndGame.GetComponentsInChildren<UIAnimations>())
                {
                    an.StartAnimation();
                }

                StartCoroutine(IShowEndGame());
            }));
        }
    }

    IEnumerator IShowEndGame()
    {
        if (EndGame.activeSelf)
        {
            _oldDiamonds = GameSettings.getDiamonds();
            GameSettings.addDiamonds(_rewardDiamonds);
            EndGame_AllDiamonds.text = _oldDiamonds.ToString();

            EndGame_rewardDiamonds.text = _rewardDiamonds.ToString();
            yield return new WaitForSeconds(1);


            EndGame_Reward.SetActive(true);
            foreach (UIAnimations an in EndGame_Reward.GetComponentsInChildren<UIAnimations>())
            {
                an.StartAnimation();
            }
            yield return new WaitForSeconds(1f);


            EndGame_Collect.SetActive(true);
            foreach (UIAnimations an in EndGame_Collect.GetComponentsInChildren<UIAnimations>())
            {
                an.StartAnimation();
            }
            yield return null;
        }
    }

    IEnumerator IShowEndGame_Collect(UnityAction actEnd)
    {
        int _newDiamonds = GameSettings.getDiamonds();
        float dDiamonds = (_newDiamonds - _oldDiamonds) / 20.0f;

        EndGame_animDiamonds2.Play();
        for (int i = 0; i < 20; i++)
        {
            EndGame_AllDiamonds.text = ((int)(_oldDiamonds + dDiamonds * i)).ToString();
            yield return new WaitForSeconds(0.1f);
        }
        EndGame_AllDiamonds.text = _newDiamonds.ToString();
        yield return new WaitForSeconds(0.25f);

        actEnd.Invoke();
    }

    public void Collect()
    {
        StartCoroutine(IShowEndGame_Collect(() =>
        {
            NextLevel();
        }));
    }

    void NextLevel()
    {
        StartCoroutine(ShowBlack(3, () =>
        {
            GameManager.instance.RestartGame();
            ShowHome();
        }));
    }

#endregion


#region Restart

    public void ShowRestart()
    {
        if (!Restart.activeSelf)
        {
            StartCoroutine(ShowBlack(1, () =>
            {
                Restart.SetActive(true);

                foreach (UIAnimations an in Restart.GetComponentsInChildren<UIAnimations>())
                {
                    an.StartAnimation();
                }
            }));
        }
    }

    public void HideRestart()
    {
        if (Restart.activeSelf)
        {
            Restart.SetActive(false);
        }
    }

#endregion

}

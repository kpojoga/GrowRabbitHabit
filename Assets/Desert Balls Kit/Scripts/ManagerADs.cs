using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public enum ButtonVideoADs
{
    Settings,
    Balls,
    Game,
    Restart,
    Next
}

[System.Serializable]
public class AdEvents
{
    public ButtonVideoADs ads;
    public int everyLevel;
    [HideInInspector]
    public int calls = 0;
}


public class ManagerADs : MonoBehaviour 
#if UNITY_ADS
    , IUnityAdsListener
#endif
{
    public static ManagerADs instance = null;

    public string GameIdAndroid = "Your GameID for Android";
    public string GameIdIos = "Your GameID for Ios";
    [Space(10)]
    public string bannerPlacementId = "YourBannerPlacementId";
    public string videoPlacementId = "YourVideoPlacementId";
    public string rewardVideoPlacementId = "YourRewardVideoPlacementId";
    [Space(10)]
    public bool HasBannerStart = true; // whether to run the banner at the start of the game
    public bool TestMode = true; // Change this to false when going live
    [Space(10)]
    [SerializeField]
    public List<AdEvents> adsEvents = new List<AdEvents>();

    System.Action FuncAfterShowRewardVideo = null; // Function that will be executed after viewing rewarded ads

    string GameId
    {
        get
        {
            string _tmp = "";
#if UNITY_IOS
            _tmp = GameIdIos;
#elif UNITY_ANDROID
            _tmp = GameIdAndroid;
#endif
            return _tmp;
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

    void Start()
    {
#if UNITY_ADS
        Advertisement.AddListener(this);

        if (Advertisement.isSupported && !Advertisement.isInitialized)
        {
            Advertisement.Initialize(GameId, TestMode);
        }

        if (HasBannerStart)
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            StartCoroutine(ShowBannerWhenReady());
        }
#endif
    }

    #region ####### VIDEO AD AND REWARDABLE AD ##########

    public void ShowVideo(ButtonVideoADs state)
    {
        FuncAfterShowRewardVideo = null;
        foreach (AdEvents item in adsEvents)
        {
            if (item.ads == state)
            {
                item.calls++;
                if (item.calls % item.everyLevel == 0)
                {
#if UNITY_ADS
                    if (Advertisement.IsReady(videoPlacementId))
                        Advertisement.Show(videoPlacementId);
#endif
                }
            }
        }
    }

    public void ShowRewardedVideo(System.Action func)
    {
#if UNITY_ADS
        if (Advertisement.IsReady(rewardVideoPlacementId))
        {
            FuncAfterShowRewardVideo = func;
            Advertisement.Show(rewardVideoPlacementId);
        }
#endif
    }

#if UNITY_ADS
    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Ad started");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("Error while playing Ad");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            Debug.Log("Ad is finished, reward the player");

            if (FuncAfterShowRewardVideo != null)
            {
                FuncAfterShowRewardVideo();
                FuncAfterShowRewardVideo = null;
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            Debug.Log("User skipped, do not reward the player");
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }
#endif

    #endregion

    #region ########## BANNER ##########

    IEnumerator ShowBannerWhenReady()
    {
#if UNITY_ADS
        while (!Advertisement.IsReady(bannerPlacementId))
        {
            yield return new WaitForSeconds(0.1f);
        }

        Advertisement.Banner.Show(bannerPlacementId);
#endif
        yield return null;
    }

    public void HideBanner()
    {
#if UNITY_ADS
        Advertisement.Banner.Hide(false);
#endif
    }

    #endregion
}

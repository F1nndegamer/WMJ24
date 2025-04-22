using UnityEngine;
using TMPro;
using NUnit.Framework.Internal.Commands;
public class AdsManager : MonoBehaviour
{
#if UNITY_ANDROID
    //string appKey = "21bfff3f5";
    string appKey = "85460dcd";
#elif UNITY_IPHONE
    string appKey = "";
#else
    string appKey = "unexpected_platform";
#endif
    bool skip;
    void Start()
    {
        IronSource.Agent.validateIntegration();
        IronSource.Agent.init(appKey);
    }
    void OnEnable()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent += SDKInitialized;
        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

    }
    void SDKInitialized()
    {
        Debug.Log("Initialized!");
    }
    void OnApplicationPause(bool pause)
    {
        IronSource.Agent.onApplicationPause(pause);
    }
    #region Interstitial
    public void LoadInterstitial()
    {
        IronSource.Agent.loadInterstitial();
    }
    public void ShowInterstitial()
    {
        if (IronSource.Agent.isInterstitialReady())
        {
            IronSource.Agent.showInterstitial();
        }
        else
        {
            Debug.Log("not ready");
        }
    }

    #endregion
    #region rewarded
    public void LoadRewarded()
    {
        IronSource.Agent.loadRewardedVideo();
    }
    public void ShowRewarded(string reason)
    {
        if(reason == "Skip")
        {
            skip = true;
        }
        else{
            skip = false;
        }
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
        else
        {

            Debug.Log("not ready");
        }
    }

    /************* RewardedVideo AdInfo Delegates *************/
    // Indicates that there’s an available ad.
    // The adInfo object includes information about the ad that was loaded successfully
    // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
    }
    // Indicates that no ads are available to be displayed
    // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable()
    {
    }
    // The Rewarded Video ad view has opened. Your activity will loose focus.
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
    }
    // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
    }
    // The user completed to watch the video, and should be rewarded.
    // The placement parameter will include the reward data.
    // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        Debug.Log("completed");
        if(skip)
        {
            
            MagnetText.instance.text.text = "completed";
            PlayerDeath.Instance.NextLevel(true);
        }
        else
        {
            
            MagnetText.instance.text.text = "completed1";
        }
    }
    // The rewarded video ad was failed to show.
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
    }
    // Invoked when the video ad was clicked.
    // This callback is not supported by all networks, and we recommend using it only if
    // it’s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
    }

    #endregion
}

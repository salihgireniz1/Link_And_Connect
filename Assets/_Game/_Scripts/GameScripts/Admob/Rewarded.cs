using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewarded : MonoSingleton<Rewarded>
{
#if UNITY_ANDROID
    string adUnitId = "b4b0eb9ab64c74aa";
#elif UNITY_IOS
    string adUnitId = "feb8f1b11b7ffd9a";
#endif
    int retryAttempt;

    private Action _onSuccessRewardAdWatch;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(adUnitId);
    }
    public bool RewardedAdIsReady()
    {
        return MaxSdk.IsRewardedAdReady(adUnitId);
    }
    public void ShowRewardedAd(Action successCallback)
    {
        if (MaxSdk.IsRewardedAdReady(adUnitId))
        {
            _onSuccessRewardAdWatch = successCallback;
            MaxSdk.ShowRewardedAd(adUnitId);
        }
        else
        {
            LoadRewardedAd();
        }
    }
    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.
        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("OnRewardedAdDisplayedEvent");
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
        Debug.Log("OnRewardedAdFailedToDisplayEvent");
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("OnRewardedAdClickedEvent");
    }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
        Debug.Log("OnRewardedAdHiddenEvent");
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        Debug.Log("OnRewardedAdReceivedRewardEvent");
        _onSuccessRewardAdWatch?.Invoke();
        _onSuccessRewardAdWatch = null;
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
        Debug.Log("OnRewardedAdRevenuePaidEvent");

    }
}

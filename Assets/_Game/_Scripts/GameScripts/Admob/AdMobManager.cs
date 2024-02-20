//using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdMobManager : MonoSingleton<AdMobManager>
{
    //     public static event Action<Reward> OnRewardEarned;
    //     // These ad units are configured to always serve test ads.
    // #if UNITY_ANDROID
    //     private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
    // #elif UNITY_IPHONE
    //   private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
    // #else
    //   private string _adUnitId = "unused";
    // #endif

    //     private RewardedAd rewardedAd;
    //     private void Start()
    //     {
    //         // When true all events raised by GoogleMobileAds will be raised
    //         // on the Unity main thread. The default value is false.

    //         /*MobileAds.RaiseAdEventsOnUnityMainThread = true;

    //         // Initialize the Google Mobile Ads SDK.
    //         MobileAds.Initialize(initStatus => { });
    //         LoadRewardedAd();
    //         RegisterReloadHandler(rewardedAd);
    //         DontDestroyOnLoad(gameObject);*/
    //     }

    //     /// <summary>
    //     /// Loads the rewarded ad.
    //     /// </summary>
    //     public void LoadRewardedAd()
    //     {
    //         // Clean up the old ad before loading a new one.
    //         if (rewardedAd != null)
    //         {
    //             rewardedAd.Destroy();
    //             rewardedAd = null;
    //         }

    //         Debug.Log("Loading the rewarded ad.");

    //         // create our request used to load the ad.
    //         var adRequest = new AdRequest();
    //         adRequest.Keywords.Add("unity-admob-sample");

    //         // send the request to load the ad.
    //         RewardedAd.Load(_adUnitId, adRequest,
    //             (RewardedAd ad, LoadAdError error) =>
    //             {
    //                 // if error is not null, the load request failed.
    //                 if (error != null || ad == null)
    //                 {
    //                     Debug.LogError("Rewarded ad failed to load an ad " +
    //                                    "with error : " + error);
    //                     return;
    //                 }

    //                 Debug.Log("Rewarded ad loaded with response : "
    //                           + ad.GetResponseInfo());

    //                 rewardedAd = ad;
    //             });
    //     }
    //     public void ShowRewardedAd(Reward reward)
    //     {
    //         const string rewardMsg =
    //             "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";
    //         if (rewardedAd != null && rewardedAd.CanShowAd())
    //         {
    //             rewardedAd.Show((rew) =>
    //             {
    //                 // TODO: Reward the user.
    //                 Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
    //                 OnRewardEarned?.Invoke(reward);
    //                 LoadRewardedAd();
    //             });
    //         }
    //     }

    //     private void RegisterReloadHandler(RewardedAd ad)
    //     {
    //         // Raised when the ad closed full screen content.
    //         ad.OnAdFullScreenContentClosed += () =>
    //     {
    //         Debug.Log("Rewarded Ad full screen content closed.");

    //         // Reload the ad so that we can show another as soon as possible.
    //         LoadRewardedAd();
    //     };
    //         // Raised when the ad failed to open full screen content.
    //         ad.OnAdFullScreenContentFailed += (AdError error) =>
    //         {
    //             Debug.LogError("Rewarded ad failed to open full screen content " +
    //                            "with error : " + error);

    //             // Reload the ad so that we can show another as soon as possible.
    //             LoadRewardedAd();
    //         };
    //     }
}

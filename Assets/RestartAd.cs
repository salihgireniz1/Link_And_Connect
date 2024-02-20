//using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartAd : MonoBehaviour
{
    private void OnEnable()
    {
        //AdMobManager.OnRewardEarned += Unlock;

        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(WatchAndUnlock);
    }
    private void OnDisable()
    {
        //AdMobManager.OnRewardEarned -= Unlock;
    }

    public void WatchAndUnlock()
    {
        // Reward reward = new Reward();
        // reward.Type = "Restart";

        //AdMobManager.Instance.ShowRewardedAd(reward);
        Rewarded.Instance.ShowRewardedAd(UnlockCurrentChoice);
    }
    void UnlockCurrentChoice()
    {
        LevelManager.Instance.LoadLevel();

        Debug.Log("Reward earned! Type: Restart");
    }
    // This method is called from the AdMobManager script when the user earns a reward.
    // public void Unlock(Reward reward)
    // {
    //     if (reward.Type == "Restart")
    //     {
    //         LevelManager.Instance.LoadLevel();

    //         Debug.Log("Reward earned! Type: Restart");
    //     }
    // }
}

//using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchAdToUnlockChoice : MonoBehaviour
{
    public ChoiceButton choiceButton;
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
        // reward.Type = "Unlock";

        //AdMobManager.Instance.ShowRewardedAd(reward);
        Rewarded.Instance.ShowRewardedAd(UnlockCurrentChoice);
    }

    // This method is called from the AdMobManager script when the user earns a reward.
    // public void Unlock(Reward reward)
    // {
    //     if (reward.Type == "Unlock")
    //     {
    //         UnlockCurrentChoice();
    //     }
    // }
    void UnlockCurrentChoice()
    {
        choiceButton.UnlockTheButton();

        // Implement your own logic to give the reward to the user.
        Debug.Log("Reward earned! Type: Unlock");
    }
}

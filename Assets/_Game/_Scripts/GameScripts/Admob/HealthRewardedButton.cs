//using GoogleMobileAds.Api;
using UnityEngine;

public class HealthRewardedButton : MonoBehaviour
{
    public string type = "Health";
    public int healthAmount = 1;
    private void OnEnable()
    {
        //AdMobManager.OnRewardEarned += GiveReward;
    }
    private void OnDisable()
    {
        //AdMobManager.OnRewardEarned -= GiveReward;
    }
    public void ShowHealthRewarded()
    {
        // Reward reward = new Reward();
        // reward.Type = type;
        // reward.Amount = healthAmount;

        //AdMobManager.Instance.ShowRewardedAd(reward);
        Rewarded.Instance.ShowRewardedAd(GiveReward);
    }

    // This method is called from the AdMobManager script when the user earns a reward.
    public void GiveReward()
    {
        HealthManager.Instance.EarnOnlineHealth((int)healthAmount);
    }
}

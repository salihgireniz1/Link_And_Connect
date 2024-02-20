using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClaimButtonController : MonoBehaviour
{
    public int CurrentMultiplier
    {
        get => currentMultiplier;
        set
        {
            if (!rewardAssigned)
            {
                currentMultiplier = value;
                claimXText.text = "Claim x" + currentMultiplier.ToString();
                extraMoneyAmount = MoneyManager.inLevelEarned * currentMultiplier;
                amountText.text = "+" + extraMoneyAmount.ToString();
            }
        }
    }
    [SerializeField] private int currentMultiplier;
    [SerializeField] private int extraMoneyAmount = 0;
    [SerializeField] private TextMeshProUGUI claimXText;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Button claimButton;

    bool rewardAssigned;

    private void Start()
    {
        claimButton?.onClick.AddListener(ClaimExtra);
    }
    void ClaimExtra()
    {
        rewardAssigned = true;
        Rewarded.Instance.ShowRewardedAd(GetReward);
    }
    void GetReward()
    {
        StartCoroutine(RewardAndRoom());
    }
    IEnumerator RewardAndRoom()
    {
        claimButton.interactable = false;
        MoneyManager.inLevelEarned = extraMoneyAmount;
        LevelManager.Instance.lastLevelEndMoney = MoneyManager.inLevelEarned;
        yield return new WaitForSeconds(.25f);
        LevelManager.Instance.LoadMenu(true);
    }
}

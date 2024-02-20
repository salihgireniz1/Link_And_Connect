using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LosePanelHandler : MonoBehaviour
{
    public Button noThanksButton;
    public Button retryButton;
    private float waitDuration = 1.5f;
    private float animDuration = .4f;

    private void OnEnable()
    {
        noThanksButton.transform.localScale = Vector2.zero;
        StartCoroutine(WaitAndShowNoThanks());
        noThanksButton?.onClick.RemoveAllListeners();
        noThanksButton?.onClick.AddListener(NoThanks);
        retryButton?.onClick.AddListener(WatchAdAndRetry);
    }
    private void Update()
    {
        if (retryButton.interactable == false && Rewarded.Instance.RewardedAdIsReady())
        {
            retryButton.interactable = true;
        }
        else if (retryButton.interactable == true && !Rewarded.Instance.RewardedAdIsReady())
        {
            retryButton.interactable = false;
        }
    }
    IEnumerator WaitAndShowNoThanks()
    {
        yield return new WaitForSeconds(waitDuration);
        noThanksButton?.transform.DOScale(Vector2.one, animDuration);
    }
    void NoThanks()
    {
        Interstitial.Instance.ShowInterstitial();
        LevelManager.Instance.LoadMenu(false);
        noThanksButton.interactable = false;
    }
    void WatchAdAndRetry()
    {
        Rewarded.Instance.ShowRewardedAd(
            delegate
            {
                retryButton.interactable = false;
                LevelManager.Instance.LoadLevel(true);
            }
        );
    }
}
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour
{
    public Button button;
    public bool preDefinedWinCondition;

    public Transform startPoint;
    public Transform endPoint;
    public GameObject mainCanvas;
    public Transform rewardBar;
    public Button noThanksButton;
    private void Start()
    {
        button.onClick.RemoveAllListeners();
        noThanksButton?.onClick.RemoveAllListeners();
        noThanksButton?.onClick.AddListener(NoThanks);
        noThanksButton.transform.localScale = Vector2.zero;
        if (preDefinedWinCondition)
        {
            //button.onClick.AddListener(BackToMenu);
            StartCoroutine(ButtonVisibility());
            //MoneyManager.Instance.SpawnForWinPanel(startPoint, endPoint);
        }
    }
    void NoThanks()
    {
        Interstitial.Instance.ShowInterstitial();
        LevelManager.Instance.LoadMenu(true);
        noThanksButton.interactable = false;
    }
    IEnumerator ButtonVisibility()
    {
        // button.interactable = false;
        button.transform.localScale = Vector2.zero;
        rewardBar.localScale = Vector2.zero;
        mainCanvas?.SetActive(false);

        yield return new WaitForSeconds(.5f);
        rewardBar.DOScale(Vector2.one, 0.25f)
            .SetEase(Ease.OutBack);

        yield return new WaitForSeconds(.25f);
        // button.interactable = true;
        MoveManager.Instance.canCheck = false;
        Debug.Log(MoneyManager.inLevelEarned);
        button.transform.DOScale(Vector2.one, 0.25f)
            .SetEase(Ease.OutBack);

        yield return new WaitForSeconds(2f);
        noThanksButton?.transform.DOScale(Vector2.one, .5f);
    }
    public void BackToMenu()
    {
        LevelManager.Instance.LoadMenu(preDefinedWinCondition);
        button.interactable = false;
    }
}

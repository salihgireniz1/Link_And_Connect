using System.Collections;
using System.Collections.Generic;
using AlmostEngine;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EarnedMoneyController : MonoSingleton<EarnedMoneyController>
{
    public int inGameEarnedMoney;
    public float waitDuration = .5f;
    public float scaleDuration = .4f;
    public TextMeshProUGUI earnedText;
    private void OnEnable()
    {
        StartCoroutine(WaitAndScale());
        MoneyManager.OnMoneyEarned += delegate { earnedText.text = MoneyManager.inLevelEarned.ToString(); };
    }
    private void OnDisable()
    {

    }
    IEnumerator WaitAndScale()
    {
        transform.localScale = Vector2.zero;
        yield return new WaitForSeconds(waitDuration);
        inGameEarnedMoney = MoneyManager.inLevelEarned;
        earnedText.text = MoneyManager.inLevelEarned.ToString();
        transform.DOScale(Vector2.one, scaleDuration).SetEase(Ease.OutBack);
    }
}

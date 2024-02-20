using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnergyAdPanel : MonoBehaviour
{
    public int energyAmount;
    public Button watchAdButton;
    public Button closeButton;
    [Header("Buy Section")]
    public int goldAmount;
    public int boughtEnergyAmount;
    public Button buyButton;

    [Header("Animation Settings")]
    public float popDuration;
    public Transform contentHolder;
    private void OnEnable()
    {
        CheckBuyButtonAvailability(MoneyManager.Instance.Money);
        MoneyManager.MoneyChanged += CheckBuyButtonAvailability;
    }
    private void OnDisable()
    {
        MoneyManager.MoneyChanged -= CheckBuyButtonAvailability;
    }
    private void Start()
    {
        watchAdButton.onClick.AddListener(WatchAdAndGainEnergy);
        closeButton.onClick.AddListener(ClosePanel);
        buyButton.onClick.AddListener(BuyEnergy);
    }
    public void BuyEnergy()
    {
        if (MoneyManager.Instance.CanSpendMoney(goldAmount))
        {
            MoneyManager.Instance.SpendMoney(goldAmount);
            HealthManager.Instance.EarnOnlineHealth(boughtEnergyAmount);
        }
    }
    public void CheckBuyButtonAvailability(int totalMoney)
    {
        if (totalMoney < goldAmount)
        {
            // Can not purchase.
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }
    public void WatchAdAndGainEnergy()
    {
        Rewarded.Instance.ShowRewardedAd
            (
                delegate
                {
                    HealthManager.Instance.EarnOnlineHealth(energyAmount);
                    ClosePanel();
                }
            );

    }
    public void ShowPanel()
    {
        gameObject.SetActive(true);
        contentHolder.DOScale(1f, popDuration)
            .SetEase(Ease.OutBack);
    }

    void ClosePanel()
    {
        contentHolder.DOScale(0f, popDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() => gameObject.SetActive(false));
    }
}

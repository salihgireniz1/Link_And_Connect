using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class SetAchivement : Achivement
{
    public override string AchivementHeader 
    { 
        get=>header;
        protected set => header = value; 
    }

    public override int Target => target;
    public override float Score
    {
        get => base.Score;
        set
        {
            base.Score = value;
            if (base.Score >= target)
            {
                // Achivement completed.
                if (IsActive)
                {
                    if (completeToActivate != null)
                        completeToActivate.IsActive = true;
                    else
                    {
                        Achivement nextAchivement = AchivementManager.Instance.GetNextInactiveAchivement();
                        if(nextAchivement != null)
                        {
                            nextAchivement.InitializeAchivement();
                            nextAchivement.IsActive = true;
                        }
                    }
                    IsActive = false;
                    if (IsCompleted == false)
                    {
                        IsCompleted = true;
                    }
                    StartCoroutine(CompleteRoutine());
                    achivementDataSet.DeactivateManually();
                }
                
            }
        }
    }

    [SerializeField]
    private int target;

    [SerializeField]
    private Image fillBar;

    [SerializeField]
    private TextMeshProUGUI uiText;

    [SerializeField]
    private Image achivementImage;

    public ImageSetData achivementDataSet;

    [SerializeField]
    private Achivement completeToActivate;

    [SerializeField]
    private string header;

    [SerializeField]
    private int price;

    [Header("Animation Settings"), Space]
    [SerializeField]
    private Sprite completeSprite;

    [SerializeField]
    private float spriteSwitchTime;

    [SerializeField]
    private Color32 completeColor;

    [SerializeField]
    private float duration = 0.25f;

    [SerializeField]
    private Color32 flashColor = Color.yellow;

    [SerializeField]
    private ParticleSystem confetti;

    private Transform doneText;
    private Transform doneMoney;

    private Image whiteBackground;

    private GameObject particles;

    private Sequence flashSequence;
    private void Awake()
    {
        whiteBackground = transform.GetChild(0).GetComponent<Image>();
        confetti = transform.GetChild(6).GetComponent<ParticleSystem>();
    }

    IEnumerator CompleteRoutine()
    {
        if (IsCompleted) yield return null;

        GetComponent<MMF_Player>().PlayFeedbacks();
        Transform fillBar = transform.GetChild(2);
        doneText = fillBar.GetChild(0);
        doneMoney = fillBar.GetChild(1);
        fillBar.GetComponent<Image>().DOColor(Color.black, spriteSwitchTime);
        doneText.GetComponent<TextMeshProUGUI>().text = "+" + price.ToString("f0");
        doneText.DOScale(Vector3.one, spriteSwitchTime).SetEase(Ease.OutBack);
        doneMoney.DOScale(Vector3.one, spriteSwitchTime).SetEase(Ease.OutBack);
        whiteBackground.DOColor(completeColor, spriteSwitchTime);
        achivementImage.transform.DOScale(Vector3.zero, 0f)
            .SetEase(Ease.InBack);
        achivementImage.sprite = completeSprite;
        particles.SetActive(true);
        yield return new WaitForSeconds(spriteSwitchTime);
        achivementImage.transform.DOScale(Vector3.one, spriteSwitchTime)
            .SetEase(Ease.OutBack);


        confetti.gameObject.SetActive(true);
        confetti.Play();
        //yield return new WaitForSeconds (1f);
        MoneyManager.Instance.SpawnMoneyToSection(transform.position, price, 10);
    }

    [Button]
    public override void InitializeAchivement()
    {
        particles = transform.GetChild(5).gameObject;
        particles.SetActive(false);
        base.InitializeAchivement();
        achivementDataSet.ActivateManually();
        uiText.text = achivementDataSet.Header;
        AchivementHeader = achivementDataSet.Header;
        achivementImage.sprite = achivementDataSet.setSymbol;

        float fillAmount = (float)base.Score / (float)Target;
        DOTween.To(() => fillBar.fillAmount, x => fillBar.fillAmount = x, fillAmount, duration);

        EarnScore(0);
    }

    [Button]
    public void ActivateManually(bool cond)
    {
        IsActive = cond;
    }
    public override void EarnScore(float score)
    {
        base.EarnScore(score);
        if(Score > Target)
        {
            Score = Target;
        }

        if(score != 0)
            AnimateFillAmount();
        
    }
    public void AnimateFillAmount()
    {
        if (flashSequence != null && flashSequence.IsPlaying())
        {
            flashSequence.Complete();
        }
        flashSequence = Animation();
        flashSequence.Play();
    }
    Sequence Animation()
    {
        float fillAmount = (float)base.Score / (float)Target;
        Tween tweenFillAmount = DOTween.To(() => fillBar.fillAmount, x => fillBar.fillAmount = x, fillAmount, duration);
        Tween tweenBackGround = whiteBackground.DOColor(flashColor, duration);
        Tween resetBackGround = whiteBackground.DOColor(Color.white, duration);
        Tween shakeScale = transform.DOShakeScale(duration, .25f, 10, 1f);
        Sequence seq = DOTween.Sequence();
        seq.Join(tweenFillAmount)
            .Join(shakeScale)
            .Join(tweenBackGround)
            .Append(resetBackGround);

        return seq;
    }
    public override void ResetAchievement()
    {
        base.ResetAchievement();
    }
}
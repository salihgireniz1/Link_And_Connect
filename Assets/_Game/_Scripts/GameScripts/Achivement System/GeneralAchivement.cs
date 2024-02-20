using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneralAchivement : Achivement
{
    public override string AchivementHeader
    {
        get => header;
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
                IsActive = false;
                IsCompleted = true;
            }
        }
    }
        
    [SerializeField]
    private string header = "2x";

    [SerializeField]
    private int target = 10;

    [SerializeField]
    private Sprite trophyIcon;

    [SerializeField]
    private string trophyName;

    [SerializeField]
    private string trophyExplanation;

    [SerializeField]
    private Image iconRenderer;

    [SerializeField]
    private TextMeshProUGUI nameTextField;

    [SerializeField]
    private TextMeshProUGUI explanationTextField;

    [SerializeField]
    private Image fillBar;

    [SerializeField]
    private TextMeshProUGUI progressText;

    public override void InitializeAchivement()
    {
        if (Score >= Target)
        {
            IsActive = false;
        }
        else
        {
            IsActive = true;
        }
        explanationTextField.text = trophyExplanation;
        nameTextField.text = trophyName;
        iconRenderer.sprite = trophyIcon;
        EarnScore(0);
    }
    public override void EarnScore(float score)
    {
        if (Score >= Target) 
        {
            progressText.text = "COMPLETED";
        }
        else
        {
            base.EarnScore(score); 
            progressText.text = $"{Score}/{Target}";
        }
        fillBar.fillAmount = (float)base.Score / Target;
    }
}

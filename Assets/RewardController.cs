using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    public EarnedMoneyController earnedMoneyController;
    public Transform arrow;
    public float startAngle = -15f;
    public float targetAngle = 15f;

    public int[] multipliers;
    public TextMeshProUGUI[] multiplierTexts;
    public int currentMultiplier;
    public Color32 defaultTextColor;
    public Color32 selectedTextColor;

    public TextMeshProUGUI currentText = null;
    public ClaimButtonController claimButtonController;

    // Start is called before the first frame update
    void Start()
    {
        arrow.rotation = Quaternion.Euler(Vector3.forward * startAngle);
        arrow.DORotate(Vector3.forward * targetAngle, 1.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic);
    }
    float angle;
    void UpdateTextColor(int index)
    {
        if (currentText == null)
        {
            currentText = multiplierTexts[index];
            currentText.color = selectedTextColor;
        }

        if (currentText != multiplierTexts[index])
        {
            currentText.color = defaultTextColor;
            currentText.transform.DOScale(Vector3.one, .1f);

            currentText = multiplierTexts[index];

            currentText.color = selectedTextColor;
            currentText.transform.DOScale(Vector3.one * 1.2f, .1f);
            claimButtonController.CurrentMultiplier = currentMultiplier;
        }
    }
    private void Update()
    {
        angle = 360 - arrow.localEulerAngles.z < 180f ? arrow.localEulerAngles.z - 360f : arrow.localEulerAngles.z;
        switch (angle)
        {
            case > 25f:
                currentMultiplier = multipliers[0];
                UpdateTextColor(0);
                break;
            case > 17f:
                currentMultiplier = multipliers[1];
                UpdateTextColor(1);
                break;
            case > 8f:
                currentMultiplier = multipliers[2];
                UpdateTextColor(2);
                break;
            case > -8f:
                currentMultiplier = multipliers[3];
                UpdateTextColor(3);
                break;
            case > -17f:
                currentMultiplier = multipliers[4];
                UpdateTextColor(4);
                break;
            case > -25f:
                currentMultiplier = multipliers[5];
                UpdateTextColor(5);
                break;
            default:
                currentMultiplier = multipliers[6];
                UpdateTextColor(6);
                break;
        }
    }
}

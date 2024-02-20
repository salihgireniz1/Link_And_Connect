using TMPro;
using UnityEngine;

public class HealthData : MonoBehaviour
{
    public TextMeshProUGUI healthAmountText;
    public TextMeshProUGUI healthTimerText;

    private void Start()
    {
        healthAmountText.text = HealthManager.Instance.HealthCount.ToString();
        healthTimerText.text = "Full";

        if (HealthManager.Instance.counterText == null)
        {
            HealthManager.Instance.counterText = healthTimerText;
            HealthManager.Instance.healthText = healthAmountText;
        }
    }
}

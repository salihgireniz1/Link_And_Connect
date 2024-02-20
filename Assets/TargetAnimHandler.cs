using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetAnimHandler : MonoBehaviour
{
    public TextMeshProUGUI text;
    int currentScore = 0;
    void Start()
    {
        currentScore = 0;
        text.text = currentScore.ToString("f0");
    }

    public void IncreaseText()
    {
        currentScore += 1;
        text.text = currentScore.ToString("f0");
    }
}

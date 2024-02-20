using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveCountController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moveCounter;

    private void OnEnable()
    {
        MoveManager.OnMove += UpdateCountText;
    }
    private void OnDisable()
    {
        MoveManager.OnMove -= UpdateCountText;
    }
    void UpdateCountText(int count)
    {
        if (moveCounter != null)
        {
            moveCounter.text = count.ToString();
        }
    }
}
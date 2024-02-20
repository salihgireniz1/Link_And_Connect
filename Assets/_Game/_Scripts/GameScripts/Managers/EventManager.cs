using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager
{
    public static event Action OnActiveWinPanel;
    public static event Action OnActiveLosePanel;
    public static event Action OnActiveStartPanel;
    public static event Action<float> OnScoreChange;
    public static event Action<Vector3,bool> OnCollectMoneyUI;


    public static void Invoke_OnActiveWinPanel()
    {
        OnActiveWinPanel?.Invoke();
    }

    public static void Invoke_OnActiveLosePanel()
    {
        OnActiveLosePanel?.Invoke();
    }
    public static void Invoke_OnChangeStartPanelCondition()
    {
        OnActiveStartPanel?.Invoke();
    }
    public static void Invoke_OnScoreChange(float score)
    {
        OnScoreChange?.Invoke(score);
    }
    public static void Invoke_OnCollectMoneyUI(Vector3 UIStartPosition,bool isMultiple)
    {
        OnCollectMoneyUI?.Invoke(UIStartPosition, isMultiple);
    }
}

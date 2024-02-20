using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class GAEventController : MonoSingleton<GAEventController>
{
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += CheckGameState;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= CheckGameState;
    }
    void Awake()
    {
        GameAnalytics.Initialize();
    }

    public void CheckGameState(GameState state)
    {
        switch (state)
        {
            case GameState.loaded:
                SendLevelStartEvent();
                break;
            case GameState.failed:
                SendFailEvent();
                break;
            case GameState.completed:
                SendWinEvent();
                break;
            default:
                break;
        }
    }
    public void SendFailEvent()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level " + (LevelManager.Instance.CurrentLevelIndex + 1).ToString());
        //Debug.Log("Level Failed: " + (LevelManager.Instance.CurrentLevelIndex + 1).ToString());
    }
    public void SendWinEvent()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level " + (LevelManager.Instance.CurrentLevelIndex + 1).ToString());
        //Debug.Log("Level Complete: " + (LevelManager.Instance.CurrentLevelIndex + 1).ToString());
    }
    public void SendLevelStartEvent()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level " + (LevelManager.Instance.CurrentLevelIndex + 1).ToString());
        //Debug.Log("Level Started: " + (LevelManager.Instance.CurrentLevelIndex + 1).ToString());
    }
}

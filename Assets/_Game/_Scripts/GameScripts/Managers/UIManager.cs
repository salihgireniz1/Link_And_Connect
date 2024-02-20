using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using PAG.Pool;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI healthText;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private GameObject winPanel;

    [SerializeField]
    private GameObject losePanel;


    private BaseGamePanel currentActivePanel;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameState;
        LevelManager.OnLevelLoaded += HandleLevelLoad;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameState;
        LevelManager.OnLevelLoaded -= HandleLevelLoad;
    }
    public void ActivatePanel(BaseGamePanel panel)
    {
        if (panel == currentActivePanel) return;
        if (currentActivePanel != null)
        {
            currentActivePanel.gameObject.SetActive(false);
        }
        panel.gameObject.SetActive(true);
        currentActivePanel = panel;
    }
    public void DisablePanel(BaseGamePanel panel)
    {
        panel.DisableThisPanel();
        if (currentActivePanel == panel)
        {
            currentActivePanel = null;
        }
    }
    public void HandleLevelLoad(Level level)
    {
        int currentLevel = LevelManager.Instance.CurrentLevelIndex;
        if (levelText != null)
        {
            levelText.text = "LEVEL " + (currentLevel + 1).ToString();
        }
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }
    }
    public void HandleGameState(GameState state)
    {
        switch (state)
        {
            case GameState.failed:
                if (losePanel != null)
                {
                    losePanel.SetActive(true);
                }
                break;
            case GameState.completed:
                if (winPanel != null)
                {
                    StartCoroutine(WinDelay());
                }
                break;
            default:
                break;
        }
    }
    IEnumerator WinDelay()
    {
        float extraWait = 0f;
        MergeRewardObject[] mergeRewardObjects = FindObjectsOfType<MergeRewardObject>();
        foreach (var item in mergeRewardObjects)
        {
            item.GetMergeReward();
        }
        extraWait = mergeRewardObjects.Length > 0 ? 1f : 0f;
        yield return new WaitForSeconds(2f + extraWait);
        winPanel.SetActive(true);
    }
}

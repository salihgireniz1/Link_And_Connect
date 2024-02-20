using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePanelController : MonoBehaviour
{
    [SerializeField] private InGamePanelSettings _settings;

    [SerializeField] private LevelPanel _levelPanel;
    [SerializeField] private ScorePanel _scorePanel;
    [SerializeField] private RestartPanel _restartPanel;
    [SerializeField] private DebugPanel _debugPanel;

    private void Start()
    {
        _restartPanel.SetPanelActive(_settings.RestartPanelActivated);
        _scorePanel.SetPanelActive(_settings.ScorePanelActivated);
        _levelPanel.SetPanelActive(_settings.LevelPanelActivated);
        _debugPanel.SetPanelActive(_settings.DebugPanelActivated);
    }

    public void OpenPanel()
    {
        transform.gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        transform.gameObject.SetActive(false);
    }
}

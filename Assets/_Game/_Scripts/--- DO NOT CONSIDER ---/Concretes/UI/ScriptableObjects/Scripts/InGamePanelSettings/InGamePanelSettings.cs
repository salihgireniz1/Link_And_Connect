using UnityEngine;

[CreateAssetMenu(fileName = "InGamePanelSettings", menuName = "UIData/InGamePanelSettings", order = 1)]
public class InGamePanelSettings : ScriptableObject
{
    [SerializeField] private bool _scorePanelActivated;
    public bool ScorePanelActivated { get => _scorePanelActivated; private set => _scorePanelActivated = value; }

    [SerializeField] private bool _levelPanelActivated;
    public bool LevelPanelActivated { get => _levelPanelActivated; private set => _levelPanelActivated = value; }

    [SerializeField] private bool _restartPanelActivated;
    public bool RestartPanelActivated { get => _restartPanelActivated; private set => _restartPanelActivated = value; }
    [SerializeField] private bool _debugPanelActivated;
    public bool DebugPanelActivated { get => _debugPanelActivated; private set => _debugPanelActivated = value; }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RestartPanelSettings", menuName = "UIData/RestartPanelSettings", order = 1)]

public class RestartPanelSettings : ScriptableObject
{
    [SerializeField] private Sprite _backgroundSprite;
    public Sprite BackgroundSprite { get => _backgroundSprite; set => _backgroundSprite = value; }

    [SerializeField] private Sprite _buttonSprite;
    public Sprite ButtonSprite { get => _buttonSprite; set => _buttonSprite = value; }

}

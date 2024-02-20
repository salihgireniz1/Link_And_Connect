using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WinPanelSettings", menuName = "UIData/WinPanelSettings", order = 1)]
public class WinPanelSettings :ScriptableObject
{
    [SerializeField] private Sprite _backgroundSprite;
    public Sprite BackgroundSprite { get => _backgroundSprite; set => _backgroundSprite = value; }

    [SerializeField] private Sprite _winSprite;
    public Sprite WinSprite { get => _winSprite; set => _winSprite = value; }

    [SerializeField] private Sprite _buttonSprite;
    public Sprite ButtonSprite { get => _buttonSprite; set => _buttonSprite = value; }

}

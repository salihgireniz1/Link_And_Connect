using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LosePanelSettings", menuName = "UIData/LosePanelSettings", order = 1)]
public class LosePanelSettings : ScriptableObject
{
    [SerializeField] private Sprite _backgroundSprite;
    public Sprite BackgroundSprite { get => _backgroundSprite; set => _backgroundSprite = value; }

    [SerializeField] private Sprite _loseSprite;
    public Sprite LoseSprite { get => _loseSprite; set => _loseSprite = value; }

    [SerializeField] private Sprite _buttonSprite;
    public Sprite ButtonSprite { get => _buttonSprite; set => _buttonSprite = value; }
}

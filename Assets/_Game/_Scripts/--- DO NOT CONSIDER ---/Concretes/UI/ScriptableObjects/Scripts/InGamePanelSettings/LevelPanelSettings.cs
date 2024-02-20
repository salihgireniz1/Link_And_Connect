using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelPanelSettings", menuName = "UIData/LevelPanelSettings", order = 1)]

public class LevelPanelSettings : ScriptableObject
{
    [SerializeField] private Sprite _backgroundSprite;
    public Sprite BackgroundSprite { get => _backgroundSprite; set => _backgroundSprite = value; }
}

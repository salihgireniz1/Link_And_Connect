using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ScorePanelSettings", menuName = "UIData/ScorePanelSettings", order = 1)]

public class ScorePanelSettings : ScriptableObject
{
    [SerializeField] private Sprite _backgroundSprite;
    public Sprite BackgroundSprite { get => _backgroundSprite; set => _backgroundSprite = value; }

    [SerializeField] private Sprite _collectibleSprite;
    public Sprite CollectibleSprite { get => _collectibleSprite; set => _collectibleSprite = value; }
}

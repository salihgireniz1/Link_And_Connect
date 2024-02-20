using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StartPanelSettings", menuName = "UIData/StartPanelSettings", order = 1)]
public class StartPanelSettings : ScriptableObject
{
    [SerializeField] private Sprite _background;
    public Sprite Background { get => _background; private set => _background = value; }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GroupInfo : SpriteInfo
{
    public TextMeshPro counterText;
    public List<SpriteInfo> connections = new();
}

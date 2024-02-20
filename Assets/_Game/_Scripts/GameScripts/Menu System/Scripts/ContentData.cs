using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Content Data", menuName = "ScriptableObjects/Content Info")]
public class ContentData : ScriptableObject
{
    public bool isLocked;
    public Sprite contentImage;
    public string header;
    public ContentPanel contentPanel;
}

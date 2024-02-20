using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteInfo : SceneBasedObject
{
    public string header;
    public SpriteRenderer contentRenderer;
    public SpriteRenderer whiteBackground;
    public Tile myTile;
    public Color32 bgDefaultColor = Color.white;

    public void UpdateBGColor(Color32 color)
    {
        whiteBackground.color = color;
    }
}

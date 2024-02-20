using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Infos", menuName = "ScriptableObjects/Level Data Holder")]
public class LevelInfo : ScriptableObject
{
    public List<Level> levels = new List<Level>();
}
[System.Serializable]
public class Level
{
    // Contains grid infos.
    public GridData gridData = new();
    public int levelMoveCount = 20;
    public bool isPassed = false;
}

[System.Serializable]
public struct ImageData
{
    public List<Sprite> LevelImages;
}
[System.Serializable]
public class GridData
{
    /// <summary>
    /// The number of columns in the grid
    /// </summary>
    public int width = 4;

    /// <summary>
    /// The number of rows in the grid
    /// </summary>
    public int height = 6;

    /// <summary>
    /// The size of each tile in the grid (width)
    /// </summary>
    public float tileWidthLength = 2.25f;

    /// <summary>
    /// The size of each tile in the grid (height)
    /// </summary>
    public float tileHeightLength = 1.65f;

}
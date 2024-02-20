using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

/// <summary>
/// A singleton class for generating a grid of tiles
/// </summary>
public class GridGenerator : MonoSingleton<GridGenerator>
{
    public static event Action OnGridGenerated;


    /// <summary>
    /// The Grid object to fill with tiles
    /// </summary>
    public Grid grid;


    public float shiftDelayTime = 0.5f;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += GenerateGrid;
        //MergeManager.OnSpritesMerged += CheckRawEmptyness;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= GenerateGrid;
        //MergeManager.OnSpritesMerged -= CheckRawEmptyness;
    }
    void CheckRawEmptyness(List<SpriteInfo> mergedSprites)
    {
        List<int> emptyRawTiles = new();
        for (int i = 0; i < mergedSprites.Count; i++)
        {
            int rowIndex = Mathf.RoundToInt(mergedSprites[i].myTile.Point.y / grid.Height);

            mergedSprites[i].myTile = new Tile(new Vector2(-10f, -10f));
            List<Tile> tilesToCheck = grid.RowInfo[rowIndex];
            bool isEmpty = true;
            for (int j = 0; j < tilesToCheck.Count; j++)
            {
                if (tilesToCheck[j].FillingSprite != null)
                {
                    isEmpty = false;
                    break;
                }
            }
            if (isEmpty)
            {
                if (!emptyRawTiles.Contains(rowIndex))
                {
                    emptyRawTiles.Add(rowIndex);
                    //ShiftTiles(rowIndex);
                }

            }
        }
        StartCoroutine(DelayedShift(emptyRawTiles));
    }
    IEnumerator DelayedShift(List<int> emptyRawTiles)
    {
        yield return new WaitForSeconds(shiftDelayTime);
        for (int i = emptyRawTiles.Count - 1; i >= 0; i--)
        {
            ShiftTiles(emptyRawTiles[i]);
        }
    }
    public void ShiftTiles(int rowIndex)
    {
        foreach (var key in grid.RowInfo)
        {
            List<Tile> from = new();
            List<Tile> to = new();
            if (key.Key > rowIndex)
            {
                from.AddRange(key.Value);
                to.AddRange(grid.RowInfo[key.Key - 1]);
            }
            else
            {
                continue;
            }
            for (int i = 0; i < from.Count; i++)
            {
                if (from[i].FillingSprite != null)
                {
                    SpriteInfo spriteToShift = from[i].FillingSprite;
                    from[i].FillingSprite = null;
                    LevelGenerator.Instance.AlignSpriteToTile(spriteToShift, to[i]);
                }
            }
        }
    }
    /// <summary>
    /// Generates a new Grid object with the specified width and height, and fills it with tiles
    /// </summary>
    void GenerateGrid(GameState state)
    {
        if (state == GameState.loaded)
        {
            Level level = LevelManager.Instance.latestLevel;
            // create a new Grid object with the specified width and height
            grid = new Grid(level.gridData.width, level.gridData.height);

            // create a list to store the tiles in each column
            List<Tile> columnTiles;

            // iterate over each column in the grid
            for (int x = 0; x < level.gridData.height; x++)
            {
                // create a new list to store the tiles in this column
                columnTiles = new List<Tile>();

                // iterate over each row in the column
                for (int y = 0; y < level.gridData.width; y++)
                {
                    // calculate the position of the current tile
                    Vector2 position = new Vector2(y * level.gridData.tileWidthLength, x * level.gridData.tileHeightLength);

                    // create a new tile at the current position
                    Tile tile = new Tile(position);

                    // add the new tile to the list for this column
                    columnTiles.Add(tile);
                }

                // add the list of tiles for this column to the Grid object
                grid.RowInfo.Add(x, columnTiles);
            }
            OnGridGenerated?.Invoke();
            // print the tiles in each row to the console
            //PrintRowTiles();
        }
    }

    // /// <summary>
    // /// Prints the tiles in each row of the grid to the console
    // /// </summary>
    // void PrintRowTiles()
    // {
    //     // iterate over each row in the grid
    //     foreach (int rowIndex in grid.RowInfo.Keys)
    //     {
    //         if (grid.RowInfo.ContainsKey(rowIndex))
    //         {
    //             // get the list of tiles in the specified row
    //             List<Tile> rowTiles = grid.RowInfo[rowIndex];

    //             // print the tile points to the console
    //             Debug.LogFormat("Row {0} Contains: {1}", rowIndex, string.Join(", ", rowTiles.Select(t => $"({t.Point.x},{t.Point.y})")));
    //         }
    //         else
    //         {
    //             Debug.LogFormat("Row {0} is not present in the grid.", rowIndex);
    //         }
    //     }
    // }
    // void OnDrawGizmos()
    // {
    //     Level level = LevelManager.Instance.GetLevelInfo();

    //     // set the color of the gizmos
    //     Gizmos.color = Color.red;

    //     // iterate over each column in the grid
    //     for (int x = 0; x < level.gridData.height; x++)
    //     {
    //         // iterate over each row in the column
    //         for (int y = 0; y < level.gridData.width; y++)
    //         {
    //             // calculate the position of the current tile
    //             Vector2 position = new Vector2(y * level.gridData.tileWidthLength, x * level.gridData.tileHeightLength);

    //             // draw a square gizmo for the current tile
    //             Gizmos.DrawWireCube(position, new Vector3(level.gridData.tileWidthLength, level.gridData.tileHeightLength, 0.01f));
    //         }
    //     }
    // }
}

using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The LevelGenerator class generates and populates a level grid with images.
/// </summary>
public class LevelGenerator : MonoSingleton<LevelGenerator>
{
    /// <summary>
    /// The background game object for the level.
    /// </summary>
    public GameObject background;

    /// <summary>
    /// The grid that has been generated for the level.
    /// </summary>
    private Grid generatedGrid;

    private GameObject bgParent;
    private void OnEnable()
    {
        ImageDataManager.OnImagesGenerated += GenerateLevelImages;
    }
    private void OnDisable()
    {
        ImageDataManager.OnImagesGenerated -= GenerateLevelImages;
    }

    /// <summary>
    /// Gets the tiles that need to be filled based on the start point.
    /// </summary>
    /// <param name="grid">The grid to search for tiles in.</param>
    /// <param name="startPoint">The starting point to search for tiles from.</param>
    /// <returns>A list of tiles that need to be filled with images.</returns>
    public List<Tile> GetTilesToFill(Grid grid, Vector2Int startPoint)
    {
        List<Tile> tiles = new List<Tile>();

        // Get the row number of the starting point
        int startingRow = startPoint.y;

        // Loop over each row in the grid
        for (int row = startingRow; row < grid.Height; row++)
        {
            // If the row contains any tiles
            if (grid.RowInfo.ContainsKey(row))
            {
                // Loop over each tile in the row
                foreach (Tile tile in grid.RowInfo[row])
                {
                    // If the tile is after the starting point
                    if (tile.Point.x >= startPoint.x || tile.Point.y > startPoint.y)
                    {
                        // Add it to the list of tiles
                        tiles.Add(tile);
                    }
                }
            }
        }

        return tiles;
    }

    [Button]
    /// <summary>
    /// Fills the grid with images starting from the specified point.
    /// </summary>
    /// <param name="point">The starting point to fill from.</param>
    public void FillGrid(Vector2Int point, List<SpriteInfo> activeImages)
    {
        try
        {
            generatedGrid = GridGenerator.Instance.grid;

            if(bgParent != null) Destroy(bgParent);
            bgParent = new GameObject();
            bgParent.name = "Backgrounds";
            bgParent.transform.SetParent(this.transform);

            List<SpriteInfo> data = new();
            foreach (SpriteInfo spriteInfo in activeImages)
            {
                data.Add(spriteInfo);
            }

            foreach (Tile tile in GetTilesToFill(generatedGrid, point))
            {
                if (data.Count == 0) break;

                SpriteInfo info = data[Random.Range(0, data.Count)];
                AlignSpriteToTile(info, tile);
                Instantiate
                    (
                    background,
                    info.gameObject.transform.position + new Vector3(0f, 0f, 0.1f),
                    Quaternion.identity,
                    bgParent.transform
                    );

                data.Remove(info);
            }
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Please Generate a GRID before filling it.");
            throw;
        }
    }
    public void AlignSpriteToTile(SpriteInfo info, Tile tile)
    {
        Debug.Log("ALIGNING SPRITES TO TILE");
        info.myTile = tile;
        tile.FillingSprite = info;

        info.gameObject.name = $"{info.header}({tile.Point})";

        Vector3 infoPosition = new Vector3(tile.Point.x, tile.Point.y, 0f);
        info.gameObject.transform.position = infoPosition;


        if (info.transform.parent != this.transform)
        {
            info.gameObject.transform.SetParent(this.transform);
        }
        if (!info.gameObject.activeInHierarchy)
        {
            info.gameObject.SetActive(true);
        } 
    }

    /// <summary>
    /// Generates images for the level based on the grid that was generated.
    /// </summary>
    /// <param name="grid">The generated grid.</param>
    public void GenerateLevelImages(List<SpriteInfo> activeImages)
    {
        Debug.Log("FILLING GRID");
        FillGrid(new Vector2Int(0, 0), activeImages);
        GameManager.Instance.UpdateGameState(GameState.playing);
    }

}
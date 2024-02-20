using UnityEngine;

[System.Serializable]
// The Tile class represents a single tile in a grid.
public class Tile
{
    // The point on the grid where this tile is located.
    public Vector2 Point;

    public SpriteInfo FillingSprite;

    // Constructor for the Tile class.
    public Tile(Vector2 point)
    {
        // Initialize the Point property.
        this.Point = point;
    }
}

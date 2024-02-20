using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Grid
{
    // The width of the grid.
    public int Width;

    // The height of the grid.
    public int Height;

    // A dictionary that maps row numbers to lists of Tile objects.
    public Dictionary<int, List<Tile>> RowInfo;

    // Constructor for the Grid class.
    public Grid(int width, int height)
    {
        // Initialize the Width and Height properties.
        this.Width = width;
        this.Height = height;

        // Create a new instance of the Dictionary class to store row information.
        RowInfo = new();
    }
}

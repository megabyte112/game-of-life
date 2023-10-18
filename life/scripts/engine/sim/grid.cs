/*

grid.cs

2023 Computer Science NEA
Aidan Norton

*/

using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Life.Engine.Simulation;

class Grid
{
    private readonly Life life;

    // width and height
    private readonly int width;
    private readonly int height;

    // how big the cells are drawn, in pixels
    private readonly int cellSizeLive;

    // rounded box texture
    private readonly Texture2D boxTextureLive;
    private readonly Texture2D boxTextureDead;

    // offset from the corner of a cell to the corner of the dead cell
    private readonly Vector2 deadOffset;

    // border texture
    private readonly Texture2D borderTexture;
    private readonly Vector2 borderOffset;

    // how many times the grid is updated per second
    private int updateRateIndex;
    public int Speed => speeds[updateRateIndex];

    // every factor of 60
    private readonly int[] speeds = { 1, 2, 3, 4, 5, 6, 10, 12, 15, 20, 30, 60 };

    // the grid of cells itself
    private readonly Cell[,] rawGrid;
    private bool isPaused;

    public bool IsPlaying => !isPaused;

    // random number generation
    private readonly Random rnd = new();

    public Grid(Life game, int width, int height,
        int liveCellSize, int deadCellSize,
        int borderSize,
        Color liveColour
    )
    {
        // initialise variables
        life = game;
        this.width = width;
        this.height = height;
        cellSizeLive = liveCellSize;
        var cellSizeDead = deadCellSize;
        var borderWidth = borderSize;

        updateRateIndex = 8;

        rawGrid = new Cell[height, width];

        isPaused = false;

        // set cell texture
        boxTextureLive = life.Drawing.GetRectangle(
            cellSizeLive, cellSizeLive, liveColour);

        if (deadCellSize > 0)
        {
            boxTextureDead = life.Drawing.GetRectangle(
                cellSizeDead, cellSizeDead, Color.Black * 0.2f);
        }
        else
        {
            boxTextureDead = null;
        }

        // set border texture
        if (borderSize > 0)
        {
            borderTexture = life.Drawing.GetRoundedRectangle(
                width * cellSizeLive + borderWidth * 2,
                height * cellSizeLive + borderWidth * 2,
                Color.White * 0.2f
            );
        }
        else
        {
            borderTexture = null;
        }

        borderOffset = new Vector2(borderWidth, borderWidth);

        deadOffset = new Vector2(
            (float)(cellSizeLive - cellSizeDead) / 2,
            (float)(cellSizeLive - cellSizeDead) / 2
        );

        // initialise the grid
        Clear();
    }

    public void Update(GameTime dt)
    {
        // return if paused or unfocused
        if (isPaused || !life.IsActive ||
            dt.TotalGameTime.TotalSeconds % (1d / speeds[updateRateIndex]) >=
            dt.ElapsedGameTime.TotalSeconds) return;

        // count the neighbours around every cell except the edges
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                int count = 0;
                for (int a = -1; a < 2; a++)
                {
                    for (int b = -1; b < 2; b++)
                    {
                        if (a == 0 && b == 0) continue;
                        if (rawGrid[y + a, x + b].IsLive) count++;
                    }
                }
                rawGrid[y, x].Neighbours = count;
            }
        }

        // apply conway's rules to each cell
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (rawGrid[y, x].IsLive && rawGrid[y, x].Neighbours < 2) rawGrid[y, x].IsLive = false;
                if (rawGrid[y, x].IsLive && rawGrid[y, x].Neighbours > 3) rawGrid[y, x].IsLive = false;
                if (!rawGrid[y, x].IsLive && rawGrid[y, x].Neighbours == 3) rawGrid[y, x].IsLive = true;
            }
        }
    }

    public void Draw(GameTime dt, Vector2 pos)
    {
        // draw border
        if (borderTexture != null) life.Drawing.DrawTexture(borderTexture,
            pos - borderOffset, 1f);

        // draw the grid, centred on the border
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (GetCell(x, y).IsLive)
                    life.Drawing.DrawTexture(boxTextureLive,
                        pos + new Vector2(x * cellSizeLive, y * cellSizeLive),
                        1f);
                else if (boxTextureDead != null) life.Drawing.DrawTexture(boxTextureDead,
                    pos + deadOffset + new Vector2(x * cellSizeLive, y * cellSizeLive),
                    1f);
            }
        }
    }

    // randomises the grid
    public void Randomise()
    {
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                rawGrid[y, x].IsLive = rnd.Next(2) == 0;
            }
        }
    }

    // sets all cells on the grid to dead
    public void Clear()
    {
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                rawGrid[y, x].IsLive = false;
            }
        }
    }

    // returns the cell at the specified coordinate
    private Cell GetCell(int x, int y)
    {
        return rawGrid[y, x];
    }

    // returns the cell under the mouse cursor
    public Vector2 GetCellPosAt(Vector2 mousePos, Vector2 gridPos)
    {
        mousePos -= gridPos;
        int x = (int)(mousePos.X / cellSizeLive);
        int y = (int)(mousePos.Y / cellSizeLive);
        return new Vector2(x, y);
    }

    // returns try if the mouse is over the grid
    public bool IsValidCell(Vector2 mousePos, Vector2 gridPos)
    {
        mousePos -= gridPos;
        int x = (int)(mousePos.X / cellSizeLive);
        int y = (int)(mousePos.Y / cellSizeLive);
        return x >= 1 && x < width - 1 && y >= 1 && y < height - 1;
    }

    public void SetCell(int x, int y, bool isLive)
    {
        rawGrid[y, x].IsLive = isLive;
    }

    public void Play()
    {
        isPaused = false;
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void DecreaseSpeed()
    {
        if (updateRateIndex > 0) updateRateIndex--;
    }

    public void IncreaseSpeed()
    {
        if (updateRateIndex < speeds.Length - 1) updateRateIndex++;
    }

    public void SetSpeedIndex(int index)
    {
        if (index >= 0 && index < speeds.Length) updateRateIndex = index;
    }
    
    // save the grid to a .bin file
    // this is never used in the app but is useful for debugging
    public void Save(string name)
    {
        var file = File.Open($"{name}.bin", FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(width);
        writer.Write(height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                writer.Write(rawGrid[y, x].IsLive);
            }
        }
        writer.Close();
        file.Close();
    }
    
    // load a grid state
    public void Load(string name)
    {
        var file = File.Open($"assets/grids/{name}.bin", FileMode.Open);
        BinaryReader reader = new BinaryReader(file);
        int newWidth = reader.ReadInt32();
        int newHeight = reader.ReadInt32();
        
        if (newWidth != width || newHeight != height)
            throw new Exception("Saved grid size does not match");
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                rawGrid[y, x].IsLive = reader.ReadBoolean();
            }
        }
        reader.Close();
        file.Close();
    }
}

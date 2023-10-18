/*

object.cs

2023 Computer Science NEA
Aidan Norton

*/

using Microsoft.Xna.Framework;

namespace Life.Engine;

public abstract class Object
{
    public float Width;
    public float Height;
    public readonly float X;
    public readonly float Y;

    public Vector2 Position;

    protected Object(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Position = new Vector2(X, Y);
    }

    public void SetPosition(Vector2 newPos)
    {
        Position = newPos;
    }

    public void SetPosition(int xPosition, int yPosition)
    {
        Position = new Vector2(xPosition, yPosition);
    }
}

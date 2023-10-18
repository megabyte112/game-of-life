/*

button.cs

2023 Computer Science NEA
Aidan Norton

*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Life.Engine.Interface;

public class Button : Object
{
    // name to identify the button against others
    // also used as the text to display on the button
    public string Name { get; }
    public bool IsDisabled { get; set; }

    // key to press to activate the button
    public readonly string Key;

    // texture to use
    public readonly Texture2D Texture;
    public readonly Texture2D IconTexture;

    public Button(string name, int x, int y, int width, int height, string key,
        Life life, Texture2D iconTexture) : base(x, y, width, height)
    {
        Position.X = x;
        Position.Y = y;
        Width = width;
        Height = height;
        Texture = life.Drawing.GetRoundedRectangle(width, height, Color.White * 0.4f);
        IconTexture = iconTexture;
        Name = name;
        Key = key;
        IsDisabled = false;
    }
}
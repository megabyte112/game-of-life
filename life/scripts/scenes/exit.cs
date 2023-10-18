/*

exit.cs

2023 Computer Science NEA
Aidan Norton

*/

using Life.Engine;
using Microsoft.Xna.Framework;

namespace Life.Scenes;

public class Exit : Scene
{
    private Vector2 windowPos;
    private float offset;
    private float multiplier;
    private float initialPos;
    public Exit(Life game) : base(game)
    {
        Life = game;
    }
        
    public override void OnEntry()
    {
        windowPos = Life.Drawing.GetWindowPosition();
        offset = 1f;
        multiplier = Life.Config.WindowAcceleration;
        initialPos = windowPos.Y;
    }

    public override void Update(GameTime dt)
    {
        // make window fall down, and close when it reaches 2 monitor heights
        if (windowPos.Y < Life.Drawing.GetDisplayHeight() * 2)
        {
            offset *= multiplier;
            windowPos.Y = offset + initialPos;
            Life.Drawing.SetWindowPosition(windowPos);
        }
        else
        {
            Life.Exit();
        }
    }

    public override void Draw(GameTime dt)
    {
            
    }
}
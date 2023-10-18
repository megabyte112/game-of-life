/*

entry.cs

2023 Computer Science NEA
Aidan Norton

*/

using Life.Engine;
using Microsoft.Xna.Framework;

namespace Life.Scenes;

public class Entry : Scene
{
    private Vector2 windowPos;
    private float offset;
    private float centrePos;
    private float multiplier;
        
    public Entry(Life game) : base(game)
    {
        Life = game;
    }

    public override void OnEntry()
    {
        windowPos.X = Life.Drawing.GetWindowPosition().X;
        float monitorHeight = Life.Drawing.GetDisplayHeight();
        centrePos = monitorHeight / 2 - Life.Drawing.GetWindowHeight() / 2;
        offset = -monitorHeight;
        multiplier = Life.Config.WindowAcceleration;
    }

    public override void Update(GameTime dt)
    {
        // make window fall down from 2 monitor heights, and switch to title scene when it reaches the centre
        if (windowPos.Y < centrePos - 1)
        {
            offset /= multiplier;
            windowPos.Y = offset + centrePos;
            Life.Drawing.SetWindowPosition(windowPos);
        }
        else
        {
            Life.SceneManager.ChangeScene(SceneId.Startup);
        }
    }

    public override void Draw(GameTime dt)
    {
            
    }
}
/*

sandbox.cs

2023 Computer Science NEA
Aidan Norton

*/

using Life.Engine;
using Life.Engine.Interface;
using Life.Engine.Simulation;
using Microsoft.Xna.Framework;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Life.Scenes;

public class Sandbox : Scene
{
    // how big the grid is
    private const int Width = 78;
    private const int Height = 38;

    // how big the cells are drawn, in pixels
    private const int CellSizeLive = 16;
    private const int CellSizeDead = 12;
        
    // border size
    private const int BorderWidth = 8;

    // the grid itself
    private readonly Grid grid;
        
    // offset from top-left corner to the proper position of the grid
    private readonly Vector2 offset;

    // define button sizes
    private const int ButtonWidth = 72;
    private const int ButtonHeight = 72;

    // buttons
    private readonly Button[] buttons;

    // create each button
    private readonly Button backButton;
    private readonly Button clearButton;
    private readonly Button randomButton;
    private readonly Button playButton;
    private readonly Button pauseButton;
    private readonly Button increaseButton;
    private readonly Button decreaseButton;

    public Sandbox(Life game) : base(game)
    {
        Life = game;
            
        // create grid
        grid = new Grid
        (
            Life, Width, Height,
            CellSizeLive, CellSizeDead,
            BorderWidth, Color.White
        );
            
        // calculate offset from top-left corner to the proper position of the grid
        offset = new Vector2(
            (Drawing.ScreenWidth / 2) - ((Width * CellSizeLive) / 2), 16f
        );
            
        backButton = new Button
        (
            "back", 8, 640, ButtonWidth, ButtonHeight, "esc", 
            Life, Life.Drawing.BackIcon
        );
        playButton = new Button
        (
            "play", Drawing.ScreenWidth - ButtonWidth - 496, 640,
            ButtonWidth, ButtonHeight, "space", Life,
            Life.Drawing.PlayIcon
        );
        pauseButton = new Button
        (
            "pause", Drawing.ScreenWidth - ButtonWidth - 496, 640,
            ButtonWidth, ButtonHeight, "space", Life,
            Life.Drawing.PauseIcon
        );
        clearButton = new Button
        (
            "clear", Drawing.ScreenWidth - ButtonWidth - 368, 640,
            ButtonWidth, ButtonHeight, "del", Life,
            Life.Drawing.TrashIcon
        );
        randomButton = new Button
        (
            "randomise", Drawing.ScreenWidth - ButtonWidth - 240, 640,
            ButtonWidth, ButtonHeight, "F5", Life,
            Life.Drawing.RefreshIcon
        );
        decreaseButton = new Button
        (
            "slower", Drawing.ScreenWidth - (2 * ButtonWidth) - 32 , 640,
            ButtonWidth, ButtonHeight, "left", Life,
            Life.Drawing.SlowerIcon
        );
        increaseButton = new Button
        (
            "faster", Drawing.ScreenWidth - ButtonWidth - 8, 640,
            ButtonWidth, ButtonHeight, "right", Life,
            Life.Drawing.FasterIcon
        );                                                                                                                          
            
        buttons = new[]
        {
            backButton,
            playButton,
            pauseButton,
            clearButton,
            randomButton,
            increaseButton,
            decreaseButton,
        };
    }

    public override void OnEntry()
    {
        grid.Play();
        grid.Clear();
        grid.SetSpeedIndex(8);
        increaseButton.IsDisabled = false;
        decreaseButton.IsDisabled = false;
    }

    public override void Update(GameTime dt)
    {
        // return to title
        if (Life.Input.WasPressed(Keys.Escape)
            || Life.Input.ClickUp(backButton))
            Life.SceneManager.ChangeScene(SceneId.Title);
            
        // toggle play/pause
        if ((Life.Input.WasPressed(Keys.Space)
             || Life.Input.ClickUp(playButton)) && !grid.IsPlaying)
            grid.Play();
        else if ((Life.Input.WasPressed(Keys.Space)
                  || Life.Input.ClickUp(pauseButton)) && grid.IsPlaying)
            grid.Pause();
            
        // clear
        if (Life.Input.WasPressed(Keys.Delete)
            || Life.Input.ClickUp(clearButton))
            grid.Clear();
            
        // randomise grid
        if (Life.Input.WasPressed(Keys.F5)
            || Life.Input.ClickUp(randomButton))
            grid.Randomise();

        // increase/decrease speed
        if (Life.Input.WasPressed(Keys.Right)
            || Life.Input.ClickUp(increaseButton))
        {
            grid.IncreaseSpeed();
            if (grid.Speed == 60) increaseButton.IsDisabled = true;
            decreaseButton.IsDisabled = false;
        }

        if (Life.Input.WasPressed(Keys.Left)
            || Life.Input.ClickUp(decreaseButton))
        {
            grid.DecreaseSpeed();
            if (grid.Speed == 1) decreaseButton.IsDisabled = true;
            increaseButton.IsDisabled = false;
        }
                
            
        // paint with the mouse
        if (Life.Input.WasPressed("lmb"))
        {
            if (grid.IsValidCell(Life.Input.MousePos, offset))
            {
                var cellpos = grid.GetCellPosAt(Life.Input.MousePos, offset);
                int x = (int)cellpos.X;
                int y = (int)cellpos.Y;
                grid.SetCell(x, y, true);
            }
        }
        if (Life.Input.IsHeld("lmb"))
        {
            // fill in the gaps between the last frame and this one
            Vector2 lastpos = Life.Input.LastMousePos;
            Vector2 thispos = Life.Input.MousePos;
            Vector2 difference = thispos - lastpos;
            float distance = difference.Length();
            Vector2 dir = difference / distance;
            for (var i = 0; i < distance; i++)
            {
                var pos = lastpos + (dir * i);
                if (grid.IsValidCell(pos, offset))
                {
                    var cellpos = grid.GetCellPosAt(pos, offset);
                    int x = (int)cellpos.X;
                    int y = (int)cellpos.Y;
                    grid.SetCell(x, y, true);
                }
            }
        }
            
        // erase with right click
        if (Life.Input.WasPressed("rmb"))
        {
            if (grid.IsValidCell(Life.Input.MousePos, offset))
            {
                var cellpos = grid.GetCellPosAt(Life.Input.MousePos, offset);
                int x = (int)cellpos.X;
                int y = (int)cellpos.Y;
                grid.SetCell(x, y, false);
            }
        }
        if (Life.Input.IsHeld("rmb"))
        {
            Vector2 lastpos = Life.Input.LastMousePos;
            Vector2 thispos = Life.Input.MousePos;
            Vector2 difference = thispos - lastpos;
            float distance = difference.Length();
            Vector2 dir = difference / distance;
            for (var i = 0; i < distance; i++)
            {
                var pos = lastpos + (dir * i);
                if (grid.IsValidCell(pos, offset))
                {
                    var cellpos = grid.GetCellPosAt(pos, offset);
                    int x = (int)cellpos.X;
                    int y = (int)cellpos.Y;
                    grid.SetCell(x, y, false);
                }
            }
        }

        // update the grid
        grid.Update(dt);
    }

    public override void Draw(GameTime dt)
    {
        // draw the grid
        grid.Draw(dt, offset);
            
        // draw buttons
        foreach (var x in buttons)
        {
            if (x.Name == "play" && grid.IsPlaying) continue;
            if (x.Name == "pause" && !grid.IsPlaying) continue;
            Life.Drawing.DrawButtonWithIcon(x);
        }
    }
}

/*

input.cs

2023 Computer Science NEA
Aidan Norton

*/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Life.Engine;

public class Input
{
    private KeyboardState kb;
    private KeyboardState lastkb;
    private MouseState ms;
    private MouseState lastms;
    private readonly Life life;

    public Input(Life life)
    {
        this.life = life;
    }

    public void Init()
    {
        // prevents variables being null in Update()
        kb = Keyboard.GetState();
        ms = Mouse.GetState();
    }

    public void Update(GameTime dt)
    {
        // keyboard and mouse input
        lastkb = kb;
        lastms = ms;
        kb = Keyboard.GetState();
        ms = Mouse.GetState();

        // do nothing if the game window is not focused
        if (!life.IsActive) return;

        // toggle keyboard shortcuts on alt key
        if (WasReleased(Keys.LeftAlt))
        {
            life.Config.ShowKeyboardShortcuts = !life.Config.ShowKeyboardShortcuts;
            life.Config.Save();
        }

        // exit on alt + F4
        // this doesn't work on windows since it handles alt + F4 by itself
        if (IsDown(Keys.LeftAlt) && IsDown(Keys.F4))
        {
            life.SceneManager.ChangeScene(SceneId.Exit);
        }
    }

    public Vector2 MousePos => ms.Position.ToVector2();
    public Vector2 LastMousePos => lastms.Position.ToVector2();
    private int MouseX => ms.X;
    private int MouseY => ms.Y;

    // returns true if a given key was just pressed down on this frame
    public bool WasPressed(Keys key)
    {
        return kb.IsKeyDown(key) && lastkb.IsKeyUp(key);
    }

    // same as above but for mouse
    public bool WasPressed(string str)
    {
        return str switch
        {
            "lmb" => ms.LeftButton == ButtonState.Pressed &&
                     lastms.LeftButton == ButtonState.Released,
            "rmb" => ms.RightButton == ButtonState.Pressed &&
                     lastms.RightButton == ButtonState.Released,
            _ => throw new ArgumentException()
        };
    }

    // returns true if a key was just released this frame
    public bool WasReleased(Keys key)
    {
        return kb.IsKeyUp(key) && lastkb.IsKeyDown(key);
    }

    // same as above but for mouse
    public bool WasReleased(string str)
    {
        return str switch
        {
            "lmb" => ms.LeftButton == ButtonState.Released &&
                     lastms.LeftButton == ButtonState.Pressed,
            "rmb" => ms.RightButton == ButtonState.Released &&
                     lastms.RightButton == ButtonState.Pressed,
            _ => throw new ArgumentException()
        };
    }

    // returns true if a key is currently pressed
    public bool IsDown(Keys key)
    {
        return kb.IsKeyDown(key);
    }

    // same as above but for mouse
    public bool IsDown(string str)
    {
        return str switch
        {
            "lmb" => ms.LeftButton == ButtonState.Pressed,
            "rmb" => ms.RightButton == ButtonState.Pressed,
            _ => throw new ArgumentException()
        };
    }

    // returns true if a key is currently held
    public bool IsHeld(Keys key)
    {
        return kb.IsKeyDown(key) && lastkb.IsKeyDown(key);
    }

    // same as above but for mouse
    public bool IsHeld(string str)
    {
        return str switch
        {
            "lmb" => ms.LeftButton == ButtonState.Pressed &&
                     lastms.LeftButton == ButtonState.Pressed,
            "rmb" => ms.RightButton == ButtonState.Pressed &&
                     lastms.RightButton == ButtonState.Pressed,
            _ => throw new ArgumentException()
        };
    }

    // returns true if the mouse cursor is above the object
    public bool IsHover(Object obj)
    {
        // allow extra 20% of the object's width and height for padding,
        // takes button magnetism option into account
        var width = obj.Width * (1f + (0.125f * life.Config.ButtonMagneticMultiplier));
        var height = obj.Height * (1f + (0.125f * life.Config.ButtonMagneticMultiplier));
        var x = obj.X - obj.Width * (0.125f * life.Config.ButtonMagneticMultiplier);
        var y = obj.Y - obj.Height * (0.125f * life.Config.ButtonMagneticMultiplier);

        return MouseX > x && MouseX < x + width &&
               MouseY > y && MouseY < y + height;
    }

    // returns true if the object is being clicked
    public bool IsClicked(Object obj)
    {
        return IsHover(obj) && IsDown("lmb");
    }
    public bool IsRightClicked(Object obj)
    {
        return IsHover(obj) && IsDown("rmb");
    }

    // returns true if the object has just started being clicked
    public bool ClickDown(Object obj)
    {
        return IsHover(obj) && WasPressed("lmb");
    }
    public bool RightClickDown(Object obj)
    {
        return IsHover(obj) && WasPressed("rmb");
    }

    // returns true if the object has just stopped being clicked
    public bool ClickUp(Object obj)
    {
        return IsHover(obj) && WasReleased("lmb");
    }
    public bool RightClickUp(Object obj)
    {
        return IsHover(obj) && WasReleased("rmb");
    }
}
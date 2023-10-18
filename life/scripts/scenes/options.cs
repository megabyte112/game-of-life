/*

options.cs

2023 Computer Science NEA
Aidan Norton

*/

using Life.Engine;
using Life.Engine.Interface;
using System;
using Microsoft.Xna.Framework;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Life.Scenes;

public class Options : Scene
{
    private const int ButtonSize = 72;
    private readonly Button[] buttons;
    private readonly Button backButton;
    private readonly Button resetButton;
    private readonly Button increaseCornerRadiusButton;
    private readonly Button decreaseCornerRadiusButton;
    private readonly Button increaseParallaxMultiplierButton;
    private readonly Button decreaseParallaxMultiplierButton;
    private readonly Button increaseButtonMagneticMultiplierButton;
    private readonly Button decreaseButtonMagneticMultiplierButton;
    private readonly Button increaseWindowAccelerationButton;
    private readonly Button decreaseWindowAccelerationButton;
    private readonly Button increaseFadeMultiplierButton;
    private readonly Button decreaseFadeMultiplierButton;
    private readonly Button toggleKeyboardShortcutsButton;

    // sys info
    private readonly string name = "life";
    private readonly string os = GetOS();

    private Vector2 namePos;
    private Vector2 osPos;

    public Options(Life game) : base(game) 
    {
        Life = game;

        // buttons
        backButton = new Button
        (
            "back", 32, 608, ButtonSize, ButtonSize, "esc",
            Life, Life.Drawing.BackIcon
        );
        resetButton = new Button
        (
            "reset", (int)Life.Drawing.GetWindowWidth() - 2 * (ButtonSize + 32),
            608, ButtonSize, ButtonSize, "del",
            Life, Life.Drawing.ResetIcon
        );
        increaseCornerRadiusButton = new Button
        (
            "cornerup", 128, 32, ButtonSize, ButtonSize, "",
            Life, Life.Drawing.PlusIcon
        );
        decreaseCornerRadiusButton = new Button
        (
            "cornerdown", 32, 32, ButtonSize, ButtonSize, "",
            Life, Life.Drawing.MinusIcon
        );
        increaseParallaxMultiplierButton = new Button
        (
            "parallaxup", 128, 128, ButtonSize, ButtonSize, "",
            Life, Life.Drawing.PlusIcon
        );
        decreaseParallaxMultiplierButton = new Button
        (
            "parallaxdown", 32, 128, ButtonSize, ButtonSize, "",
            Life, Life.Drawing.MinusIcon
        );
        increaseButtonMagneticMultiplierButton = new Button
        (
            "magnetup", 128, 224, ButtonSize, ButtonSize, "",
            Life, Life.Drawing.PlusIcon
        );
        decreaseButtonMagneticMultiplierButton = new Button
        (
            "magnetdown", 32, 224, ButtonSize, ButtonSize, "",
            Life, Life.Drawing.MinusIcon
        );
        increaseWindowAccelerationButton = new Button
        (
            "windowup", 128, 320, ButtonSize, ButtonSize, "",
            Life, Life.Drawing.PlusIcon
        );
        decreaseWindowAccelerationButton = new Button
        (
            "windowdown", 32, 320, ButtonSize, ButtonSize, "",
            Life, Life.Drawing.MinusIcon
        );
        increaseFadeMultiplierButton = new Button
        (
            "fadeup", 128, 416, ButtonSize, ButtonSize, "",
            Life, Life.Drawing.PlusIcon
        );
        decreaseFadeMultiplierButton = new Button
        (
            "fadedown", 32, 416, ButtonSize, ButtonSize, "",
            Life, Life.Drawing.MinusIcon
        );
        toggleKeyboardShortcutsButton = new Button
        (
            "kbshortcut", 32, 512, ButtonSize, ButtonSize, "alt",
            Life, Life.Drawing.KeyboardIcon
        );

        buttons = new[]
        {
            backButton,
            resetButton,
            increaseCornerRadiusButton,
            decreaseCornerRadiusButton,
            increaseParallaxMultiplierButton,
            decreaseParallaxMultiplierButton,
            increaseWindowAccelerationButton,
            decreaseWindowAccelerationButton,
            increaseButtonMagneticMultiplierButton,
            decreaseButtonMagneticMultiplierButton,
            increaseFadeMultiplierButton,
            decreaseFadeMultiplierButton,
            toggleKeyboardShortcutsButton
        };
    }

    public override void OnEntry()
    {
        var size = Life.Drawing.GetSmallTextSize(name);
        namePos = new Vector2(
            Drawing.ScreenWidth - size.X,
            Drawing.ScreenHeight - (2 * size.Y) - 10
        );

        size = Life.Drawing.GetSmallTextSize(os);
        osPos = new Vector2(
            Drawing.ScreenWidth - size.X,
            Drawing.ScreenHeight - size.Y - 5
        );
            
        // disable buttons if they can't be used
        if (Life.Config.BorderRadius <= 0)
            decreaseCornerRadiusButton.IsDisabled = true;
        else if (Life.Config.BorderRadius >= 20)
            increaseCornerRadiusButton.IsDisabled = true;
        if (Life.Config.ParallaxMultiplier <= 0f)
            decreaseParallaxMultiplierButton.IsDisabled = true;
        else if (Life.Config.ParallaxMultiplier >= 2f)
            increaseParallaxMultiplierButton.IsDisabled = true;
        if (Life.Config.WindowAcceleration <= 1.03125f)
            decreaseWindowAccelerationButton.IsDisabled = true;
        else if (Life.Config.WindowAcceleration >= 2f)
            increaseWindowAccelerationButton.IsDisabled = true;
        if (Life.Config.ButtonMagneticMultiplier <= 0f)
            decreaseButtonMagneticMultiplierButton.IsDisabled = true;
        else if (Life.Config.ButtonMagneticMultiplier >= 2f)
            increaseButtonMagneticMultiplierButton.IsDisabled = true;
        if (Life.Config.FadeMultiplier <= 1f)
            decreaseFadeMultiplierButton.IsDisabled = true;
        else if (Life.Config.FadeMultiplier >= 20f)
            increaseFadeMultiplierButton.IsDisabled = true;
    }

    public override void Update(GameTime dt)
    {
        if (Life.Input.WasPressed(Keys.Escape) || Life.Input.ClickUp(backButton))
        {
            Life.Config.Save();
            Life.SceneManager.ChangeScene(SceneId.Title);
        }
        else if (Life.Input.WasPressed(Keys.Delete) || Life.Input.ClickUp(resetButton))
        {
            Life.Config.Reset();
            foreach (var x in buttons) x.IsDisabled = false;
        }

        // buttons
        if (Life.Input.ClickUp(increaseCornerRadiusButton))
        {
            Life.Config.BorderRadius += 1;

            if (decreaseCornerRadiusButton.IsDisabled)
                decreaseCornerRadiusButton.IsDisabled = false;

            if (Life.Config.BorderRadius >= 20)
            {
                Life.Config.BorderRadius = 20;
                increaseCornerRadiusButton.IsDisabled = true;
            }
        }
        else if (Life.Input.ClickUp(decreaseCornerRadiusButton))
        {
            Life.Config.BorderRadius -= 1;

            if (increaseCornerRadiusButton.IsDisabled)
                increaseCornerRadiusButton.IsDisabled = false;

            if (Life.Config.BorderRadius <= 0)
            {
                Life.Config.BorderRadius = 0;
                decreaseCornerRadiusButton.IsDisabled = true;
            }
        }
        else if (Life.Input.ClickUp(increaseParallaxMultiplierButton))
        {
            Life.Config.ParallaxMultiplier = (float)Math.Round(
                Life.Config.ParallaxMultiplier + 0.1f, 1
            );

            if (decreaseParallaxMultiplierButton.IsDisabled)
                decreaseParallaxMultiplierButton.IsDisabled = false;

            if (Life.Config.ParallaxMultiplier >= 2f)
            {
                Life.Config.ParallaxMultiplier = 2f;
                increaseParallaxMultiplierButton.IsDisabled = true;
            }
        }
        else if (Life.Input.ClickUp(decreaseParallaxMultiplierButton))
        {
            Life.Config.ParallaxMultiplier = (float)Math.Round(
                Life.Config.ParallaxMultiplier - 0.1f, 1
            );

            if (increaseParallaxMultiplierButton.IsDisabled)
                increaseParallaxMultiplierButton.IsDisabled = false;

            if (Life.Config.ParallaxMultiplier <= 0f)
            {
                Life.Config.ParallaxMultiplier = 0f;
                decreaseParallaxMultiplierButton.IsDisabled = true;
            }
        }
        else if (Life.Input.ClickUp(increaseButtonMagneticMultiplierButton))
        {
            Life.Config.ButtonMagneticMultiplier = (float)Math.Round(
                Life.Config.ButtonMagneticMultiplier + 0.1f, 1
            );

            if (decreaseButtonMagneticMultiplierButton.IsDisabled)
                decreaseButtonMagneticMultiplierButton.IsDisabled = false;

            if (Life.Config.ButtonMagneticMultiplier >= 2f)
            {
                Life.Config.ButtonMagneticMultiplier = 2f;
                increaseButtonMagneticMultiplierButton.IsDisabled = true;
            }
        }
        else if (Life.Input.ClickUp(decreaseButtonMagneticMultiplierButton))
        {
            Life.Config.ButtonMagneticMultiplier = (float)Math.Round(
                Life.Config.ButtonMagneticMultiplier - 0.1f, 1
            );

            if (increaseButtonMagneticMultiplierButton.IsDisabled)
                increaseButtonMagneticMultiplierButton.IsDisabled = false;

            if (Life.Config.ButtonMagneticMultiplier <= 0f)
            {
                Life.Config.ButtonMagneticMultiplier = 0f;
                decreaseButtonMagneticMultiplierButton.IsDisabled = true;
            }
        }
        else if (Life.Input.ClickUp(increaseWindowAccelerationButton))
        {
            var tmp = Life.Config.WindowAcceleration - 1f;
            tmp *= 2f;
            Life.Config.WindowAcceleration = 1 + tmp;

            if (decreaseWindowAccelerationButton.IsDisabled)
                decreaseWindowAccelerationButton.IsDisabled = false;

            if (Life.Config.WindowAcceleration >= 2f)
            {
                Life.Config.WindowAcceleration = 2f;
                increaseWindowAccelerationButton.IsDisabled = true;
            }
        }
        else if (Life.Input.ClickUp(decreaseWindowAccelerationButton))
        {
            var tmp = Life.Config.WindowAcceleration - 1f;
            tmp /= 2f;
            Life.Config.WindowAcceleration = 1 + tmp;

            if (increaseWindowAccelerationButton.IsDisabled)
                increaseWindowAccelerationButton.IsDisabled = false;

            if (Life.Config.WindowAcceleration <= 1.03125f)
            {
                Life.Config.WindowAcceleration = 1.03125f;
                decreaseWindowAccelerationButton.IsDisabled = true;
            }
        }
        else if (Life.Input.ClickUp(increaseFadeMultiplierButton))
        {
            Life.Config.FadeMultiplier += 1f;

            if (decreaseFadeMultiplierButton.IsDisabled)
                decreaseFadeMultiplierButton.IsDisabled = false;

            if (Life.Config.FadeMultiplier >= 20f)
            {
                Life.Config.FadeMultiplier = 20f;
                increaseFadeMultiplierButton.IsDisabled = true;
            }
        }
        else if (Life.Input.ClickUp(decreaseFadeMultiplierButton))
        {
            Life.Config.FadeMultiplier -= 1f;

            if (increaseFadeMultiplierButton.IsDisabled)
                increaseFadeMultiplierButton.IsDisabled = false;

            if (Life.Config.FadeMultiplier <= 1f)
            {
                Life.Config.FadeMultiplier = 1f;
                decreaseFadeMultiplierButton.IsDisabled = true;
            }
        }
        else if (Life.Input.ClickUp(toggleKeyboardShortcutsButton))
            Life.Config.ShowKeyboardShortcuts = !Life.Config.ShowKeyboardShortcuts;
    }

    public override void Draw(GameTime dt)
    {
        foreach (var x in buttons)
        {
            Life.Drawing.DrawButtonWithIcon(x);
        }
            
        // button text
        Life.Drawing.TypeMedium("Corner Radius: "+ Life.Config.BorderRadius + "  (restart required)", new Vector2(220, 45), 0.5f);
        Life.Drawing.TypeMedium("Parallax Multiplier: " + Life.Config.ParallaxMultiplier, new Vector2(220, 141), 0.5f);
        Life.Drawing.TypeMedium("Button Magnetism Multiplier: " + Life.Config.ButtonMagneticMultiplier, new Vector2(220, 237), 0.5f);
        Life.Drawing.TypeMedium("Window Acceleration: " + Life.Config.WindowAcceleration, new Vector2(220, 333), 0.5f);
        Life.Drawing.TypeMedium("Fade Multiplier: " + Life.Config.FadeMultiplier, new Vector2(220, 429), 0.5f);
        Life.Drawing.TypeMedium("Toggle Keyboard Shortcuts", new Vector2(124, 525), 0.5f);
        Life.Drawing.TypeMedium("Reset", new Vector2(1058, 560), 0.5f);
            

        // system info
        Life.Drawing.TypeSmall(name, namePos, 0.25f);
        Life.Drawing.TypeSmall(os, osPos, 0.25f);
    }

    private static string GetOS()
    {
        return Environment.OSVersion.VersionString;
    }
}
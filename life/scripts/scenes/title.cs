/*

title.cs

2023 Computer Science NEA
Aidan Norton

*/

using Life.Engine;
using Life.Engine.Interface;
using Life.Engine.Simulation;
using Microsoft.Xna.Framework;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Life.Scenes;

public class Title : Scene
{
    // button dimensions
    private const int ButtonWidth = 480;
    private const int ButtonHeight = 120;

    // where text is drawn inside the button
    private static readonly Vector2 ButtonTextOffset = new Vector2(40, 12);

    private readonly Button[] buttons;

    private readonly Button startButton;
    private readonly Button tutorialButton;
    private readonly Button optionButton;
    private readonly Button closeButton;

    private Vector2 parallaxOffset;
        
    // background grid
    private readonly Grid grid;
    private static readonly Vector2 BackgroundOffset = new Vector2(-2500, -1400);

    public Title(Life game) : base(game)
    {
        Life = game;

        // create each button
        startButton = new Button
        (
            "Sandbox", 20, 100, ButtonWidth, ButtonHeight,
            "space", Life, null
        );
        tutorialButton = new Button
        (
            "Guide", 20, 250, ButtonWidth, ButtonHeight,
            "right", Life, null
        );
        optionButton = new Button
        (
            "Options", 20, 400, ButtonWidth, ButtonHeight,
            "backspace", Life, null
        );
        closeButton = new Button
        (
            "Exit", 20, 550, ButtonWidth, ButtonHeight,
            "esc", Life, null
        );
        buttons = new[]
        {
            startButton, tutorialButton, optionButton, closeButton
        };
            
        // setup background grid
        grid = new Grid(
            Life, 100, 40 ,64, 0, 0, new Color(95, 95, 95) * 0.125f
        );
            
        grid.SetSpeedIndex(2);
    }

    public override void OnEntry()
    {
        grid.Randomise();
    }

    public override void Update(GameTime dt)
    {
        // parallax depending on mouse position
        parallaxOffset = new Vector2(
            Life.Input.MousePos.X / 100f,
            Life.Input.MousePos.Y / 100f
        );
        parallaxOffset *= Life.Config.ParallaxMultiplier;
            
        // update background grid
        grid.Update(dt);
            
        // buttons
        if (Life.Input.ClickUp(startButton) || Life.Input.WasPressed(Keys.Space))
            Life.SceneManager.ChangeScene(SceneId.Sim);
            
        else if (Life.Input.ClickUp(tutorialButton) || Life.Input.WasPressed(Keys.Right))
            Life.SceneManager.ChangeScene(SceneId.Tutorial);

        else if (Life.Input.ClickUp(optionButton) || Life.Input.WasPressed(Keys.Back))
            Life.SceneManager.ChangeScene(SceneId.Options);

        else if (Life.Input.ClickUp(closeButton) || Life.Input.WasPressed(Keys.Escape))
            Life.SceneManager.ChangeScene(SceneId.Exit);
            
        // randomise background grid on F5
        if (Life.Input.WasPressed(Keys.F5)) grid.Randomise();
    }
    public override void Draw(GameTime dt)
    {
        // draw background grid
        grid.Draw(dt, BackgroundOffset - (5 * parallaxOffset));
            
        // draw buttons
        foreach (var x in buttons)
        {
            Life.Drawing.DrawButtonWithText(x, ButtonTextOffset, parallaxOffset);
        }
    }
}
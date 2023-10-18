/*

startup.cs

2023 Computer Science NEA
Aidan Norton

*/

using Life.Engine;
using Life.Engine.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Life.Scenes;

public class Startup : Scene
{
    private readonly Button confirmButton;
    private readonly Vector2 line1CentrePos;
    private readonly Vector2 line2CentrePos;
    private readonly Vector2 line3CentrePos;
    private const string Line1Message = "Conway's Game of Life";
    private const string Line2Message = "2023 Computer Science NEA";
    private const string Line3Message = "Click the button to start";

    public Startup(Life game) : base(game)
    {
        Life = game;
            
        confirmButton = new Button
        (
            "Confirm", 592, 500, 96, 96,
            "enter", Life, Life.Drawing.ConfirmIcon
        );
            
        line1CentrePos.X = Life.Drawing.GetWindowWidth()/2 - Life.Drawing.GetMediumTextSize(Line1Message).X/2;
        line1CentrePos.Y = 200f;
        line2CentrePos.X = Life.Drawing.GetWindowWidth()/2 - Life.Drawing.GetMediumTextSize(Line2Message).X/2;
        line2CentrePos.Y = 280f;
        line3CentrePos.X = Life.Drawing.GetWindowWidth()/2 - Life.Drawing.GetMediumTextSize(Line3Message).X/2;
        line3CentrePos.Y = 360f;
    }

    public override void OnEntry()
    {
            
    }

    public override void Update(GameTime dt)
    {
        if (Life.Input.ClickUp(confirmButton) || Life.Input.WasReleased(Keys.Enter))
        {
            Life.SceneManager.ChangeScene(SceneId.Title);
        }
    }

    public override void Draw(GameTime dt)
    {
        Life.Drawing.TypeMedium(Line1Message, line1CentrePos, 0.5f);
        Life.Drawing.TypeMedium(Line2Message, line2CentrePos, 0.5f);
        Life.Drawing.TypeMedium(Line3Message, line3CentrePos, 0.5f);
        Life.Drawing.DrawButtonWithIcon(confirmButton);
    }
}

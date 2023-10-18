/*

main.cs

2023 Computer Science NEA
Aidan Norton

*/

using Life.Engine;
using Microsoft.Xna.Framework;

namespace Life;

public class Life : Game
{
    // used for rendering anything
    public readonly Drawing Drawing;

    // handles mouse and keyboard input
    public readonly Input Input;
        
    // scene manager
    public SceneManager SceneManager;
        
    // configuration options
    public readonly Configuration Config;

    public Life()
    {
        // initialise rendering and input
        Drawing = new Drawing(new GraphicsDeviceManager(this), this);
        Input = new Input(this);

        // set assets folder so that assets can be imported
        Content.RootDirectory = "assets";
            
        // load configuration
        Config = new Configuration();

        // initialise input
        Input.Init();
    }

    // runs once the program starts
    protected override void Initialize()
    {
        // SceneManager needs the textures to be loaded, which is done through LoadContent().
        // LoadContent() is called during base.Initialize(), so SceneManager needs to be initialised after that.
        base.Initialize();
        SceneManager = new SceneManager(this);
        Drawing.Init();
    }

    // this is intended for importing content, but the drawing class can handle this
    protected override void LoadContent() { Drawing.SpritebatchInit(); }

    // runs once per frame: 60 times per second
    protected override void Update(GameTime dt)
    {
        Input.Update(dt);
        SceneManager.Update(dt);
        base.Update(dt);
    }

    // same with this: runs 60 times a second
    protected override void Draw(GameTime dt)
    {
        Drawing.Draw(dt);
        base.Draw(dt);
    }
}

// program entrypoint
internal static class Program
{
    private static void Main()
    {
        using var game = new Life();
        game.Run();
    }
}
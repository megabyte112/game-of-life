/*

scene.cs

2023 Computer Science NEA
Aidan Norton

*/

using Life.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Life.Engine;

public class SceneManager
{
    private readonly Life life;
    private readonly Texture2D fadeTexture;
    private float fadeAlpha;
    private bool fadingIn;
    private bool fadingOut;
    private Scene fadingToScene;
    private readonly Scene[] scenes;

    private Scene CurrentScene { get; set; }
        
    public SceneManager(Life game)
    {
        life = game;
        fadeTexture = life.Drawing.GetRectangle(1, 1, life.Config.BackgroundColor);
        fadeAlpha = 1;
        fadingIn = true;
        fadingOut = false;
            
        scenes = new Scene[System.Enum.GetNames(typeof(SceneId)).Length];
        scenes[(int)SceneId.Entry] = new Entry(life);
        scenes[(int)SceneId.Exit] = new Exit(life);
        scenes[(int)SceneId.Title] = new Title(life);
        scenes[(int)SceneId.Sim] = new Sandbox(life);
        scenes[(int)SceneId.Options] = new Options(life);
        scenes[(int)SceneId.Tutorial] = new Tutorial(life);
        scenes[(int)SceneId.Startup] = new Startup(life);

        // first scene
        fadingToScene = GetScene(SceneId.Entry);
        SwitchScene();
    }
        
    public void Update(GameTime dt)
    {
        CurrentScene.Update(dt);
        if (fadingOut)
        {
            fadeAlpha += 0.01f * life.Config.FadeMultiplier;
            if (fadeAlpha >= 1)
            {
                SwitchScene();
            }
        }
        else if (fadingIn)
        {
            fadeAlpha -= 0.01f * life.Config.FadeMultiplier;
            if (fadeAlpha <= 0)
            {
                fadingIn = false;
            }
        }
    }
        
    public void Draw(GameTime dt)
    {
        CurrentScene.Draw(dt);
        if (fadingOut || fadingIn)
        {
            life.Drawing.DrawTexture(fadeTexture, 
                new Rectangle(0, 0, Drawing.ScreenWidth, Drawing.ScreenHeight), fadeAlpha);
        }
    }
        
    public void ChangeScene(SceneId id)
    {
        fadingToScene = GetScene(id);
        fadingOut = true;
    }
        
    private void SwitchScene()
    {
        CurrentScene = fadingToScene;
        CurrentScene.OnEntry();
        fadingIn = true;
        fadingOut = false;
    }

    private Scene GetScene(SceneId id)
    {
        return scenes[(int)id];
    }
}
    
public abstract class Scene
{
    protected static Life Life;

    protected Scene(Life game)
    {
        Life = game;
    }

    public abstract void OnEntry();
    public abstract void Update(GameTime dt);
    public abstract void Draw(GameTime dt);
}

public enum SceneId
{
    Entry,
    Exit,
    Title,
    Sim,
    Options,
    Tutorial,
    Startup
}
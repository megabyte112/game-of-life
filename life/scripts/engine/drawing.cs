/*

drawing.cs

2023 Computer Science NEA
Aidan Norton

*/

using Life.Engine.Interface;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Life.Engine;

public class Drawing
{
    private readonly Life life;
    private SpriteBatch spriteBatch;
    private readonly GraphicsDevice gd;
    private readonly GameWindow win;

    // fonts
    private SpriteFont bigFont;
    private SpriteFont normalFont;
    private SpriteFont smallFont;

    // icons
    public Texture2D PlayIcon;
    public Texture2D PauseIcon;
    public Texture2D RefreshIcon;
    public Texture2D BackIcon;
    public Texture2D FasterIcon;
    public Texture2D SlowerIcon;
    public Texture2D TrashIcon;
    public Texture2D PlusIcon;
    public Texture2D MinusIcon;
    public Texture2D KeyboardIcon;
    public Texture2D ResetIcon;
    public Texture2D LeftArrowIcon;
    public Texture2D RightArrowIcon;
    public Texture2D ConfirmIcon;

    public const int ScreenWidth = 1280;
    public const int ScreenHeight = 720;

    public Drawing(GraphicsDeviceManager g, Life game)
    {
        life = game;

        // set resolution
        g.PreferredBackBufferWidth = ScreenWidth;
        g.PreferredBackBufferHeight = ScreenHeight;
        g.ApplyChanges();

        // lock framerate to 60fps for consistency across hardware
        life.IsFixedTimeStep = true;
        life.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
        life.IsMouseVisible = true;
        gd = life.GraphicsDevice;
        win = life.Window;
        win.Title = "Life";
        win.AllowAltF4 = false;
    }

    // initialises the spritebatch, this must be
    // called within the LoadContent() method
    public void SpritebatchInit()
    {
        spriteBatch = new SpriteBatch(gd);

        // fonts
        smallFont = life.Content.Load<SpriteFont>("font/smallfont");
        normalFont = life.Content.Load<SpriteFont>("font/regularfont");
        bigFont = life.Content.Load<SpriteFont>("font/largefont");

        // https://icons8.com/icon/5722/play
        PlayIcon = life.Content.Load<Texture2D>("icons/play");

        // https://icons8.com/icon/85882/pause
        PauseIcon = life.Content.Load<Texture2D>("icons/pause");

        // https://icons8.com/icon/91479/shuffle
        RefreshIcon = life.Content.Load<Texture2D>("icons/shuffle");

        // https://icons8.com/icon/v53GNyzjXLD5/u-turn-to-left
        BackIcon = life.Content.Load<Texture2D>("icons/back");

        // https://icons8.com/icon/87484/fast-forward
        FasterIcon = life.Content.Load<Texture2D>("icons/faster");

        // https://icons8.com/icon/5716/rewind
        SlowerIcon = life.Content.Load<Texture2D>("icons/slower");

        // https://icons8.com/icon/100064/clear-symbol
        TrashIcon = life.Content.Load<Texture2D>("icons/clear");

        // https://icons8.com/icon/62888/plus-math
        PlusIcon = life.Content.Load<Texture2D>("icons/plus");

        // https://icons8.com/icon/79029/subtract
        MinusIcon = life.Content.Load<Texture2D>("icons/minus");

        // https://icons8.com/icon/60677/keyboard
        KeyboardIcon = life.Content.Load<Texture2D>("icons/keyboard");

        // https://icons8.com/icon/86198/reset
        ResetIcon = life.Content.Load<Texture2D>("icons/reset");

        // https://icons8.com/icon/99996/left-arrow
        LeftArrowIcon = life.Content.Load<Texture2D>("icons/left");

        // https://icons8.com/icon/99982/right-arrow
        RightArrowIcon = life.Content.Load<Texture2D>("icons/right");

        // https://icons8.com/icon/98955/done
        ConfirmIcon = life.Content.Load<Texture2D>("icons/confirm");
    }

    public void Init()
    {
        
    }

    public void Draw(GameTime dt)
    {
        gd.Clear(life.Config.BackgroundColor);
        spriteBatch.Begin();
        life.SceneManager.Draw(dt);
        spriteBatch.End();
    }

    public void DrawTexture(Texture2D texture, Vector2 pos, float opacity)
    {
        spriteBatch.Draw(texture, pos, Color.White * opacity);
    }

    public void TypeSmall(string text, Vector2 pos, float opacity)
    {
        spriteBatch.DrawString(smallFont, text, pos, Color.White * opacity);
    }

    public void TypeMedium(string text, Vector2 pos, float opacity)
    {
        spriteBatch.DrawString(normalFont, text, pos, Color.White * opacity);
    }

    public void TypeLarge(string text, Vector2 pos, float opacity)
    {
        spriteBatch.DrawString(bigFont, text, pos, Color.White * opacity);
    }

    public void DrawTexture(Texture2D texture, Rectangle rect, float opacity)
    {
        spriteBatch.Draw(texture, rect, Color.White * opacity);
    }

    public void DrawButtonWithText(Button btn, Vector2 textOffset, Vector2 parallaxOffset)
    {
        float opacity;
        
        Vector2 buttonCentre = new Vector2(btn.X + btn.Width / 2, btn.Y + btn.Height / 2);

        if (btn.IsDisabled) opacity = 0.2f;
        else if (life.Input.IsClicked(btn)) opacity = 0.4f;
        else if (life.Input.IsHover(btn))
        {
            // button magnetism
            parallaxOffset = -((buttonCentre - life.Input.MousePos) / 10) * life.Config.ButtonMagneticMultiplier;
            opacity = 0.8f;
        }
        else opacity = 0.6f;
        
        spriteBatch.Draw(btn.Texture, btn.Position + parallaxOffset, Color.White * opacity);
        spriteBatch.DrawString(bigFont, btn.Name, btn.Position + parallaxOffset + textOffset, Color.Black * opacity);
        if (!string.IsNullOrEmpty(btn.Key) && life.Config.ShowKeyboardShortcuts)
        {
            Vector2 keyOffset = new Vector2(textOffset.X, textOffset.Y - 4);
            spriteBatch.DrawString(smallFont, btn.Key,
                btn.Position + keyOffset + parallaxOffset, Color.Black * opacity);
        }
    }

    public void DrawButtonWithIcon(Button btn)
    {
        float opacity;
        
        Vector2 iconOffset = new Vector2(
            btn.Width / 2 - (float)btn.IconTexture.Width / 2,
            btn.Height / 2 - (float)btn.IconTexture.Height / 2
        );
        
        Vector2 buttonCentre = new Vector2(btn.X + btn.Width / 2, btn.Y + btn.Height / 2);
        Vector2 parallaxOffset = Vector2.Zero;

        if (btn.IsDisabled) opacity = 0.2f;
        else if (life.Input.IsClicked(btn)) opacity = 0.4f;
        else if (life.Input.IsHover(btn))
        {
            // button magnetism
            parallaxOffset = -((buttonCentre - life.Input.MousePos) / 10) * life.Config.ButtonMagneticMultiplier;
            opacity = 0.8f;
        }
        else opacity = 0.6f;
        
        spriteBatch.Draw(btn.Texture, btn.Position + parallaxOffset, Color.White * opacity);

        // keyboard shortcuts
        if (!string.IsNullOrEmpty(btn.Key) && life.Config.ShowKeyboardShortcuts)
        {
            spriteBatch.Draw(btn.IconTexture,
                btn.Position + parallaxOffset + iconOffset + new Vector2(0, btn.Height / 10),
                Color.White * opacity);

            Vector2 keyOffset;
            // edge case for keyboard shortcut toggle button
            if (btn.Name == "kbshortcut") keyOffset = new Vector2(-smallFont.MeasureString(btn.Key).X / 2 + btn.Width / 2, iconOffset.Y - 4);
            else keyOffset = new Vector2(-smallFont.MeasureString(btn.Key).X / 2 + btn.Width / 2, iconOffset.Y - 10);
            spriteBatch.DrawString(smallFont, btn.Key,
                btn.Position + keyOffset + parallaxOffset, Color.Black * opacity);
        }
        else spriteBatch.Draw(btn.IconTexture,
            btn.Position + parallaxOffset + iconOffset, Color.White * opacity);
    }

    // returns a rectangle texture
    public Texture2D GetRectangle(int width, int height, Color colour)
    {
        Texture2D texture = new Texture2D(gd, width, height);
        Color[] data = new Color[width * height];

        for (int a = 0; a < height; a++)
        {
            for (int b = 0; b < width; b++)
            {
                data[a * width + b] = colour;
            }
        }

        texture.SetData(data);
        return texture;
    }

    // returns a texture of a rectangle with rounded off corners
    public Texture2D GetRoundedRectangle(int width, int height, Color colour)
    {
        int radius = life.Config.BorderRadius;
        radius++;
        int sizeX = width;
        int sizeY = height;
        width -= 2 * radius;
        height -= 2 * radius;

        Texture2D texture = new Texture2D(gd, sizeX, sizeY);
        Color[] data = new Color[sizeX * sizeY];

        // position of each corner
        Vector2[] corners =
        {
            new Vector2(radius - 1, radius - 1),
            new Vector2(width + radius, radius - 1),
            new Vector2(radius - 1, height + radius),
            new Vector2(width + radius, height + radius)
        };

        for (int a = 0; a < sizeY; a++)
        {
            for (int b = 0; b < sizeX; b++)
            {
                // check if this pixel is within the rectangle
                Vector2 point = new Vector2(b, a);
                if ((a >= radius || b >= radius) &&
                    (a <= sizeY - radius || b <= sizeX - radius) &&
                    (a >= radius || b <= sizeX - radius) &&
                    (a <= sizeY - radius || b >= radius)
                   )
                {
                    data[a * sizeX + b] = colour;
                }
                else
                {
                    // check if this pixel is within the radius of a corner
                    foreach (var x in corners)
                    {
                        if (Math.Abs(Vector2.Distance(point, x)) < radius)
                        {
                            data[a * sizeX + b] = colour;
                        }
                    }
                }
            }
        }
        texture.SetData(data);
        return texture;
    }

    public Vector2 GetWindowPosition()
    {
        return new Vector2(win.ClientBounds.X, win.ClientBounds.Y);
    }

    public void SetWindowPosition(Vector2 pos)
    {
        win.Position = new Point((int)pos.X, (int)pos.Y);
    }

    // returns the height of the display
    public float GetDisplayHeight()
    {
        return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
    }

    public float GetDisplayWidth()
    {
        return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
    }

    public float GetWindowHeight()
    {
        return win.ClientBounds.Height;
    }

    public float GetWindowWidth()
    {
        return win.ClientBounds.Width;
    }

    public Vector2 GetSmallTextSize(string text)
    {
        return smallFont.MeasureString(text);
    }

    public Vector2 GetMediumTextSize(string text)
    {
        return normalFont.MeasureString(text);
    }

    public Vector2 GetLargeTextSize(string text)
    {
        return bigFont.MeasureString(text);
    }
}
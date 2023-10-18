/*

tutorial.cs

2023 Computer Science NEA
Aidan Norton

*/

using Life.Engine;
using Life.Engine.Interface;
using Life.Engine.Simulation;
using Microsoft.Xna.Framework;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Life.Scenes;

public class Tutorial : Scene
{
    // grid
    private readonly Grid grid;

    // how far from the top-left corner the grid is drawn
    private readonly Vector2 offset = new(12f, 12f);

    // what "page" the tutorial is on
    private int tutorialPhase;
    
    private const int MaxPhase = 6;

    private readonly Button nextButton;
    private readonly Button prevButton;
    private readonly Button backButton;
    private readonly Button resetButton;
    private readonly Button playButton;
    private readonly Button pauseButton;
    private readonly Button sandboxButton;
    private readonly Vector2 sandboxButtonTextOffset = new Vector2(20, 10);
    private readonly Button[] buttons;

    // define button sizes
    private const int ButtonWidth = 72;
    private const int ButtonHeight = 72;
    
    // grid
    private const int Width = 38;
    private const int Height = 38;
    private const int CellSizeLive = 16;
    private const int CellSizeDead = 12;
    private const int BorderWidth = 4;

    // text
    private const string Intro = 
@"Conway's Game of Life is a
cellular automation devised
by John Horton Conway in 1970.

This program allows you to
interact with the simulation
and explore the many different
patterns that can emerge.

Drag your mouse across the grid
on the left and see what happens.

Click the right arrow button
below to continue.";
    private const string IntroTwo = 
@"Each square in the grid is
known as a 'cell'. Every time the
grid changes, this is called a new
'generation' of cells. Each cell
is checked against a set of rules
to determine whether it lives to
the next generation:

- Any live cell with two or three
  neighbours survives
- Any dead cell with 3 live
  neighbours dies
- All other live cells die, and
  all other dead cells stay dead.";
    private const string Stills = 
@"These simple rules can create
some very complex structures.
Let's look at the simple ones first.
On the left you can see a grid full
of structures known as Still Lifes.
These are stable structures that do
not change between generations.

Click the play button below to
start the simulation.";
    private const string StillsPlaying =
@"The simulation is now running,
and as you can see, nothing is
changing. Try and apply Conway's
Rules to some cells on
the grid, you will find that
every live cell has either two or
three neighbours, therefore they
all survive to the next generation.

A cell's 'neighbours' are the eight
cells touching the cell, including
corners.";
    private const string Oscillators = 
@"The next structure we will
look at are called Oscillators.
These are structures that show
a repeating pattern when they
evolve between generations.
The number of generations taken
for an oscillator to repeat is
known as the period.

The grid shows a variety of
oscillators with periods 2
and 15.

Click play and see what happens.";
    private const string OscillatorsPlaying = 
@"It looks like a lot is going
on, but the entire grid will end
up in exactly the same state as
it started at after only 30
generations.

Click the reset button to reset
the grid to its original state
and see what's changed.

When a Life grid is randomised,
the final outcome is usually a
mostly empty grid with a handful
of Still Lifes and Oscillators.";
    private const string Spaceships =
@"Spaceships are structures
that can move themselves along
the grid. They also have a period
just like oscillators, however
they always end up at a different
position on the grid after each
period. The most common
spaceship is known as a Glider,
and is visible at the very top
of the grid. Click play to
watch what each of the
spaceships do.";
    private const string SpaceshipsPlaying =
@"Different spaceships can
move in different ways. Gliders
always move diagnonally, the
direction depending on the initial
orientation that it started in.

In this demonstration, after the
spaceships collide with the walls,
they leave still lives and an
oscillator. See if you can find
the period of the oscillator.";
    private const string Pentamino =
@"The shape shown here is known
as an R-pentamino. 'Penta-' is a 
prefix meaning 'five', and this
shape is made from five live cells.

This simple shape produces a
chaotic pattern that doesn't
stabilise for over a thousand
generations. It also spawns
6 gliders throughout its
lifespan, which were very likely
the first gliders to ever be
discovered.";
    private const string PentaminoPlaying =
@"You will notice that in this
simulation, the pattern stabilises
quicker than 1000 generations
and not as many gliders are seen.
This is due to the size limitations
of the grid.

Try and make one in the sandbox
to see it evolve more!";
    private const string Final =
@"And that's all you need
to know about Game of Life.

You can go back and look at the
guide again, or go straight into
the sandbox to create your own
pattens and see how they evolve.";
    private const float TextOpacity = 0.5f;
    private readonly Vector2 textOffset = new(640, 16);
    
    // determines if the user can draw on the grid
    private bool canDraw;

    public Tutorial(Life game) : base(game)
    {
        Life = game;
        
        grid = new Grid
        (
            Life, Width, Height,
            CellSizeLive, CellSizeDead,
            BorderWidth, Color.White
        );

        backButton = new Button
        (
            "back", 8, 640, ButtonWidth, ButtonHeight, "esc",
            Life, Life.Drawing.BackIcon
        );
        resetButton = new Button
        (
            "reset", 264, 640, ButtonWidth, ButtonHeight, "F5",
            Life, Life.Drawing.ResetIcon
        );
        prevButton = new Button
        (
            "prev", 520, 640,
            ButtonWidth, ButtonHeight, "left",
            Life, Life.Drawing.LeftArrowIcon
        );
        nextButton = new Button
        (
            "next", Drawing.ScreenWidth - ButtonWidth - 520, 640,
            ButtonWidth, ButtonHeight, "right",
            Life, Life.Drawing.RightArrowIcon
        );
        playButton = new Button
        (
            "play", Drawing.ScreenWidth - ButtonWidth - 264, 640,
            ButtonWidth, ButtonHeight, "space", Life,
            Life.Drawing.PlayIcon
        );
        pauseButton = new Button
        (
            "pause", Drawing.ScreenWidth - ButtonWidth - 264, 640,
            ButtonWidth, ButtonHeight, "space", Life,
            Life.Drawing.PauseIcon
        );
        sandboxButton = new Button
        (
            "Sandbox", 750, 400, 5*ButtonWidth, (int)(1.5f*ButtonHeight),
            "enter", Life, null
        );
        
        buttons = new[]
        {
            backButton,
            resetButton,
            nextButton,
            prevButton,
            playButton,
            pauseButton,
            sandboxButton
        };
    }

    public override void OnEntry()
    {
        tutorialPhase = 0;
        prevButton.IsDisabled = true;
        nextButton.IsDisabled = false;
        OnPhaseChange();
    }

    public override void Update(GameTime dt)
    {        
        // update grid
        grid.Update(dt);
        
        // return to title
        if (Life.Input.WasPressed(Keys.Escape) || Life.Input.ClickUp(backButton))
            Life.SceneManager.ChangeScene(SceneId.Title);
        
        // reset
        if (Life.Input.WasPressed(Keys.F5) || Life.Input.ClickUp(resetButton))
            OnPhaseChange();

        // go to sandbox
        if ((Life.Input.WasPressed(Keys.Enter) || Life.Input.ClickUp(sandboxButton)) && tutorialPhase == MaxPhase)
            Life.SceneManager.ChangeScene(SceneId.Sim);

        // next/prev page
        if ((Life.Input.WasPressed(Keys.Right) || Life.Input.ClickUp(nextButton)) && tutorialPhase < MaxPhase)
        {
            tutorialPhase++;
            prevButton.IsDisabled = false;
            if (tutorialPhase == MaxPhase)
                nextButton.IsDisabled = true;
            OnPhaseChange();
        }
        if ((Life.Input.WasPressed(Keys.Left) || Life.Input.ClickUp(prevButton)) && tutorialPhase > 0)
        {
            tutorialPhase--;
            nextButton.IsDisabled = false;
            if (tutorialPhase == 0)
                prevButton.IsDisabled = true;
            OnPhaseChange();
        }
        
        // play/pause
        if ((Life.Input.WasPressed(Keys.Space) || Life.Input.ClickUp(playButton))
            && !grid.IsPlaying && !playButton.IsDisabled)
            grid.Play();
        else if ((Life.Input.WasPressed(Keys.Space) || Life.Input.ClickUp(pauseButton))
                 && grid.IsPlaying && !pauseButton.IsDisabled)
            grid.Pause();

        // mouse painting
        if (Life.Input.WasPressed("lmb") && canDraw)
        {
            if (grid.IsValidCell(Life.Input.MousePos, offset))
            {
                var cellpos = grid.GetCellPosAt(Life.Input.MousePos, offset);
                int x = (int)cellpos.X;
                int y = (int)cellpos.Y;
                grid.SetCell(x, y, true);
            }
        }
        if (Life.Input.IsHeld("lmb") && canDraw)
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
            
        // erasing
        if (Life.Input.WasPressed("rmb") && canDraw)
        {
            if (grid.IsValidCell(Life.Input.MousePos, offset))
            {
                var cellpos = grid.GetCellPosAt(Life.Input.MousePos, offset);
                int x = (int)cellpos.X;
                int y = (int)cellpos.Y;
                grid.SetCell(x, y, false);
            }
        }
        if (Life.Input.IsHeld("rmb") && canDraw)
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
    }

    public override void Draw(GameTime dt)
    {
        grid.Draw(dt, offset);
            
        // draw buttons
        foreach (var x in buttons)
        {
            if (x.Name == "play" && grid.IsPlaying) continue;
            if (x.Name == "pause" && !grid.IsPlaying) continue;
            if (x.Name == "Sandbox")
            {
                if (tutorialPhase == MaxPhase)
                    Life.Drawing.DrawButtonWithText(x, sandboxButtonTextOffset, Vector2.Zero);
            }
            else Life.Drawing.DrawButtonWithIcon(x);
        }

        // text
        switch (tutorialPhase)
        {
            case 0:
                Life.Drawing.TypeMedium(Intro, textOffset, TextOpacity);
                break;
            case 1:
                Life.Drawing.TypeMedium(IntroTwo, textOffset, TextOpacity);
                break;
            case 2:
                if (!grid.IsPlaying) Life.Drawing.TypeMedium(Stills, textOffset, TextOpacity);
                else Life.Drawing.TypeMedium(StillsPlaying, textOffset, TextOpacity);
                break;
            case 3:
                if (!grid.IsPlaying) Life.Drawing.TypeMedium(Oscillators, textOffset, TextOpacity);
                else Life.Drawing.TypeMedium(OscillatorsPlaying, textOffset, TextOpacity);
                break;
            case 4:
                if (!grid.IsPlaying) Life.Drawing.TypeMedium(Spaceships, textOffset, TextOpacity);
                else Life.Drawing.TypeMedium(SpaceshipsPlaying, textOffset, TextOpacity);
                break;
            case 5:
                if (!grid.IsPlaying) Life.Drawing.TypeMedium(Pentamino, textOffset, TextOpacity);
                else Life.Drawing.TypeMedium(PentaminoPlaying, textOffset, TextOpacity);
                break;
            case 6:
                Life.Drawing.TypeMedium(Final, textOffset, TextOpacity);
                break;
        }
    }
    
    // the default attributes of the grid at each phase
    private void OnPhaseChange()
    {
        switch (tutorialPhase)
        {
            case 0:
                DisablePlayPause();
                canDraw = true;
                grid.Play();
                grid.SetSpeedIndex(3);
                grid.Load("empty");
                break;
            case 1:
                DisablePlayPause();
                canDraw = true;
                grid.Play();
                grid.SetSpeedIndex(1);
                grid.Load("empty");
                break;
            case 2:
                EnablePlayPause();
                canDraw = false;
                grid.Pause();
                grid.SetSpeedIndex(1);
                grid.Load("stills");
                break;
            case 3:
                EnablePlayPause();
                canDraw = false;
                grid.Pause();
                grid.SetSpeedIndex(3);
                grid.Load("oscillators");
                break;
            case 4:
                EnablePlayPause();
                canDraw = false;
                grid.Pause();
                grid.SetSpeedIndex(5);
                grid.Load("spaceships");
                break;
            case 5:
                EnablePlayPause();
                canDraw = false;
                grid.Pause();
                grid.SetSpeedIndex(5);
                grid.Load("pentamino");
                break;
            case 6:
                EnablePlayPause();
                canDraw = true;
                grid.Play();
                grid.SetSpeedIndex(6);
                grid.Load("border");
                break;
        }
    }

    private void DisablePlayPause()
    {
        playButton.IsDisabled = true;
        pauseButton.IsDisabled = true;
    }
    
    private void EnablePlayPause()
    {
        playButton.IsDisabled = false;
        pauseButton.IsDisabled = false;
    }
}

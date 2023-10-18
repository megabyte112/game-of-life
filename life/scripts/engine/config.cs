/*

config.cs

2023 Computer Science NEA
Aidan Norton

*/

using System.IO;
using Microsoft.Xna.Framework;

namespace Life.Engine;

public class Configuration
{
    public bool ShowKeyboardShortcuts { get; set; }
    public int BorderRadius { get; set; }
    public float FadeMultiplier { get; set; }
    public Color BackgroundColor { get; set; }
    public float ParallaxMultiplier { get; set; }
    public float ButtonMagneticMultiplier { get; set; }
    public float WindowAcceleration { get; set; }

    public Configuration()
    {
        Load();
    }

    public void Reset()
    {
        // default values
        ShowKeyboardShortcuts = false;
        BorderRadius = 10;
        FadeMultiplier = 4f;
        BackgroundColor = new Color(32, 32, 32, 255);
        ParallaxMultiplier = 1f;
        ButtonMagneticMultiplier = 1f;
        WindowAcceleration = 1.125f;
    }

    public void Save()
    {
        FileStream stream = File.Open("lifeconfig.bin", FileMode.Create);
        BinaryWriter bw = new BinaryWriter(stream);
        bw.Write(ShowKeyboardShortcuts);
        bw.Write(BorderRadius);
        bw.Write(FadeMultiplier);
        bw.Write(ParallaxMultiplier);
        bw.Write(ButtonMagneticMultiplier);
        bw.Write(WindowAcceleration);
        bw.Close();
        stream.Close();
    }

    private void Load()
    {
        try
        {
            FileStream stream = File.Open("lifeconfig.bin", FileMode.Open);
            BinaryReader br = new BinaryReader(stream);
            ShowKeyboardShortcuts = br.ReadBoolean();
            BorderRadius = br.ReadInt32();
            FadeMultiplier = br.ReadSingle();
            ParallaxMultiplier = br.ReadSingle();
            ButtonMagneticMultiplier = br.ReadSingle();
            WindowAcceleration = br.ReadSingle();
            br.Close();
            stream.Close();
            BackgroundColor = new Color(32, 32, 32, 255);
        }
        catch (FileNotFoundException)
        {
            Reset();
        }
    }
}
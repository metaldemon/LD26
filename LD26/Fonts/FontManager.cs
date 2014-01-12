
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace LD26
{
    public class FontManager
    {
        public static List<Text> TextList = new List<Text>();
        static List<int> FontTextures = new List<int>();

        public static void Initialize()
        {
            for (int a = 0; a < ContentManager.Textures.Count; a++)
            {
                if (ContentManager.Textures[a].File.Contains("Fonts"))
                    FontTextures.Add(ContentManager.Textures[a].GLTexture);
            }
            if (FontTextures.Count == 0)
            {
                Debug.WriteLine("[Warning] No Font found!");
                return;
            }
        }

        public static void AddText(Text text)
        {
            foreach (Text t in TextList)
                if (text.name == t.name)
                {
                    TextList.Remove(t);
                    break;
                }
            TextList.Add(text);
        }

        public static void RemoveText(string name)
        {
            bool finished = false;
            while (!finished)
            {
                int size = TextList.Count;
                foreach (Text T in TextList)
                    if (T.name == name)
                    {
                        TextList.Remove(T);
                        break;
                    }
                if (TextList.Count == size)
                    finished = true;
            }
        }

        public static void Draw()
        {
            //GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            int begin = (int)'A';
            TextList = TextList.Where(x => x.Time == float.MinValue || x.Time >= 0).ToList();

            TextList.ForEach(x =>
            {
                if (x.Time >= 0)
                    x.Time -= GameInfo.CoreGameInfo.DeltaTime;
            });
            foreach (Text T in TextList)
            {
                
                int plusy = 0;
                int minusx = 0;
                string text = T.text;
                float Xoffset = 0;
                for (int a = 0; a < text.Length; a++)
                {

                    char ch = text[a];
                    /*if (a > 0 && text[a - 1] == '\n' && text[a] != '\n')
                        continue;*/
                    if (text[a] == '\n')
                    {
                        plusy += (int)(43 * T.size);
                        minusx = (int)Xoffset;
                        continue;
                    }
                    bool islower = char.IsLower(ch);
                    bool ischar = char.IsLetter(ch);

                    int nr = (int)(ch) - begin;

                    if (ch == ' ')
                    {
                        if (ch == ' ')
                        {
                            Xoffset += (int)(24 * (T.size));
                            continue;
                        }

                    }

                    float addx = 0;
                    float addy = 0;
                    if (T.onscreen)
                    {
                        addx = (float)-(GameInfo.CoreGameInfo.CameraPosition.X);
                        addy = (float)-(GameInfo.CoreGameInfo.CameraPosition.Y);
                    }
                    float xscale = 24f * (T.size);
                    float yscale = 43f * (T.size);
                    float texposx = 2f / 1024f;
                    float texposy = 3f / 256f;
                    float letterx = 24f / 1024f;
                    float lettery = 43f / 256f;
                    if (char.IsNumber(ch))
                    {
                        texposy = 165f / 256f;
                        int chnr = int.Parse(ch.ToString());
                        texposx = (24f * (float)chnr) / 1024f;
                    }
                    if (char.IsSymbol(ch) || char.IsPunctuation(ch))
                    {
                        texposy = 165f / 256f;
                        if (ch == '.')
                            texposx = (24f * 10f) / 1024f;
                        if (ch == ',')
                            texposx = (24f * 11f) / 1024f;
                        if (ch == '?')
                            texposx = (24f * 12f) / 1024f;
                        if (ch == '!')
                            texposx = (24f * 13f) / 1024f;
                        if (ch == '(')
                            texposx = (24f * 14f) / 1024f;
                        if (ch == ')')
                            texposx = (24f * 15f) / 1024f;
                        if (ch == '"')
                            texposx = (24f * 16f) / 1024f;
                        if (ch == '\'')
                            texposx = (24f * 17f) / 1024f;
                        if (ch == ';')
                            texposx = (24f * 18f) / 1024f;
                        if (ch == ':')
                            texposx = (24f * 19f) / 1024f;
                        if (ch == '/')
                            texposx = (24f * 20f) / 1024f;
                        if (ch == '\\')
                            texposx = (24f * 21f) / 1024f;
                    }

                    if (char.IsLower(ch))
                    {
                        texposy = 86f / 256f;
                        nr -= 32;
                    }
                    if (char.IsLetter(ch))
                        texposx += (letterx * (float)nr);
                    try
                    {
                        ContentManager.CurrentBindedTexture = FontTextures[0];
                        GL.BindTexture(TextureTarget.Texture2D, FontTextures[0]);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    GL.Color4(T.Color);

                    GL.Enable(EnableCap.Texture2D);



                    GL.Begin(BeginMode.Quads);
                    {
                        GL.TexCoord2(texposx, texposy);
                        GL.Vertex2(((T.position.X + addx) + Xoffset) - (xscale / 2) - minusx, (T.position.Y + addy) + plusy);

                        GL.TexCoord2(texposx + letterx, texposy);
                        GL.Vertex2((((T.position.X + addx) + Xoffset) + xscale / 2) - minusx, (T.position.Y + addy) + plusy);

                        GL.TexCoord2(texposx + letterx, texposy + lettery);
                        GL.Vertex2((((T.position.X + addx) + Xoffset) + xscale / 2) - minusx, ((T.position.Y + yscale) + addy) + plusy);

                        GL.TexCoord2(texposx, texposy + lettery);
                        GL.Vertex2(((T.position.X + addx) + Xoffset) - (xscale / 2) - minusx, ((T.position.Y + yscale) + addy) + plusy);
                    }
                    GL.End();
                    GL.Disable(EnableCap.Texture2D);
                    Xoffset += (int)xscale;


                }


            }
            GL.Disable(EnableCap.Blend);
        }
    }
    public class Text
    {
        public string name;
        public string text;
        public OpenTK.Vector2 position;
        public float size;
        public bool onscreen = false;
        public Color4 Color = Color4.Black;
        public float Time = float.MinValue;

        public Text(string name, string text, OpenTK.Vector2 position, float size)
        {
            this.name = name;
            this.text = text;
            this.position = position;
            this.size = size;
        }
        public Text(string name, string text, OpenTK.Vector2 position, float size, bool onscreen)
        {
            this.name = name;
            this.text = text;
            this.position = position;
            this.size = size;
            this.onscreen = onscreen;
        }
        public Text(string name, string text, OpenTK.Vector2 position, float size, bool onscreen, Color4 color)
        {
            this.name = name;
            this.text = text;
            this.position = position;
            this.size = size;
            this.onscreen = onscreen;
            Color = color;
        }
        public Text(string name, string text, OpenTK.Vector2 position, float size, bool onscreen, Color4 color, float time)
        {
            this.name = name;
            this.text = text;
            this.position = position;
            this.size = size;
            this.onscreen = onscreen;
            this.Time = time;
            Color = color;
        }
    }
}

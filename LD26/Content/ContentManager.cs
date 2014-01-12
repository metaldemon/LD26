using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace LD26
{
    class ContentManager
    {
        public static List<Texture2D> Textures = new List<Texture2D>();

        public static int CurrentBindedTexture { get; set; }

        public static void LoadTextures()
        {
            foreach (Texture2D T in Textures)
                T.TextureInfo = Content.TexUtil.CreateTextureFromFile(T.File);

        }

        public static Texture2D GetTextureByName(string name)
        {
            return Textures.First(x => x.Name == name);
        }

        public static void BindTextureByName(string name)
        {
            Textures.First(x => x.Name == name).Bind();
        }

    }
    public class Texture2D
    {
        public string Name { get; set; }
        public string File { get; set; }
        public int GLTexture = 0;
        public int Height, Width;
        int[] TextureInfo_;
        public int[] TextureInfo
        {
            get { return TextureInfo_; }
            set
            {
                TextureInfo_ = value;
                GLTexture = TextureInfo_[0];
                Height = TextureInfo_[1];
                Width = TextureInfo_[2];
                Debug.WriteLine("Loaded Texture2D : \"" + Name + "\"");
            }
        }

        public Texture2D(string file, string name)
        {
            Name = name;
            File = file;
            Debug.WriteLine("Created Texture2D : \"" + name + "\"");

        }

        public void Bind()
        {
            if (ContentManager.CurrentBindedTexture != GLTexture)
            {
                GL.BindTexture(TextureTarget.Texture2D, GLTexture);
                ContentManager.CurrentBindedTexture = GLTexture;
            }
        }
    }
}

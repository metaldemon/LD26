using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace LD26
{
    class Sprite
    {
        string Name = "";
        public Vector2 Position;
        public double Rotation { get; set; }
        public float Scale { get; set; }


        public enum Flipmode
        {
            None = 0,
            Horizontally,
            Vertically,
            Both
        }
        public Flipmode FlipMode;

        public Vector2 GetRadius()
        {
            Texture2D buf = ContentManager.GetTextureByName(Name);
            return new Vector2(buf.Width, buf.Height);
        }

        public Sprite(string name)
        {
            Name = name;
            FlipMode = Flipmode.None;
            Position = new Vector2(400, 300);
        }

        public void Draw()
        {
            if (Position.X + GetRadius().X > -GameInfo.CoreGameInfo.CameraPosition.X &&
                Position.X < -GameInfo.CoreGameInfo.CameraPosition.X + MyGameWindow.Instance.Width &&
                Position.Y + GetRadius().Y > -GameInfo.CoreGameInfo.CameraPosition.Y &&
                Position.Y < -GameInfo.CoreGameInfo.CameraPosition.Y + MyGameWindow.Instance.Height)
            {
                Texture2D tex = ContentManager.GetTextureByName(Name);

                ContentManager.BindTextureByName(Name);
                GL.Enable(EnableCap.Texture2D);
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                GL.Color3(Color.White);

                GL.PushMatrix();
                Vector2 mid = new Vector2(tex.Width / 2, tex.Height / 2);
                Vector3 pos = new Vector3(Position.X, Position.Y, 0f);
                GL.Translate((pos + new Vector3(mid.X, mid.Y, 0)));
                GL.Rotate((int)Rotation, new Vector3(0, 0, 1));
                GL.Translate(-(pos + new Vector3(mid.X, mid.Y, 0)));
                

                GL.Begin(BeginMode.Quads);

                //GL.Color4(new Color4(255, 0, 0, 128));

                Vector2[] vectors = new Vector2[4];
                vectors[0] = Position;
                vectors[1] = Position + new Vector2(tex.Width, 0);
                vectors[2] = Position + new Vector2(tex.Width, tex.Height);
                vectors[3] = Position + new Vector2(0, tex.Height);




                if (FlipMode == Flipmode.None)
                {
                    for (int a = 0; a < vectors.Length; a++)
                    {
                        if (a == 0)
                            GL.TexCoord2(0, 0);
                        if (a == 1)
                            GL.TexCoord2(1, 0);
                        if (a == 2)
                            GL.TexCoord2(1, 1);
                        if (a == 3)
                            GL.TexCoord2(0, 1);
                        GL.Vertex3(vectors[a].X, vectors[a].Y, 0f);
                    }
                }
                if (FlipMode == Flipmode.Horizontally)
                {
                    for (int a = 0; a < vectors.Length; a++)
                    {
                        if (a == 0)
                            GL.TexCoord2(1, 0);
                        if (a == 1)
                            GL.TexCoord2(0, 0);
                        if (a == 2)
                            GL.TexCoord2(0, 1);
                        if (a == 3)
                            GL.TexCoord2(1, 1);
                        GL.Vertex3(vectors[a].X, vectors[a].Y, 0f);
                    }
                }
                if (FlipMode == Flipmode.Vertically)
                {
                    for (int a = 0; a < vectors.Length; a++)
                    {
                        if (a == 0)
                            GL.TexCoord2(0, 1);
                        if (a == 1)
                            GL.TexCoord2(1, 1);
                        if (a == 2)
                            GL.TexCoord2(1, 0);
                        if (a == 3)
                            GL.TexCoord2(0, 0);
                        GL.Vertex3(vectors[a].X, vectors[a].Y, 0f);
                    }
                }
                if (FlipMode == Flipmode.Both)
                {
                    for (int a = 0; a < vectors.Length; a++)
                    {
                        if (a == 0)
                            GL.TexCoord2(1, 1);
                        if (a == 1)
                            GL.TexCoord2(0, 1);
                        if (a == 2)
                            GL.TexCoord2(0, 0);
                        if (a == 3)
                            GL.TexCoord2(1, 0);
                        GL.Vertex3(vectors[a].X, vectors[a].Y, 0f);
                    }
                }
                GL.End();
                GL.Disable(EnableCap.Texture2D);
                GL.Disable(EnableCap.Blend);
                GL.Rotate(0, new Vector3(0, 0, 1));
                GL.PopMatrix();
            }

        }
    }
}

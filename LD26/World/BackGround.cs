using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace LD26.World
{
    class BackGround
    {

        static List<Vector2> stars = new List<Vector2>();

        public static void Initialize()
        {
            Random rand = new Random();
            for (int a = 0; a < 200; a++)
            {
                stars.Add(new Vector2(rand.Next(0, MyGameWindow.Instance.Width), rand.Next(0, MyGameWindow.Instance.Height)));
            }
        }


        public static void Draw()
        {
            for (int i = 0; i < stars.Count; ++i)
            {
                stars[i] = new Vector2(stars[i].X, stars[i].Y + (Player.MaxSpeed) * GameInfo.CoreGameInfo.DeltaTime * 0.1f);
            }

            for (int i = 0; i < stars.Count; ++i)
            {
                Vector2 v = stars[i];
                v.Y %= MyGameWindow.Instance.Height;
                stars[i] = v;
            }

                GL.Begin(BeginMode.Quads);
            {
                GL.Color3(Color.White);
                foreach (Vector2 V in stars)
                {
                    GL.Vertex2(V + new Vector2(-1, -(float)(Math.Pow(Player.MaxSpeed - 3f, 8f))));
                    GL.Vertex2(V + new Vector2(1, -(float)(Math.Pow(Player.MaxSpeed - 3f, 8f))));
                    GL.Vertex2(V + new Vector2(1, 1));
                    GL.Vertex2(V + new Vector2(-1, 1));
                }
            }
            GL.End();

        }
    }
}

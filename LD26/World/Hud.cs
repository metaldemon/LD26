using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace LD26.World
{
    class Hud
    {
        static Sprite Health;
        static Sprite Shield;

        public static void Initialize()
        {
            Health = new Sprite("Health");
            Health.Position = new Vector2(-Health.GetRadius().X, MyGameWindow.Instance.Height + Health.GetRadius().Y);
            Shield = new Sprite("Shield");
            Shield.Position = new Vector2(MyGameWindow.Instance.Width + Shield.GetRadius().X, MyGameWindow.Instance.Height + Shield.GetRadius().Y);
        }

        public static void Draw()
        {
            if (GameInfo.GameStateHandler.State == GameInfo.GameStateHandler.GameState.Intro)
            {
                if (BasicMath.GetLength(Health.Position, new Vector2(0, MyGameWindow.Instance.Height - Health.GetRadius().Y)) > 2)
                {
                    float rot = (float)MathHelper.DegreesToRadians(BasicMath.GetRotation(Health.Position, new Vector2(0, MyGameWindow.Instance.Height - Health.GetRadius().Y)));
                    Health.Position += Vector2.Transform(new Vector2(0, 1f * GameInfo.CoreGameInfo.DeltaTime * 0.1f), Quaternion.FromAxisAngle(new Vector3(0, 0, 1), rot - 90));
                }
                if (BasicMath.GetLength(Shield.Position, new Vector2(MyGameWindow.Instance.Width - Shield.GetRadius().X, MyGameWindow.Instance.Height - Shield.GetRadius().Y)) > 2)
                {
                    float rot = (float)MathHelper.DegreesToRadians(BasicMath.GetRotation(Shield.Position, new Vector2(MyGameWindow.Instance.Width - Shield.GetRadius().X, MyGameWindow.Instance.Height - Shield.GetRadius().Y)));
                    Shield.Position += Vector2.Transform(new Vector2(0, 1f * GameInfo.CoreGameInfo.DeltaTime * 0.1f), Quaternion.FromAxisAngle(new Vector3(0, 0, 1), rot - 90));
                }

            }
            if (GameInfo.GameStateHandler.State == GameInfo.GameStateHandler.GameState.PlayerDied)
            {
                if (BasicMath.GetLength(Health.Position, new Vector2(-Health.GetRadius().X, MyGameWindow.Instance.Height + Health.GetRadius().Y)) > 2)
                {
                    float rot = (float)MathHelper.DegreesToRadians(BasicMath.GetRotation(Health.Position, new Vector2(-Health.GetRadius().X, MyGameWindow.Instance.Height + Health.GetRadius().Y)));
                    Health.Position += Vector2.Transform(new Vector2(0, 1f * GameInfo.CoreGameInfo.DeltaTime * 0.1f), Quaternion.FromAxisAngle(new Vector3(0, 0, 1), rot - 90));
                }
                if (BasicMath.GetLength(Shield.Position, new Vector2(MyGameWindow.Instance.Width + Shield.GetRadius().X, MyGameWindow.Instance.Height + Shield.GetRadius().Y)) > 2)
                {
                    float rot = (float)MathHelper.DegreesToRadians(BasicMath.GetRotation(Shield.Position, new Vector2(MyGameWindow.Instance.Width + Shield.GetRadius().X, MyGameWindow.Instance.Height + Shield.GetRadius().Y)));
                    Shield.Position += Vector2.Transform(new Vector2(0, 1f * GameInfo.CoreGameInfo.DeltaTime * 0.1f), Quaternion.FromAxisAngle(new Vector3(0, 0, 1), rot - 90));
                }
            }
            float healthpercentage = Player.Health / Player.MaxHealth;
            float shieldpercentage = Player.Armor  / Player.MaxArmor;
            Health.Draw();
            Shield.Draw();
            GL.Begin(BeginMode.Quads);
            {
                GL.Color4(Color4.Red);
                GL.Vertex2(Health.Position + new Vector2(4, 4));
                GL.Color4(new Color4(1f - healthpercentage, healthpercentage, 0f, 1f));
                GL.Vertex2(Health.Position + new Vector2(4f + (155f * healthpercentage), 4f));
                GL.Color4(new Color4(1f - healthpercentage, healthpercentage, 0f, 1f));
                GL.Vertex2(Health.Position + new Vector2(4f + (155f * healthpercentage), 32f));
                GL.Color4(Color4.Red);
                GL.Vertex2(Health.Position + new Vector2(4, 32));


                GL.Color4(new Color4(1f - shieldpercentage, shieldpercentage, 0f, 1f));
                GL.Vertex2(Shield.Position + new Vector2(4f + (155f * (1f - shieldpercentage)), 4f));
                GL.Color4(Color4.Red);
                GL.Vertex2(Shield.Position + new Vector2(159, 4));
                GL.Color4(Color4.Red);
                GL.Vertex2(Shield.Position + new Vector2(159, 32));
                GL.Color4(new Color4(1f - shieldpercentage, shieldpercentage, 0f, 1f));
                GL.Vertex2(Shield.Position + new Vector2(4f + (155f * (1f - shieldpercentage)), 32f));

            }
            GL.End();

        }
    }
}

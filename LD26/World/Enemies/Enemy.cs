using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace LD26.World.Enemies
{
    class Enemy
    {
        public Vector2 Position;
        public double Rotation;
        public bool Remove { get; set; }
        public bool Shot { get; set; }
        float health;

        public float Health
        {
            get { return health; }
            set
            {
                health = value;
                if (Shape == EnemyShape.Pentagon || Shape == EnemyShape.TriangleBoss)
                    SoundManager.Hurt.Play();

                if (health <= 0)
                {
                    
                    Random rand = new Random();
                    if (Shape == EnemyShape.Pentagon || Shape == EnemyShape.TriangleBoss)
                    {
                        World.Enemies.EnemyHandler.SpawnedEnemies++;
                    }
                    if (Shape != EnemyShape.Pentagon && Shape != EnemyShape.TriangleBoss)
                    {
                        int percentage = rand.Next(0, 1000);
                        if (percentage < 250)
                        {
                            int powerup = rand.Next(0, 5);
                            switch (powerup)
                            {
                                case 1:
                                    World.Powerups.PowerupHandler.PowerupList.Add(new Powerups.Powerup("ShieldPowerup", Position));
                                    break;
                                case 2:
                                    World.Powerups.PowerupHandler.PowerupList.Add(new Powerups.Powerup("HealthPowerup", Position));
                                    break;
                                case 3:
                                    World.Powerups.PowerupHandler.PowerupList.Add(new Powerups.Powerup("FireSpeedPowerup", Position));
                                    break;
                                case 4:
                                    World.Powerups.PowerupHandler.PowerupList.Add(new Powerups.Powerup("MoveSpeedPowerup", Position));
                                    break;
                            }
                        }
                    }
                    Remove = true;
                }
            }
        }
        

        public EnemyShape Shape = EnemyShape.Triangle;

        public enum EnemyShape
        {
            Triangle = 0,
            Square,
            Circle,
            Pentagon,
            TriangleBoss
        }

        public void Initialize()
        {
            Random rand = new Random();
            Health = (float)(rand.Next(750,1500)) * (GameInfo.CoreGameInfo.Difficultylevel / 10);
        }


        public virtual void Draw(EnemyShape shape)
        {
            Vector3 origin = new Vector3(Position.X, Position.Y, 0);
            GL.PushMatrix();
            GL.Translate((origin + new Vector3(0, 0, 0)));
            GL.Rotate((int)Rotation, new Vector3(0, 0, 1));
            GL.Translate(-(origin + new Vector3(0, 0, 0)));
            if (shape == EnemyShape.Triangle)
            {
                GL.Begin(BeginMode.Triangles);
                {
                    GL.Color3(Color.Red);
                    GL.Vertex2(Position + new Vector2(-16, 0));
                    GL.Color3(Color.Green);
                    GL.Vertex2(Position + new Vector2(16, -16));
                    GL.Color3(Color.Blue);
                    GL.Vertex2(Position + new Vector2(16, 16));
                }
                GL.End();
                GL.PopMatrix();
            }
            if (shape == EnemyShape.Square)
            {
                GL.Begin(BeginMode.Quads);
                {
                    GL.Color3(Color.Red);
                    GL.Vertex2(Position + new Vector2(-16, -16));
                    GL.Color3(Color.Green);
                    GL.Vertex2(Position + new Vector2(16, -16));
                    GL.Color3(Color.Blue);
                    GL.Vertex2(Position + new Vector2(16, 16));
                    GL.Color3(Color.Yellow);
                    GL.Vertex2(Position + new Vector2(-16, 16));
                }
                GL.End();
                GL.PopMatrix();
            }
            if (shape == EnemyShape.Circle)
            {
                GL.Begin(BeginMode.TriangleFan);
                {
                    
                    for ( int a = 0 ;a < EnemyHandler.CircleVectors.Count; ++a)
                    {
                        float color = 255;
                        if (a % 16 == 0)
                            color = 0;
                        GL.Color4(new Color4((int)((float)(color) / EnemyHandler.CircleVectors.Count * 255), 0, 0, 255));
                        GL.Vertex2(Position + EnemyHandler.CircleVectors[a]);
                    }
                }
                GL.End();
                GL.PopMatrix();
            }
            if (shape == EnemyShape.Pentagon)
            {
                GL.Begin(BeginMode.TriangleFan);
                {
                    GL.Color4(Color4.Green);
                    GL.Vertex2(Position + new Vector2(0, -60));
                    GL.Color4(Color4.Gray);
                    GL.Vertex2(Position + new Vector2(60, -15));
                    GL.Color4(Color4.Red);
                    GL.Vertex2(Position + new Vector2(30, 60));
                    GL.Color4(Color4.Blue);
                    GL.Vertex2(Position + new Vector2(-30, 60));
                    GL.Color4(Color4.Gray);
                    GL.Vertex2(Position + new Vector2(-60, -15));
                }
                GL.End();
                GL.PopMatrix();
            }
            if (shape == EnemyShape.TriangleBoss)
            {
                GL.Begin(BeginMode.TriangleFan);
                {
                    for (int a = 0; a < EnemyHandler.TriangleBossVectors.Count; ++a)
                    {
                        float color = 255;
                        if (a % 16 == 0)
                            color = 0;
                        GL.Color4(new Color4((int)((float)(color) / EnemyHandler.TriangleBossVectors.Count * 255), 0, 0, 255));
                        GL.Vertex2(Position + EnemyHandler.TriangleBossVectors[a]);
                    }
                }
                GL.End();
                GL.PopMatrix();
            }
        }

    }
}

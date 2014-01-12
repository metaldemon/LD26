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
    class EnemyHandler
    {
        public static List<Vector2> CircleVectors = new List<Vector2>();
        public static List<Vector2> TriangleBossVectors = new List<Vector2>();
        public static List<Enemy> EnemyList = new List<Enemy>();


        public static int SpawnedEnemies { get; set; }

        public static void Initialize()
        {
            for (float a = 0; a < 360; a += 8)
            {
                if ( a % 16 == 0) 
                    CircleVectors.Add(Vector2.Transform(new Vector2(0, 16), Quaternion.FromAxisAngle(new Vector3(0, 0, 1), MathHelper.DegreesToRadians(a))));
                else
                    CircleVectors.Add(Vector2.Transform(new Vector2(0, 2), Quaternion.FromAxisAngle(new Vector3(0, 0, 1), MathHelper.DegreesToRadians(a))));
            }

            for (float a = 0; a < 360; a += 8)
            {
                if (a % 16 == 0)
                    TriangleBossVectors.Add(Vector2.Transform(new Vector2(0, 60), Quaternion.FromAxisAngle(new Vector3(0, 0, 1), MathHelper.DegreesToRadians(a))));
                else
                    TriangleBossVectors.Add(Vector2.Transform(new Vector2(0, 2), Quaternion.FromAxisAngle(new Vector3(0, 0, 1), MathHelper.DegreesToRadians(a))));
            }
        }

        public static bool ProjectileClipsWithEnemy(OpenTK.Vector2 position, OpenTK.Vector2 radius)
        {
            foreach (Enemy E in EnemyList)
                if (position.X + radius.X > E.Position.X && position.X < E.Position.X + 32 &&
                     position.Y + radius.Y > E.Position.Y && position.Y < E.Position.Y + 32)
                {
                    Random rand = new Random();
                    E.Health -= rand.Next(20, 50);
                    if (E.Health <= 0)
                        E.Shot = true;
                    SoundManager.Boom.Play();
                    return true;
                }
            return false;
        }

        public static bool CheckEnemyClips()
        {
            Vector2 position = Player.Ship.Position;
            Vector2 radius = Player.Ship.GetRadius();
            foreach (Enemy E in EnemyList)
                if (position.X + radius.X > E.Position.X && position.X < E.Position.X - 16 &&
                     position.Y + radius.Y > E.Position.Y && position.Y < E.Position.Y - 16)
                {
                    Random rand = new Random();
                    float numdamage = rand.Next(75, 150);
                    if ( Player.Armor > 0 )
                    {
                        Player.Armor -= (float)(numdamage / 2f);
                        numdamage -= numdamage / 2f;
                    }

                    Player.Health -= numdamage;
                    if (E.Shape == Enemy.EnemyShape.Pentagon || E.Shape == Enemy.EnemyShape.TriangleBoss)
                    {
                        Player.Health = -1f;
                        SoundManager.Boom.Play();
                    }
                    SoundManager.Hurt.Play();
                    if (E.Shape != Enemy.EnemyShape.Pentagon && E.Shape != Enemy.EnemyShape.TriangleBoss)
                        E.Remove = true;
                    return true;
                }

            return false;

        }

        public static void Draw()
        {
            CheckEnemyClips();
            XPManager.Update();
            EnemyList = EnemyList.Where(x => !x.Remove).ToList();
            EnemyList.ForEach(x => x.Draw(x.Shape));

        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;


namespace LD26.World.Projectiles
{
    class ProjectileHandler
    {
        public static List<Projectile> ProjectileList = new List<Projectile>();
        public static List<Projectile> EnemyProjectileList = new List<Projectile>();

        public static void Update()
        {
            ProjectileList = ProjectileList.Where(x => !x.Remove).ToList();
            ProjectileList.ForEach(x => x.Update());
            EnemyProjectileList = EnemyProjectileList.Where(x => !x.Remove).ToList();
            EnemyProjectileList.ForEach(x => x.Update());
            
        }

        public static bool ProjectileClipsWithPlayer(Vector2 position, Vector2 radius)
        {

            if (position.X + radius.X > Player.Ship.Position.X && position.X < Player.Ship.Position.X + Player.Ship.GetRadius().X &&
                     position.Y + radius.Y > Player.Ship.Position.Y && position.Y < Player.Ship.Position.Y + Player.Ship.GetRadius().Y)
            {
                Random rand = new Random();
                Player.Health -= rand.Next(20, 50);
                SoundManager.Hurt.Play();
                return true;
            }
            return false;
        }

        public static void Draw()
        {
            EnemyProjectileList.ForEach(x => x.Draw());
            ProjectileList.ForEach(x => x.Draw());
        }
        
    }

    public class Projectile
    {
        Sprite Sprite;
        
        public bool Remove { get; set; }
        float Speed = 0f;
        bool fromplayer = false;


        public Projectile(string spritename, Vector2 position, double angle, float speed, bool fromplayer)
        {
            this.fromplayer = fromplayer;
            Sprite = new Sprite(spritename);
            Sprite.Position = position;
            Sprite.Rotation = angle;
            Speed = speed;
        }

        public void Update()
        {
            Sprite.Position += Vector2.Transform(new Vector2(0, Speed * GameInfo.CoreGameInfo.DeltaTime * 0.1f), Quaternion.FromAxisAngle(new Vector3(0, 0, 1), MathHelper.DegreesToRadians((float)Sprite.Rotation - 90)));
            if (fromplayer && World.Enemies.EnemyHandler.ProjectileClipsWithEnemy(Sprite.Position, Sprite.GetRadius()))
            {
                Remove = !Remove;
            }
            if (!fromplayer && ProjectileHandler.ProjectileClipsWithPlayer(Sprite.Position, Sprite.GetRadius()))
            {
                Remove = !Remove;
            }
            else
            if (Sprite.Position.X < -MyGameWindow.Instance.Width || Sprite.Position.X > MyGameWindow.Instance.Width ||
                Sprite.Position.Y < -MyGameWindow.Instance.Height || Sprite.Position.Y > MyGameWindow.Instance.Height)
            {
                Remove = !Remove;
            }

        }

        public void Draw()
        {
            Sprite.Rotation += 90;
            Sprite.Draw();
            Sprite.Rotation -= 90;
        }

    }
}

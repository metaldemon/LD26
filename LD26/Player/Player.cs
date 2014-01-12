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
    class Player
    {
        public static Sprite Ship;
        static Sprite ShipFire;

        static FireState currentFireState = FireState.Idle;

        #region Properties

        public static float health = 250f;
        public static float Health
        {
            get { return health; }
            set
            {
                health = value;
                if (health <= 0)
                {
                    SoundManager.Boom.Play();
                    IsDead = true;
                }
            }
        }
        public static float MaxHealth = 250f;
        public static float IncreaseHealth = 0;
        public static float IncreaseArmor = 0;
        public static float Armor = 100f;
        public static float MaxArmor = 100f;
        static float fireDelay = 0;
        public static float MaxFireDelay = 400;
        static float xSpeed, ySpeed;
        public static float MaxSpeed = 3f;
        public static float IncreaseMaxSpeed { get; set; }

        public static bool IsDead { get; private set; }

        #endregion

        public enum FireState
        {
            Idle = 0,
            Firing,
            Reloading
        }

        public static void Initialize()
        {
            Ship = new Sprite("ship1");
            ShipFire = new Sprite("ship1fire");
            Ship.Position = new Vector2(MyGameWindow.Instance.Width * 2 - Ship.GetRadius().X * 2, MyGameWindow.Instance.Height - Ship.GetRadius().Y);
            ShipFire.Position = new Vector2(MyGameWindow.Instance.Width * 2 - Ship.GetRadius().X * 2, MyGameWindow.Instance.Height - Ship.GetRadius().Y);
            
        }




        public static void Update()
        {
            if (IsDead)
            {
                Ship.Rotation += 4f * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                Ship.Position += Vector2.Transform(new Vector2(0, 4f * GameInfo.CoreGameInfo.DeltaTime * 0.1f),
                    Quaternion.FromAxisAngle(new Vector3(0,0,1f), (float)MathHelper.DegreesToRadians(BasicMath.GetRotation(Ship.Position + Ship.GetRadius() / 2, new Vector2(MyGameWindow.Instance.Width / 2, -MyGameWindow.Instance.Height / 2)) + 90f)));
                if (GameInfo.GameStateHandler.State != GameInfo.GameStateHandler.GameState.PlayerDied)
                {
                    GameInfo.GameStateHandler.State = GameInfo.GameStateHandler.GameState.PlayerDied;
                    GameInfo.GameStateHandler.Stage = 0;
                    GameInfo.GameStateHandler.StageTimer = 1500;
                }
            }
            if (!IsDead)
            {
                if (IncreaseMaxSpeed > 0)
                {
                    MaxSpeed += 0.001f * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                    IncreaseMaxSpeed -= 0.001f * GameInfo.CoreGameInfo.DeltaTime * 0.1f;

                }
                if (IncreaseHealth > 0)
                {
                    IncreaseHealth -= 0.5f * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                    if (Health < MaxHealth)
                        Health += 0.5f * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                }
                if (IncreaseArmor > 0)
                {
                    IncreaseArmor -= 0.5f * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                    if (Armor < MaxArmor)
                        Armor += 0.5f * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                }
                Ship.Position.Y -= ySpeed * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                Ship.Position.X += xSpeed * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                if (Ship.Position.Y + Ship.GetRadius().Y > MyGameWindow.Instance.Height)
                    Ship.Position.Y += ySpeed * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                if (Ship.Position.Y < 0)
                    Ship.Position.Y -= ySpeed * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                if (Ship.Position.X + Ship.GetRadius().X > MyGameWindow.Instance.Width)
                    Ship.Position.X += xSpeed * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                if (Ship.Position.X < 0)
                    Ship.Position.X -= xSpeed * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                ySpeed *= 0.95f;
                xSpeed *= 0.95f;
                xSpeed = (float)Math.Round(xSpeed, 3);
                ySpeed = (float)Math.Round(ySpeed, 3);
                float rotspeed = 0.5f;
                if (GameInfo.GameStateHandler.State == GameInfo.GameStateHandler.GameState.Intro)
                    rotspeed = 2f;
                if (Ship.Rotation < 1 && 1 - Ship.Rotation <= 180 || Ship.Rotation - 1 >= 180)
                    Ship.Rotation += rotspeed * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                if (Ship.Rotation > 1 && Ship.Rotation - 1 <= 180 || 1 - Ship.Rotation >= 180)
                    Ship.Rotation -= rotspeed * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                if (InputHandler.CurrentKeyboardState.IsKeyDown(OpenTK.Input.Key.Up))
                {
                    if (ySpeed < MaxSpeed)
                        ySpeed += 0.2f * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                }
                if (InputHandler.CurrentKeyboardState.IsKeyDown(OpenTK.Input.Key.Down))
                {
                    if (ySpeed > -MaxSpeed)
                        ySpeed -= 0.2f * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                }
                if (InputHandler.CurrentKeyboardState.IsKeyDown(OpenTK.Input.Key.Left))
                {
                    if (xSpeed > -MaxSpeed)
                        xSpeed -= 0.2f * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                    if (Ship.Rotation > -20)
                        Ship.Rotation -= 1f * GameInfo.CoreGameInfo.DeltaTime * 0.1f;

                }
                if (InputHandler.CurrentKeyboardState.IsKeyDown(OpenTK.Input.Key.Right))
                {
                    if (xSpeed < MaxSpeed)
                        xSpeed += 0.2f * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                    if (Ship.Rotation < 20)
                        Ship.Rotation += 1f * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
                }
                if (InputHandler.CurrentKeyboardState.IsKeyDown(OpenTK.Input.Key.Space) &&
                    currentFireState == FireState.Idle)
                {
                    currentFireState = FireState.Firing;
                    fireDelay = MaxFireDelay;
                    World.Projectiles.ProjectileHandler.ProjectileList.Add(new World.Projectiles.Projectile("missle", Ship.Position + new Vector2(2, 20), -90 + Ship.Rotation, 6f, true));
                    World.Projectiles.ProjectileHandler.ProjectileList.Add(new World.Projectiles.Projectile("missle", Ship.Position + new Vector2(55, 20), -90 + Ship.Rotation, 6f, true));
                    SoundManager.Lazer.Play();
                    // BLAM BLAM BLAM
                }
                if (currentFireState == FireState.Firing)
                {
                    fireDelay -= GameInfo.CoreGameInfo.DeltaTime;
                    if (fireDelay <= MaxFireDelay - (MaxFireDelay / 3))
                        currentFireState = FireState.Reloading;
                }
                if (currentFireState == FireState.Reloading)
                {
                    fireDelay -= GameInfo.CoreGameInfo.DeltaTime;
                    if (fireDelay <= 0)
                    {
                        currentFireState = FireState.Idle;
                    }
                }



            }
                ShipFire.Rotation = Ship.Rotation;
                ShipFire.Position = Ship.Position;
            
        }




        public static void Draw()
        {
            if (currentFireState == FireState.Idle || currentFireState == FireState.Reloading)
                Ship.Draw();
            if (currentFireState == FireState.Firing)
                ShipFire.Draw();
            Vector2 shippos2d = Ship.Position;
            Vector3 shippos3d = new Vector3(shippos2d.X, shippos2d.Y, 0);
            GL.PushMatrix();
            GL.Translate((shippos3d + new Vector3(32, 32, 0)));
            GL.Rotate((int)Ship.Rotation, new Vector3(0, 0, 1));
            GL.Translate(-(shippos3d + new Vector3(32, 32, 0)));
            GL.Begin(BeginMode.Triangles);
            {
                

                GL.Color3(Color.Red);
                GL.Vertex2(shippos2d + new Vector2(21, 48));
                GL.Color3(Color.Red);
                GL.Vertex2(shippos2d + new Vector2(42, 48));
                GL.Color3(Color.Yellow);
                GL.Vertex2(shippos2d + new Vector2(31.5f, 80 * 1 + (8f * ((ySpeed + Math.Abs(xSpeed)) / 2))));

            }
            GL.End();
            GL.PopMatrix();

        }

    }
}

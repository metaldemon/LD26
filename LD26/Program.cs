using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.IO;


namespace LD26
{

    class MyGameWindow : GameWindow
    {
        public static GameWindow Instance { get; private set; }
        public MyGameWindow(int width, int height)
            : base(width, height, new GraphicsMode(ColorFormat.Empty, 24, 0, 4), "LD26 - Minimalistic! by Metaldemon")
        {
            ac = new OpenTK.Audio.AudioContext();

            VSync = VSyncMode.Off;
            Instance = this;
            GL.Viewport(0, 0, MyGameWindow.Instance.Width, MyGameWindow.Instance.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Ortho(0, MyGameWindow.Instance.Width, MyGameWindow.Instance.Height, 0, 10, -10);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            ContentManager.Textures.Add(new Texture2D("Content/World/Tile1.png", "tile1"));
            ContentManager.Textures.Add(new Texture2D("Content/Ships/ShipIdle.png", "ship1"));
            ContentManager.Textures.Add(new Texture2D("Content/Ships/ShipFire.png", "ship1fire"));
            ContentManager.Textures.Add(new Texture2D("Content/Fonts/Font.png", "font"));
            ContentManager.Textures.Add(new Texture2D("Content/Ships/Missle1.png", "missle"));
            ContentManager.Textures.Add(new Texture2D("Content/Dialog/DialogBox.png", "dialogbox"));
            ContentManager.Textures.Add(new Texture2D("Content/Hud/Health.png", "Health"));
            ContentManager.Textures.Add(new Texture2D("Content/Hud/Shield.png", "Shield"));
            ContentManager.Textures.Add(new Texture2D("Content/PowerUps/Shield.png", "ShieldPowerup"));
            ContentManager.Textures.Add(new Texture2D("Content/PowerUps/Health.png", "HealthPowerup"));
            ContentManager.Textures.Add(new Texture2D("Content/PowerUps/MoveSpeed.png", "MoveSpeedPowerup"));
            ContentManager.Textures.Add(new Texture2D("Content/PowerUps/FireSpeed.png", "FireSpeedPowerup"));
            ContentManager.Textures.Add(new Texture2D("Content/Projectiles/PentagonProjectile.png", "EnemyProjectile"));
            ContentManager.LoadTextures();
            FontManager.Initialize();
            World.BackGround.Initialize();
            World.Enemies.EnemyHandler.Initialize();
            World.Hud.Initialize();
            SoundManager.Initialize();

        }
        OpenTK.Audio.AudioContext ac;

        private static void Main()
        {
            StreamReader sr = new StreamReader("Preferences.txt");
            int width, height;
            width = Convert.ToInt32(sr.ReadLine());
            height = Convert.ToInt32(sr.ReadLine());
            sr.Close();
            sr.Dispose();
            using (GameWindow game = new MyGameWindow(width, height))
            {
                Player.Initialize();
                Debug.WriteLine("starting game");
                game.Run(60, 60);

            }
        }

        private static double totalTime = 0.0;
        private static int frames = 0;

        public override void Dispose()
        {
            SoundManager.Dispose();
            ac.Dispose();
            base.Dispose();
        }

        static float EnemySpawnTimer = 8000;

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            totalTime += e.Time;
            frames++;
            if (totalTime > 1.0)
            {
                int fps = frames;
                totalTime = 0.0;
                frames = 0;
                GameInfo.CoreGameInfo.Difficultylevel += .05f;
            }
            GameInfo.CoreGameInfo.DeltaTime = (float)e.Time * 1500f;
            InputHandler.Update();
            if (GameInfo.GameStateHandler.State == GameInfo.GameStateHandler.GameState.Gameplay || GameInfo.GameStateHandler.State == GameInfo.GameStateHandler.GameState.PlayerDied)
            {
                Player.Update();
                World.Projectiles.ProjectileHandler.Update();
                if (GameInfo.GameStateHandler.State == GameInfo.GameStateHandler.GameState.Gameplay)
                    EnemySpawnTimer -= (float)e.Time * 1500f;
                
                World.Powerups.PowerupHandler.Update();
            }
            GameInfo.GameStateHandler.Update();
            if (EnemySpawnTimer <= 0 && !Player.IsDead)
            {
                 if (World.Enemies.EnemyHandler.SpawnedEnemies == 125)
                {
                    World.Enemies.EnemyHandler.EnemyList.Add(new World.Enemies.TriangleBoss(new Vector2(MyGameWindow.Instance.Width / 2 - 15, -MyGameWindow.Instance.Height / 2)));
                    World.Enemies.EnemyHandler.SpawnedEnemies++; EnemySpawnTimer = 1000;
                }
                if (World.Enemies.EnemyHandler.SpawnedEnemies == 50)
                {
                    World.Enemies.EnemyHandler.EnemyList.Add(new World.Enemies.PentagonBoss(new Vector2(MyGameWindow.Instance.Width / 2 - 15, -MyGameWindow.Instance.Height / 2)));
                    World.Enemies.EnemyHandler.SpawnedEnemies++; EnemySpawnTimer = 1000;
                }
                if ((World.Enemies.EnemyHandler.SpawnedEnemies < 50 || World.Enemies.EnemyHandler.SpawnedEnemies > 51) &&
                    (World.Enemies.EnemyHandler.SpawnedEnemies < 125 || World.Enemies.EnemyHandler.SpawnedEnemies > 126))
                {
                    Random rand = new Random();
                    EnemySpawnTimer = rand.Next(2000 / (int)GameInfo.CoreGameInfo.Difficultylevel, 6000 / (int)GameInfo.CoreGameInfo.Difficultylevel);
                    int enemytype = rand.Next(0, 4);

                    switch (enemytype)
                    {
                        case 1:
                            World.Enemies.EnemyHandler.EnemyList.Add(new World.Enemies.Cube(new Vector2(rand.Next(32, Instance.Width - 32), 0)));
                            World.Enemies.EnemyHandler.SpawnedEnemies++;
                            break;
                        case 2:
                            World.Enemies.EnemyHandler.EnemyList.Add(new World.Enemies.Triangle(new Vector2(rand.Next(32, Instance.Width - 32), 0)));
                            World.Enemies.EnemyHandler.SpawnedEnemies++;
                            break;
                        case 3:
                            World.Enemies.EnemyHandler.EnemyList.Add(new World.Enemies.Circle(new Vector2(rand.Next(32, Instance.Width - 32), 0)));
                            World.Enemies.EnemyHandler.SpawnedEnemies++;
                            break;

                    }
                }

            }

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);

            GL.LoadIdentity();
            GL.Translate(GameInfo.CoreGameInfo.CameraPosition);
            GL.PushMatrix();
            GL.ClearColor(Color.Black);

            World.BackGround.Draw();
            World.Powerups.PowerupHandler.Draw();
            World.Projectiles.ProjectileHandler.Draw();
           
            Player.Draw();
            World.Enemies.EnemyHandler.Draw();
            GameInfo.GameStateHandler.Draw();
            World.Hud.Draw();
            FontManager.Draw();

            GL.PopMatrix();

            SwapBuffers();
        }

        

    }
    class Program
    {

    }

}

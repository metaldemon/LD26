using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace LD26.GameInfo
{
    class GameStateHandler
    {
        static Sprite DialogBox;
        public static GameState State = GameState.Menu;
        public static int Stage = 0;
        public static float StageTimer = 1000f;
        static bool UpdatePilotText = false;
        static bool movedowntext = false;
        static Text menutext;
        static Text pilottext;
        static Text PilotText
        {
            get { return pilottext; }
            set { pilottext = value; SoundManager.Notice.Play(); }
        }


        public enum GameState
        {
            Menu = 0,
            Intro,
            Gameplay,
            PlayerDied
        }

        static float IntroRotationVal = 8.7f;

        public static void Update()
        {
            if (State == GameState.Intro && Stage == 1 && BasicMath.GetLength(Player.Ship.Position,new Vector2(MyGameWindow.Instance.Width / 3, MyGameWindow.Instance.Height / 2)) > 20)
            {
                Player.Ship.Position += Vector2.Transform(new Vector2(0, 4f * GameInfo.CoreGameInfo.DeltaTime * 0.1f), 
                    Quaternion.FromAxisAngle(new Vector3(0, 0, 1),
                    MathHelper.DegreesToRadians((float)BasicMath.GetRotation(Player.Ship.Position, new Vector2(MyGameWindow.Instance.Width / 3, MyGameWindow.Instance.Height / 2)) - 90f)));
                if (BasicMath.GetLength(Player.Ship.Position, new Vector2(MyGameWindow.Instance.Width / 3, MyGameWindow.Instance.Height / 2)) < 200 && IntroRotationVal > 0)
                    IntroRotationVal -= 0.5f * CoreGameInfo.DeltaTime * 0.1f;
                if (IntroRotationVal < 0)
                    IntroRotationVal = 0;
                Player.Ship.Rotation += IntroRotationVal * CoreGameInfo.DeltaTime * 0.1f;
                while (Player.Ship.Rotation > 360)
                    Player.Ship.Rotation -= 360;
                while (Player.Ship.Rotation < 0)
                    Player.Ship.Rotation += 360;
            }
            if (PilotText != null)
            {
                if (UpdatePilotText)
                {
                    PilotText.position = DialogBox.Position + new Vector2(10, 10);
                    FontManager.AddText(PilotText);
                }
                else
                    FontManager.RemoveText(PilotText.name);
            }
            StageTimer -= CoreGameInfo.DeltaTime;
            if (State == GameState.Menu)
            {
                if (menutext != null)
                {
                    if (movedowntext)
                    {
                        if (menutext.position.Y < MyGameWindow.Instance.Height / 2 - 10)
                        {
                            movedowntext = false;
                        }
                        else
                            menutext.position.Y -= (0.8f * GameInfo.CoreGameInfo.DeltaTime * 0.1f);

                    }
                    if (!movedowntext)
                    {
                        if (menutext.position.Y > MyGameWindow.Instance.Height / 2 + 10)
                        {
                            movedowntext = true;
                        }
                        else
                            menutext.position.Y += (0.8f * GameInfo.CoreGameInfo.DeltaTime * 0.1f);

                    }
                    FontManager.AddText(menutext);
                }
            }

            if (State == GameState.Menu  && (Stage == 1 || Stage == 3 || Stage == 4) )
            {
                if (InputHandler.CurrentKeyboardState.IsKeyDown(OpenTK.Input.Key.Space))
                    StageTimer = 1;
            }
            if (StageTimer < 0)
            {
                Stage++;
                if (State == GameState.Menu)
                {
                    if (Stage == 1)
                    {
                        menutext = new Text("Menu", "Press (space) to insert coin!", new Vector2(MyGameWindow.Instance.Width / 2 - (324), MyGameWindow.Instance.Height / 2 - 21.5f), 1f, true, Color4.Blue);
                        StageTimer = int.MaxValue;
                    }
                    if (Stage == 2)
                    {
                        menutext = new Text("Menu", "Coin not accepted!", new Vector2(MyGameWindow.Instance.Width / 2 - (324), MyGameWindow.Instance.Height / 2 - 21.5f), 1f, true, Color4.Blue);
                        StageTimer = 2000;
                    }
                    if (Stage == 3)
                    {
                        menutext = new Text("Menu", "Press (space) to bash machine!", new Vector2(MyGameWindow.Instance.Width / 2 - (324), MyGameWindow.Instance.Height / 2 - 21.5f), 1f, true, Color4.Blue);
                        StageTimer = int.MaxValue;
                    }
                    if (Stage == 4)
                    {
                        menutext = new Text("Menu", "Use the Arrow Keys to move\nPress (Space) to Fire \n\nPress (Space) now to start \nthe game!", new Vector2(MyGameWindow.Instance.Width / 2 - (324), MyGameWindow.Instance.Height / 2 - 21.5f), 1f, true, Color4.White);
                        SoundManager.Bash.Play();
                        StageTimer = int.MaxValue;
                    }
                    if (Stage == 5)
                    {
                        menutext = new Text("Menu", "Starting Game!", new Vector2(MyGameWindow.Instance.Width / 2 - (324), MyGameWindow.Instance.Height / 2 - 21.5f), 1f, true, Color4.Blue);
                       
                        StageTimer = 1500f;
                    }
                    if (Stage == 6)
                    {
                        FontManager.RemoveText("Menu");
                        State = GameState.Intro;
                        StageTimer = 0;
                        Stage = 1;
                        Player.Ship.Rotation += 136f;
                    }
                }
                if (State == GameState.Intro)
                {
                    if (Stage == 1)
                    {
                        Player.Ship.Position = new Vector2(MyGameWindow.Instance.Width * 1.5f, MyGameWindow.Instance.Height / 2);
                        SoundManager.Woosh.Play();
                        SoundManager.Music.Play();
                        StageTimer = 5000f;
                    }
                    if (Stage == 2)
                    {
                        DialogBox = new Sprite("dialogbox");
                        UpdatePilotText = true;
                        PilotText = new Text("Pilot", "Huh?", DialogBox.Position + new Vector2(10, 10), 0.45f, true);
                        Player.Ship.Rotation = 0;
                        State = GameState.Gameplay;
                        StageTimer = 2000;
                    }
                    
                }
                if (State == GameState.Gameplay)
                {

                    if (Stage == 3)
                    {
                        PilotText = new Text("Pilot", "How did I end up here?", DialogBox.Position + new Vector2(10, 10), 0.45f, true);
                        StageTimer = 2000;
                    }
                    if (Stage == 4)
                    {
                        PilotText = new Text("Pilot", "Could it be that \nwormhole I just went \nthrough?", DialogBox.Position + new Vector2(10, 10), 0.4f, true);
                        StageTimer = 2000;
                    }
                    if (Stage == 5)
                    {
                        UpdatePilotText = false;
                        StageTimer = float.MaxValue;
                        Stage = 0;
                    }
                }
                if (State == GameState.PlayerDied)
                {
                    if (Stage == 1)
                    {
                        string text = "You Died! \n\n Game Over!";
                        FontManager.AddText(new Text("diedText", text, new Vector2(MyGameWindow.Instance.Width / 2 - ((text.Length / 2) * 20), MyGameWindow.Instance.Height / 12), 1f, true, Color4.White));
                        StageTimer = 1500;
                    }
                    if (Stage == 2)
                    {
                        string text = "Your final score was:";
                        FontManager.AddText(new Text("diedText2", text, new Vector2(MyGameWindow.Instance.Width / 2 - ((text.Length / 2) * 20), (MyGameWindow.Instance.Height / 12) + 160), 1f, true, Color4.White));
                        StageTimer = 1500;
                    }
                    if (Stage == 3)
                    {
                        string text = World.Enemies.XPManager.PlayerScore + " Potatoes!";
                        FontManager.AddText(new Text("diedText3", text, new Vector2(MyGameWindow.Instance.Width / 2 - ((text.Length / 2) * 20), (MyGameWindow.Instance.Height / 12) + 160 + 80), 1f, true, Color4.Red));
                        StageTimer = 1500;


                    }

                    if (Stage == 4)
                    {
                        int highscore = 0;
                        if (File.Exists("Highscore.POTATO"))
                        {
                            StreamReader sr = new StreamReader("Highscore.POTATO");
                            highscore = Convert.ToInt32(sr.ReadLine());
                            sr.Close();
                            sr.Dispose();
                        }
                        if (World.Enemies.XPManager.PlayerScore > highscore)
                        {
                            string text = "New Highscore!!!!";
                            FontManager.AddText(new Text("diedText4", text, new Vector2(MyGameWindow.Instance.Width / 2 - ((text.Length / 2) * 20), (MyGameWindow.Instance.Height / 12) + 160 + 80 + 80), 1f, true, Color4.White));
                            StreamWriter sw = new StreamWriter("Highscore.POTATO");
                            sw.WriteLine(World.Enemies.XPManager.PlayerScore.ToString());
                            sw.Close();
                            sw.Dispose();
                        }
                        else
                        {
                            string text = "You did not beat the highscore of: " + highscore + " potatoes";
                            FontManager.AddText(new Text("diedText4", text, new Vector2(MyGameWindow.Instance.Width / 2 - ((text.Length / 2) * 20), (MyGameWindow.Instance.Height / 12) + 160 + 80 + 80), 0.70f, true, Color4.White));
                        }
                        StageTimer = 1500;
                    }
                    if (Stage == 5)
                    {
                        string text = "Thank you for playing! :D\nMade by: Metaldemon";
                        FontManager.AddText(new Text("diedText5", text, new Vector2(MyGameWindow.Instance.Width / 2 - ((text.Length / 2) * 12), (MyGameWindow.Instance.Height / 12) + 160 + 80 + 80 + 80), 1f, true, Color4.Blue));
                        StageTimer = float.MaxValue;
                    }
                }

            }
        }
        public static void Draw()
        {
            if (DialogBox != null && UpdatePilotText)
            {
                DialogBox.Position = Player.Ship.Position + new Vector2((Player.Ship.GetRadius().X / 2) - DialogBox.GetRadius().X / 2, 100);
                if (DialogBox.Position.X < 0)
                    DialogBox.Position.X = 0;
                if (DialogBox.Position.X + DialogBox.GetRadius().X > MyGameWindow.Instance.Width)
                    DialogBox.Position.X = MyGameWindow.Instance.Width - DialogBox.GetRadius().X;
                if (DialogBox.Position.Y < 0)
                    DialogBox.Position.Y = 0;
                if (DialogBox.Position.Y + DialogBox.GetRadius().Y > MyGameWindow.Instance.Height)
                    DialogBox.Position.Y = MyGameWindow.Instance.Height - DialogBox.GetRadius().Y;
                DialogBox.Draw();
            }
        }

    }
}

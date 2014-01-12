using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace LD26.World.Powerups
{
    class Powerup
    {
        Sprite Sprite;

        public string SpriteName { get; set; }
        public bool Remove { get; set; }

        public Powerup(string sprite, Vector2 position)
        {
            SpriteName = sprite;
            Sprite = new Sprite(sprite);
            Sprite.Position = position;
        }

        public void Update()
        {
            float rotation = (float)MathHelper.DegreesToRadians(BasicMath.GetRotation(Sprite.Position + Sprite.GetRadius() / 2f , Player.Ship.Position + Player.Ship.GetRadius() /2f )) - 90f;
            float speed = 1f + (250f / (float)BasicMath.GetLength(Sprite.Position + Sprite.GetRadius() / 2f, Player.Ship.Position + Player.Ship.GetRadius() / 2f));
            if (BasicMath.GetLength(Sprite.Position + Sprite.GetRadius() / 2f, Player.Ship.Position + Player.Ship.GetRadius() / 2f) < 10)
            {
                SoundManager.Powerup.Play();
                if (SpriteName == "MoveSpeedPowerup" && Player.MaxSpeed < 4.2f)
                {
                    string line = "Picked up a coil thruster";
                    FontManager.AddText(new Text("Pickup", line, new Vector2(MyGameWindow.Instance.Width / 2 - (line.Length / 2f * 12f), MyGameWindow.Instance.Height - 30), 0.55f, true, Color4.White, 2000));
                    Player.IncreaseMaxSpeed = Player.MaxSpeed * .05f;
                }
                if (SpriteName == "ShieldPowerup")
                {
                    Player.IncreaseArmor += (Player.MaxArmor * 0.2f);
                    string line = "Picked up an armor shard";
                    FontManager.AddText(new Text("Pickup", line, new Vector2(MyGameWindow.Instance.Width / 2 - (line.Length / 2f * 12f), MyGameWindow.Instance.Height - 30), 0.55f, true, Color4.White, 2000));
                }
                if (SpriteName == "HealthPowerup")
                {

                    Player.IncreaseHealth += (Player.MaxHealth * 0.2f);
                    string line = "Picked up a repair pod";
                    FontManager.AddText(new Text("Pickup", line, new Vector2(MyGameWindow.Instance.Width / 2 - (line.Length / 2f * 12f), MyGameWindow.Instance.Height - 30), 0.55f, true, Color4.White, 2000));
                }
                if (SpriteName == "FireSpeedPowerup" && Player.MaxFireDelay > 75)
                {
                    string line = "Picked up a Lazer Overdrive Module";
                    FontManager.AddText(new Text("Pickup", line, new Vector2(MyGameWindow.Instance.Width / 2 - (line.Length / 2f * 12f), MyGameWindow.Instance.Height - 30), 0.55f, true, Color4.White, 2000));
                    Player.MaxFireDelay *= .95f;
                }
                Remove = true;
            }
            Sprite.Position += Vector2.Transform(new Vector2(0, speed), Quaternion.FromAxisAngle(new Vector3(0, 0, 1f), rotation)); 
        }

        public void Draw()
        {
            Sprite.Draw();
        }
    }
}

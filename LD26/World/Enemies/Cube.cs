using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LD26.World.Enemies
{
    class Cube : Enemy
    {
        float speed = 0;
        double rotationspeed;
        double movementRotation = 90;
        public Cube(OpenTK.Vector2 position)
        {
            Random rand = new Random();
            speed = 13.5f * (GameInfo.CoreGameInfo.Difficultylevel * 0.05f);
            rotationspeed = (float)(rand.NextDouble() - .5f) * 12f * GameInfo.CoreGameInfo.Difficultylevel;
            base.Position = position;
            base.Shape = EnemyShape.Square;

        }
        public override void Draw(EnemyShape shape)
        {
            base.Position += Vector2.Transform(new Vector2(0, speed * GameInfo.CoreGameInfo.DeltaTime * 0.1f), Quaternion.FromAxisAngle(new Vector3(0, 0, 1), (float)MathHelper.DegreesToRadians(movementRotation - 90)));

            double RequiredRotation = BasicMath.GetRotation(base.Position + new Vector2(16, 16), Player.Ship.Position + Player.Ship.GetRadius());
            movementRotation = RequiredRotation;
            /*
            if (movementRotation < RequiredRotation && RequiredRotation - movementRotation <= 180 || movementRotation - RequiredRotation >= 180)
                movementRotation += 2 * GameInfo.CoreGameInfo.DeltaTime * 0.1f;
            if (movementRotation > RequiredRotation && movementRotation - RequiredRotation <= 180 || RequiredRotation - movementRotation >= 180)
                movementRotation -= 2 * GameInfo.CoreGameInfo.DeltaTime * 0.1f;*/

            base.Rotation += rotationspeed;
            if (base.Position.Y > MyGameWindow.Instance.Height)
                base.Remove = true;
            base.Draw(shape);
        }

    }
}

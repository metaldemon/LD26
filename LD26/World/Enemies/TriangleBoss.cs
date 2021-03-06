﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;


namespace LD26.World.Enemies
{
    class TriangleBoss : Enemy
    {
        float speed = 0;
        double rotationspeed;
        double movementRotation = 90;

        float ProjectTileSpawnDelay = 3000;

        public TriangleBoss(OpenTK.Vector2 position)
        {
            Random rand = new Random();
            speed = 1.5f;

            rotationspeed = 5f;
            base.Position = position;
            base.Shape = EnemyShape.TriangleBoss;
            base.Health = 500;

        }
        public override void Draw(EnemyShape shape)
        {
            Random rand = new Random();

            ProjectTileSpawnDelay -= GameInfo.CoreGameInfo.DeltaTime;
            if (ProjectTileSpawnDelay < 0 && !Player.IsDead)
            {
                ProjectTileSpawnDelay = (float)rand.Next(300, 1500);
                World.Projectiles.ProjectileHandler.EnemyProjectileList.Add(new Projectiles.Projectile("EnemyProjectile", Position, BasicMath.GetRotation(Position, Player.Ship.Position + Player.Ship.GetRadius() / 2), 6f, false));
            }
            base.Position += Vector2.Transform(new Vector2(0, speed * GameInfo.CoreGameInfo.DeltaTime * 0.1f), Quaternion.FromAxisAngle(new Vector3(0, 0, 1), (float)MathHelper.DegreesToRadians(movementRotation - 90)));

            double RequiredRotation = BasicMath.GetRotation(base.Position + new Vector2(16, 16), Player.Ship.Position + Player.Ship.GetRadius());
            movementRotation = RequiredRotation;

            base.Rotation += rotationspeed;
            base.Draw(shape);
        }

    }
}

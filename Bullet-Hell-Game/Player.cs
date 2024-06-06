using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullet_Hell_Game
{
    public class Player : Character, ICollidable
    {
        private int lives = 3;

        public CollisionArea.CollisionType CollisionType { get; } = CollisionArea.CollisionType.Player;

        public RotatableShape BoundingBox { get; private set; }
        public bool IsCollidable { get; set; }

        public Player(AnimatedSprite sprite, Vector2 position, float speed) : base(sprite, position, Vector2.Zero, speed) 
        {
            BoundingBox = new RotatableShape(new Rectangle((int) Math.Round(position.X), (int) Math.Round(position.Y), (int) Math.Round(sprite.Texture.Width * sprite.Scale*1.5), (int) Math.Round(sprite.Texture.Height * sprite.Scale*1.5)), 0);
        }

        public override void Update(float deltaSeconds)
        {
            base.Update(deltaSeconds);
            BoundingBox.X = (int) Math.Round(Position.X);
            BoundingBox.Y = (int) Math.Round(Position.Y);
        }

        public void OnCollision(CollisionArea.CollisionType collisionType, Vector2 minimumTranslationVector)
        {
            Debug.WriteLine("Player Collision: " + collisionType.ToString());
            Position = Vector2.Add(Position, minimumTranslationVector);
        }

    }
}

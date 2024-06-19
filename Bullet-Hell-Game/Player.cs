using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Bullet_Hell_Game
{
    public class Player : Character, ICollidable
    {
        private int lives = 3;

        public CollisionArea.CollisionType CollisionType { get; } = CollisionArea.CollisionType.Player;

        public RotatableShape BoundingBox { get; private set; }
        public bool IsCollidable { get; set; } = true;
        public bool CollisionChecked { get; set; } = false;

        public Player(AnimatedSprite sprite, Vector2 position, float speed) : base(sprite, position, Vector2.Zero, speed) 
        {
            float width = sprite.Texture.Width * sprite.Scale;
            float height = sprite.Texture.Height * sprite.Scale;
            BoundingBox = new RotatableShape(position.X + (width/2), position.Y + (height/2), width/2/3);
        }

        public override void FixedUpdate()
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                MoveDirection = new Vector2(MoveDirection.X, -1);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                MoveDirection = new Vector2(MoveDirection.X, 1);
            }
            else
            {
                MoveDirection = new Vector2(MoveDirection.X, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                MoveDirection = new Vector2(1, MoveDirection.Y);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                MoveDirection = new Vector2(-1, MoveDirection.Y);
            }
            else
            {
                MoveDirection = new Vector2(0, MoveDirection.Y);
            }

            if (!MoveDirection.Equals(Vector2.Zero))
            {
                MoveDirection = Vector2.Normalize(MoveDirection);
            }
            base.FixedUpdate();
            BoundingBox.Move(new Vector2(BoundingBox.X, BoundingBox.Y) + MoveDirection*Speed);
        }

        public void OnCollision(CollisionArea.CollisionType collisionType, Vector2 minimumTranslationVector)
        {
            switch(collisionType)
            {
                case CollisionArea.CollisionType.Obstacle:
                    {
                        Position = Vector2.Add(Position, minimumTranslationVector/2);
                        BoundingBox.Move(new Vector2(BoundingBox.X, BoundingBox.Y) + minimumTranslationVector / 2);
                        MoveDirection = Vector2.Zero;
                        break;
                    }
                case CollisionArea.CollisionType.EnemyProjectile:
                    {
                        OnKill(EventArgs.Empty);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            
        }

    }
}

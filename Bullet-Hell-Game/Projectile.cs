using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Bullet_Hell_Game
{
    public class Projectile : ICollidable, ILerpMovable, IFixedUpdatable
    {
        private AnimatedSprite sprite;

        public Vector2 Position { get; set; }
        public Vector2 PreviousPosition { get; private set; }
        public Vector2 LerpPosition { get; private set; }
        public Vector2 MoveDirection { get; private set; }
        public float Speed { get; set; }

        private Func<Vector2> DirectionPattern;
        private Func<float> SpeedPattern;

        int milli = 0;
        public bool IsCollidable {  get; set; }
        public CollisionArea.CollisionType CollisionType { get; private set; }
        public RotatableShape BoundingBox { get; private set; }
        public bool CollisionChecked { get; set; } = false;
        public event EventHandler? Kill;

        public Projectile(RotatableShape boundingBox, Vector2 position, AnimatedSprite sprite, CollisionArea.CollisionType collisionType, Func<Vector2> directionPattern, Func<float> speedPattern)
        {
            BoundingBox = boundingBox;
            Position = position;
            this.sprite = sprite;
            CollisionType = collisionType;
            DirectionPattern = directionPattern;
            SpeedPattern = speedPattern;
        }

        public void FixedUpdate()
        {
            sprite.Update(milli, 10);
            milli += 33;
            PreviousPosition = Position;
            MoveDirection = Vector2.Normalize(DirectionPattern());
            Speed = SpeedPattern();
            Position += MoveDirection * Speed;
            BoundingBox.Move(new Vector2(BoundingBox.X, BoundingBox.Y) + MoveDirection * Speed);

        }

        public void LerpDraw(SpriteBatch spriteBatch, float ALPHA)
        {
            LerpPosition = Vector2.Lerp(PreviousPosition, Position, ALPHA);
            sprite.Draw(spriteBatch, LerpPosition, false);
        }

        public virtual void OnCollision(CollisionArea.CollisionType collisionType, Vector2 minimumTranslationVector)
        {
            switch (collisionType)
            {
                case CollisionArea.CollisionType.Player:
                    OnKill(EventArgs.Empty);
                    break;
                default:
                    break;
            }
        }

        public void OnKill(EventArgs e)
        {
            Kill?.Invoke(this, e);
        }
    }
}

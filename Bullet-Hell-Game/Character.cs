using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Bullet_Hell_Game
{
    public class Character : ILerpMovable
    {
        private AnimatedSprite sprite;

        public Vector2 MoveVelocity {  get; set; }
        public float Speed { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 PreviousPosition { get; private set; } = Vector2.Zero;
        public Vector2 LerpPosition { get; private set; } = Vector2.Zero;

        public event EventHandler? Kill;

        public Character(AnimatedSprite sprite) : this(sprite, Vector2.Zero, Vector2.Zero, 1) { }

        public Character(AnimatedSprite sprite, Vector2 position, Vector2 velocity, float speed)
        {
            MoveVelocity = velocity;
            Speed = speed;
            Position = position;
            this.sprite = sprite;
        }

        public void Move(float deltaSeconds)
        {
            Position = Vector2.Add(Position, Vector2.Multiply(MoveVelocity, deltaSeconds * Speed));
        }

        public virtual void Update(float deltaSeconds)
        {
            PreviousPosition = Position;
            Move(deltaSeconds);
        }

        public void LerpDraw(SpriteBatch spriteBatch, float ALPHA)
        {
            LerpPosition = Vector2.Lerp(PreviousPosition, Position, ALPHA);
            sprite.Draw(spriteBatch, LerpPosition, false);
        }

        public virtual void OnKill(EventArgs e)
        {
            Kill?.Invoke(this, e);
        }
    }
}

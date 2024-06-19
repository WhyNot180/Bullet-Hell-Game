using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Bullet_Hell_Game
{
    /// <summary>
    /// Simple base for character entities
    /// </summary>
    public class Character : ILerpMovable, IFixedUpdatable
    {
        private AnimatedSprite sprite;

        public Vector2 MoveDirection {  get; set; }
        public float Speed { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 PreviousPosition { get; private set; } = Vector2.Zero;
        public Vector2 LerpPosition { get; private set; } = Vector2.Zero;

        public event EventHandler? Kill;

        /// <summary>
        /// Initializes a character at position (0,0) with zero velocity
        /// </summary>
        /// <param name="sprite"></param>
        public Character(AnimatedSprite sprite) : this(sprite, Vector2.Zero, Vector2.Zero, 1) { }

        /// <summary>
        /// Initializes a character with a specific position and velocity
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="position"></param>
        /// <param name="moveDirection"></param>
        /// <param name="speed"></param>
        public Character(AnimatedSprite sprite, Vector2 position, Vector2 moveDirection, float speed)
        {
            MoveDirection = moveDirection;
            Speed = speed;
            Position = position;
            this.sprite = sprite;
        }

        public void Move()
        {
            Position = Vector2.Add(Position, Vector2.Multiply(MoveDirection, Speed));
        }

        public virtual void FixedUpdate()
        {
            PreviousPosition = Position;
            Move();
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

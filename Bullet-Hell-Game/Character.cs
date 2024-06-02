using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bullet_Hell_Game
{
    public class Character : ILerpMovable
    {
        private AnimatedSprite sprite;

        public Vector2 MoveVelocity {  get; set; }

        public Vector2 Position { get; set; }
        public Vector2 PreviousPosition { get; private set; } = Vector2.Zero;
        public Vector2 LerpPosition { get; private set; } = Vector2.Zero;

        public Character(AnimatedSprite sprite) : this(Vector2.Zero, Vector2.Zero, sprite) { }

        public Character(Vector2 position, Vector2 velocity, AnimatedSprite sprite)
        {
            MoveVelocity = velocity;
            Position = position;
            this.sprite = sprite;
        }

        public void Move(float deltaSeconds)
        {
            Position = Vector2.Add(Position, Vector2.Multiply(MoveVelocity, deltaSeconds));
        }

        public void Update(float deltaSeconds)
        {
            PreviousPosition = Position;
            Move(deltaSeconds);
        }

        public void LerpDraw(SpriteBatch spriteBatch, float ALPHA)
        {
            LerpPosition = Vector2.Lerp(PreviousPosition, Position, ALPHA);
            sprite.Draw(spriteBatch, LerpPosition, false);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bullet_Hell_Game
{
    public class Stage : ILerpMovable
    {
        public Vector2 Position {  get; set; }
        public Vector2 PreviousPosition { get; private set; } = Vector2.Zero;
        public Vector2 LerpPosition { get; private set; } = Vector2.Zero;
        public Vector2 MoveVelocity { get; set; } = Vector2.Zero;
        public float Speed { get; set; }

        private Texture2D background;

        public Stage(Texture2D backgroundSprite)
        {
            background = backgroundSprite;
        }

        public void Move(float deltaSeconds)
        {
            Position = Vector2.Add(Position, Vector2.Multiply(MoveVelocity, deltaSeconds * Speed));
        }

        public void Update(float deltaTime)
        {
            PreviousPosition = Position;
            Move(deltaTime);
        }

        public void LerpDraw(SpriteBatch spriteBatch, float ALPHA)
        {
            LerpPosition = Vector2.Lerp(PreviousPosition, Position, ALPHA);
            spriteBatch.Draw(background, LerpPosition, Color.Transparent);
        }

    }
}

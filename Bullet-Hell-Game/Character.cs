using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bullet_Hell_Game
{
    public class Character
    {
        private AnimatedSprite sprite;

        public Vector2 MoveVelocity {  get; set; }

        public Vector2 Position { get; set; }

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

        public void Update(GameTime gameTime)
        {
            Move((float) gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Draw(SpriteBatch spriteBatch, bool horizontalFlip)
        {
            sprite.Draw(spriteBatch, Position, horizontalFlip);
        }
    }
}

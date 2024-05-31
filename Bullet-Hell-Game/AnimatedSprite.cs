using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bullet_Hell_Game
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        public float Scale { get; set; }

        /// <summary>
        /// An animated sprite with a sprite sheet.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="scale"></param>
        public AnimatedSprite(Texture2D texture, int rows, int columns, float scale)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            Scale = scale;
        }
        public void Reset()
        {
            currentFrame = 0;
        }

        /// <summary>
        /// Updates the currently displayed frame at a fixed rate.
        /// </summary>
        /// <param name="currentMilli">The current time in milliseconds.</param>
        /// <param name="fps">The desired frame rate in frames per second.</param>
        /// <returns>Returns true when the animation has reached the max frames.</returns>
        public bool Update(int currentMilli, double fps)
        {
            bool isFinished = false;
            if (currentMilli % (1 / fps * 1000) == 0)
                currentFrame++;
            if (currentFrame == totalFrames)
            {
                currentFrame = 0;
                isFinished = true;
            }
            return isFinished;
        }

        /// <summary>
        /// Draws the current frame on the window.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch in which it should be drawn.</param>
        /// <param name="location">The location on the window.</param>
        /// <param name="horizontalFlip">Whether the sprite should be flipped.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 location, bool horizontalFlip)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = currentFrame / Columns;
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);

            spriteBatch.Draw(Texture, location, sourceRectangle, Color.White, 0, location, Scale, horizontalFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
    }
}

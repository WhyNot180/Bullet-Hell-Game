using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;

namespace Bullet_Hell_Game
{
    public class BulletHell : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Fix updates to 30 fps
        private float fixedUpdateDelta = (int)(1000 / (float)30);

        private float previousTime = 0;
        private float timeAccumulator = 0.0f;
        private float maxFrameTime = 250;

        // this value stores how far we are in the current frame. For example, when the 
        // value of ALPHA is 0.5, it means we are halfway between the last frame and the 
        // next upcoming frame.
        private float ALPHA = 0;

        public BulletHell()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (previousTime == 0)
            {
                previousTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
            }

            float currentTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
            float frameTime = currentTime - previousTime;
            if (frameTime > maxFrameTime)
            {
                frameTime = maxFrameTime;
            }

            previousTime = currentTime;

            timeAccumulator += frameTime;

            while (timeAccumulator >= fixedUpdateDelta)
            {
                FixedUpdate();
                timeAccumulator -= fixedUpdateDelta;
            }

            // this value stores how far we are in the current frame. For example, when the 
            // value of ALPHA is 0.5, it means we are halfway between the last frame and the 
            // next upcoming frame.
            ALPHA = (timeAccumulator / fixedUpdateDelta);

            // Put updates that don't depend on a fixed framerate below

            base.Update(gameTime);
        }

        private void FixedUpdate()
        {

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

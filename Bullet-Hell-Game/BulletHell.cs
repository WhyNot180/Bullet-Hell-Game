using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

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

        private Player player;

        private CollisionArea collisionArea;

        public BulletHell()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.ApplyChanges();

            collisionArea = new CollisionArea(new Rectangle(0, 0, 640, 480));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            player = new Player(new AnimatedSprite(Content.Load<Texture2D>("Sprites/Player"), 1, 1, 3), new Vector2(110,110), 13);
            
            collisionArea.colliders.Add(player);
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
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                player.MoveVelocity = new Vector2(player.MoveVelocity.X, -1);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                player.MoveVelocity = new Vector2(player.MoveVelocity.X, 1);
            }
            else
            {
                player.MoveVelocity = new Vector2(player.MoveVelocity.X, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                player.MoveVelocity = new Vector2(1, player.MoveVelocity.Y);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                player.MoveVelocity = new Vector2(-1, player.MoveVelocity.Y);
            }
            else
            {
                player.MoveVelocity = new Vector2(0, player.MoveVelocity.Y);
            }

            if (!player.MoveVelocity.Equals(Vector2.Zero))
            {
                player.MoveVelocity = Vector2.Normalize(player.MoveVelocity);
            }

            player.Update(1);
            collisionArea.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            player.LerpDraw(_spriteBatch, ALPHA);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

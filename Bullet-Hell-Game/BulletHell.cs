using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Bullet_Hell_Game
{
    public class BulletHell : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private EntityManager<ILerpMovable> lerpEntityManager;
        private EntityManager<ICollidable> collisionEntityManager;
        private EntityManager<IFixedUpdatable> fixedUpdateablesManager;

        // Fix updates to 30 fps
        private float fixedUpdateDelta = (int)(1000 / (float)30);

        private float previousTime = 0;
        private float timeAccumulator = 0.0f;
        private float maxFrameTime = 250;
        
        /// <summary>
        /// Stores how far into the frame a lerp movable is (i.e. 0.5 is halfway into the frame)
        /// </summary>
        private float ALPHA = 0;

        public ObservableCollection<ILerpMovable> lerpMovables = new ObservableCollection<ILerpMovable>();
        public ObservableCollection<IFixedUpdatable> fixedUpdateables = new ObservableCollection<IFixedUpdatable>();

        private CollisionArea collisionArea;

        public BulletHell()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Set screen size
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 780;
            _graphics.ApplyChanges();

            collisionArea = new CollisionArea(new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Player player = new Player(new AnimatedSprite(Content.Load<Texture2D>("Sprites/Player"), 1, 1, 3), new Vector2(320,700), 13);
            
            ObservableCollection<StageElement> stageElements = new ObservableCollection<StageElement>() { new(Content.Load<Texture2D>("Sprites/Brass Pipe"), true, new RotatableShape(new Rectangle(605, 200, 80, 40), 0), new Vector2(545, 185), CollisionArea.CollisionType.Obstacle) };

            Stage stage = new Stage(stageElements);
            
            List<Projectile> projectiles = new List<Projectile>();
            
            // Movement pattern for each projectile
            for (int j = 0; j < 4; j++)
            {
                projectiles.Add(new Projectile(new RotatableShape(320 - j*70, 200, 20), new Vector2(300 - j*70, 180), new AnimatedSprite(Content.Load<Texture2D>("Sprites/Gear Projectile"), 1, 2, 2.5f), CollisionArea.CollisionType.EnemyProjectile,
                    i =>
                    {
                        return new Vector2(MathF.Sin(i), -MathF.Cos(i)+1);
                    },
                    i =>
                    {
                        return 5;
                    },
                    new Iterator(0, 0.1f)));
            }

            // Add each relevant object to entity managers
            lerpMovables.Add(player);
            lerpMovables.Add(stage);
            projectiles.ForEach(x => lerpMovables.Add(x));
            stageElements.AsEnumerable().ToList().ForEach(x => lerpMovables.Add(x));

            lerpEntityManager = new EntityManager<ILerpMovable>(lerpMovables);

            collisionArea.colliders.Add(player);
            projectiles.ForEach(x => collisionArea.colliders.Add(x));
            stageElements.AsEnumerable().ToList().ForEach(x => collisionArea.colliders.Add(x));

            collisionEntityManager = new EntityManager<ICollidable>(collisionArea.colliders);

            fixedUpdateables.Add(player);
            fixedUpdateables.Add(stage);
            projectiles.ForEach(x => fixedUpdateables.Add(x));
            stageElements.AsEnumerable().ToList().ForEach(x => fixedUpdateables.Add(x));
            fixedUpdateables.Add(collisionArea);

            fixedUpdateablesManager = new EntityManager<IFixedUpdatable>(fixedUpdateables);
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
            collisionEntityManager.KillFlaggedObjects();
            fixedUpdateablesManager.KillFlaggedObjects();
            lerpEntityManager.KillFlaggedObjects();

            base.Update(gameTime);
        }

        private void FixedUpdate()
        {
            foreach (var item in fixedUpdateables)
            {
                item.FixedUpdate();
            }

            // Kill any object that goes off screen
            foreach (var item in lerpMovables)
            {
                if (item.Position.X < -50 || item.Position.X > 640 || item.Position.Y < -50 || item.Position.Y > 780)
                {
                    item.OnKill(EventArgs.Empty);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (var item in lerpMovables)
            {
                item.LerpDraw(_spriteBatch, ALPHA);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

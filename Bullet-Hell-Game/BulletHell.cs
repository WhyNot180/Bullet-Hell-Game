using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bullet_Hell_Game
{
    public class BulletHell : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private EntityManager<ILerpMovable> lerpEntityManager;
        private EntityManager<ILerpMovable> stageElementManager;
        private EntityManager<ICollidable> collisionEntityManager;

        // Fix updates to 30 fps
        private float fixedUpdateDelta = (int)(1000 / (float)30);

        private float previousTime = 0;
        private float timeAccumulator = 0.0f;
        private float maxFrameTime = 250;
        
        // this value stores how far we are in the current frame. For example, when the 
        // value of ALPHA is 0.5, it means we are halfway between the last frame and the 
        // next upcoming frame.
        private float ALPHA = 0;

        public ObservableCollection<ILerpMovable> lerpMovables = new ObservableCollection<ILerpMovable>();

        private Player player;

        private CollisionArea collisionArea;
        private Stage stage;

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
            _graphics.PreferredBackBufferHeight = 780;
            _graphics.ApplyChanges();

            collisionArea = new CollisionArea(new Rectangle(0, 0, 640, 780));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            player = new Player(new AnimatedSprite(Content.Load<Texture2D>("Sprites/Player"), 1, 1, 3), new Vector2(110,110), 13);
            ObservableCollection<StageElement> stageElements = new ObservableCollection<StageElement>() { new(Content.Load<Texture2D>("Sprites/Brass Pipe"), true, new RotatableShape(new Rectangle(605, 200, 80, 40), 0), new Vector2(545, 185), CollisionArea.CollisionType.Obstacle) };
            stage = new Stage(stageElements);

            lerpMovables.Add(player);
            lerpMovables.Add(stage);
            stageElements.AsEnumerable().ToList().ForEach(x => lerpMovables.Add(x));

            lerpEntityManager = new EntityManager<ILerpMovable>(() => { return lerpMovables; });

            collisionArea.colliders.Add(player);
            stageElements.AsEnumerable().ToList().ForEach(x => collisionArea.colliders.Add(x));

            collisionEntityManager = new EntityManager<ICollidable>(() => { return collisionArea.colliders;  });

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
            player.Update(1);
            stage.Update(1);
            collisionArea.Update();
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

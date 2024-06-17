using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Bullet_Hell_Game
{
    public class StageElement : ILerpMovable, ICollidable
    {
        public Vector2 Position { get; set; }
        public Vector2 PreviousPosition { get; private set; } = Vector2.Zero;
        public Vector2 LerpPosition { get; private set; } = Vector2.Zero;
        
        public bool IsCollidable { get; set; }
        public RotatableShape BoundingBox { get; set; }
        public CollisionArea.CollisionType CollisionType { get; set; }
        public bool CollisionChecked { get; set; } = false;
        private Texture2D texture;

        public event EventHandler? Kill;

        public StageElement(Texture2D texture, bool isCollidable, RotatableShape boundingBox, Vector2 position, CollisionArea.CollisionType collisionType)
        {
            this.texture = texture;
            IsCollidable = isCollidable;
            BoundingBox = boundingBox;
            CollisionType = collisionType;
            Position = position;
        }

        public virtual void OnCollision(CollisionArea.CollisionType collisionType, Vector2 minimumTranslationVector)
        {
            switch (collisionType)
            {
                case CollisionArea.CollisionType.Player:
                    break;
                default:
                    break;
            }
        }

        public void Move(Vector2 moveVelocity)
        {
            PreviousPosition = Position;
            Position += moveVelocity;
            BoundingBox.Move(new Vector2(BoundingBox.X, BoundingBox.Y) + moveVelocity);
        }

        public void LerpDraw(SpriteBatch spriteBatch, float ALPHA)
        {
            LerpPosition = Vector2.Lerp(PreviousPosition, Position, ALPHA);
            spriteBatch.Draw(texture, LerpPosition, null, Color.White, -BoundingBox.Rotation, Vector2.Zero, 3, SpriteEffects.None, 0);
        }

        public void OnKill(EventArgs e)
        {
            Kill?.Invoke(this, e);
        }
    }
}

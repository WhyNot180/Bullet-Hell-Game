using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Bullet_Hell_Game
{
    public class Stage : ILerpMovable
    {
        public Vector2 Position {  get; set; }
        public Vector2 PreviousPosition { get; private set; } = Vector2.Zero;
        public Vector2 LerpPosition { get; private set; } = Vector2.Zero;
        public Vector2 MoveVelocity { get; set; } = Vector2.Zero;
        public float Speed { get; set; }

        private List<StageElement> elements;

        public Stage(List<StageElement> elements)
        {
            this.elements = elements;
        }

        public void Move(float deltaSeconds)
        {
            Position = Vector2.Add(Position, Vector2.Multiply(MoveVelocity, deltaSeconds * Speed));
        }

        public void Update(float deltaTime)
        {
            elements.ForEach(element => element.Move(MoveVelocity*Speed));
        }

        public void LerpDraw(SpriteBatch spriteBatch, float ALPHA)
        {
            elements.ForEach(element => element.LerpDraw(spriteBatch, ALPHA));
        }

    }
}

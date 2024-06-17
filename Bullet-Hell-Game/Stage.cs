using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bullet_Hell_Game
{
    public class Stage : ILerpMovable
    {
        public Vector2 Position {  get; set; }
        public Vector2 PreviousPosition { get; private set; } = Vector2.Zero;
        public Vector2 LerpPosition { get; private set; } = Vector2.Zero;
        public Vector2 MoveDirection { get; set; } = Vector2.Zero;
        public float Speed { get; set; }

        public event EventHandler? Kill;

        public ObservableCollection<StageElement> elements;

        public Stage(ObservableCollection<StageElement> elements)
        {
            this.elements = elements;
        }

        public void Move(float deltaSeconds)
        {
            Position = Vector2.Add(Position, Vector2.Multiply(MoveDirection, deltaSeconds * Speed));
        }

        public void Update(float deltaTime)
        {
            elements.AsEnumerable().ToList().ForEach(element => element.MoveVelocity = MoveDirection * Speed);
        }

        public void LerpDraw(SpriteBatch spriteBatch, float ALPHA)
        {
            return;
        }
        
        public void OnKill(EventArgs e)
        {
            Kill?.Invoke(this, e);
        }

    }
}

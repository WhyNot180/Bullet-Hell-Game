using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bullet_Hell_Game
{
    /// <summary>
    /// A scrolling set of StageElements
    /// </summary>
    public class Stage : ILerpMovable, IFixedUpdatable
    {
        public Vector2 Position {  get; set; }
        public Vector2 PreviousPosition { get; private set; } = Vector2.Zero;
        public Vector2 LerpPosition { get; private set; } = Vector2.Zero;
        public Vector2 MoveDirection { get; set; } = Vector2.Zero;
        public float Speed { get; set; }

        public event EventHandler? Kill;

        public ObservableCollection<StageElement> elements;

        /// <summary>
        /// Initializes a Stage with a set of StageElements
        /// </summary>
        /// <param name="elements"></param>
        public Stage(ObservableCollection<StageElement> elements)
        {
            this.elements = elements;
        }

        public void Move()
        {
            Position = Vector2.Add(Position, Vector2.Multiply(MoveDirection, Speed));
        }

        public void FixedUpdate()
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

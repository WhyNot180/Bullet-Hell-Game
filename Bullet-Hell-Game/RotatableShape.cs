using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bullet_Hell_Game
{
    public class RotatableShape
    {
        public float X;
        public float Y;
        public float MaxWidth;
        public float MaxHeight;
        public float Rotation = 0;

        public List<Vector2> RelativeVertices;
        public List<Vector2> AbsoluteVertices;

        public RotatableShape(float x, float y, float width, float height, float radians, List<Vector2> vertices)
        {
            X = x;
            Y = y;
            MaxWidth = width; 
            MaxHeight = height;
            RelativeVertices = vertices;
            AbsoluteVertices = new List<Vector2>();
            Rotation = radians;
            RelativeVertices.ForEach(vert => AbsoluteVertices.Add(new Vector2(X + (vert.X * MathF.Cos(Rotation) + vert.Y * MathF.Sin(Rotation)), Y + (vert.X * MathF.Sin(Rotation) - vert.Y * MathF.Cos(Rotation)))));
        }

        public RotatableShape(Rectangle rect, float radians)
        {
            X = rect.X;
            Y = rect.Y;
            MaxWidth = rect.Width;
            MaxHeight = rect.Height;
            Rotation = radians;
            RelativeVertices = new List<Vector2>
            {
                new(-MaxWidth / 2, MaxHeight / 2),
                new(MaxWidth / 2, MaxHeight / 2),
                new(MaxWidth / 2, -MaxHeight / 2),
                new(-MaxWidth / 2, -MaxHeight / 2)
            };
            AbsoluteVertices = new List<Vector2>();
            RelativeVertices.ForEach(vert => AbsoluteVertices.Add(new Vector2(X + (vert.X * MathF.Cos(Rotation) + vert.Y * MathF.Sin(Rotation)), Y + (vert.X * MathF.Sin(Rotation) - vert.Y * MathF.Cos(Rotation)))));
        }

        public void Move(Vector2 newPosition)
        {
            X = newPosition.X;
            Y = newPosition.Y;
            AbsoluteVertices.Clear();
            RelativeVertices.ForEach(vert => AbsoluteVertices.Add(new Vector2(X + (vert.X * MathF.Cos(Rotation) + vert.Y * MathF.Sin(Rotation)), Y + (vert.X * MathF.Sin(Rotation) - vert.Y * MathF.Cos(Rotation)))));
        }
    }
}

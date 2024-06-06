using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Bullet_Hell_Game
{
    public class RotatableShape
    {
        public int X;
        public int Y;
        public int MaxWidth;
        public int MaxHeight;
        private float rotation = 0;
        public float Rotation {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
                for (int i = 0; i < OriginalVertices.Count; i++)
                {
                    float vectorLength = Math.Sign(OriginalVertices[i].X)*OriginalVertices[i].Length();
                    float angle = MathF.Acos(Vector2.Dot(OriginalVertices[i], Vector2.UnitY) / (vectorLength * Vector2.UnitY.Length()));
                    Vertices[i] = new Vector2(vectorLength * MathF.Sin(rotation + angle), vectorLength * MathF.Cos(rotation + angle));
                }
            }
        }

        public List<Vector2> OriginalVertices;
        public List<Vector2> Vertices;

        public RotatableShape(int x, int y, int width, int height, float radians, List<Vector2> vertices)
        {
            X = x;
            Y = y;
            MaxWidth = width; 
            MaxHeight = height;
            OriginalVertices = vertices;
            Vertices = vertices;
            Rotation = radians;
        }

        public RotatableShape(Rectangle rect, float radians)
        {
            X = rect.X;
            Y = rect.Y;
            MaxWidth = rect.Width;
            MaxHeight = rect.Height;
            OriginalVertices = new List<Vector2>
            {
                new(-MaxWidth / 2, -MaxHeight / 2),
                new(MaxWidth / 2, -MaxHeight / 2),
                new(MaxWidth / 2, MaxHeight / 2),
                new(-MaxWidth / 2, MaxHeight / 2)
            };
            Vertices = OriginalVertices;
            Rotation = radians;
        }

    }
}

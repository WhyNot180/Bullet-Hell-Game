using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Bullet_Hell_Game
{
    public class RotatableShape
    {
        /// <summary>
        /// X position of a RotatableShape's center
        /// </summary>
        public float X;

        /// <summary>
        /// Y position of a RotatableShape's center
        /// </summary>
        public float Y;

        public float MaxWidth;
        public float MaxHeight;

        /// <summary>
        /// Rotation in radians
        /// </summary>
        public float Rotation = 0;

        public float Radius;
        public bool IsCircle { get; private set; }

        /// <summary>
        /// Vertex coordinates relative to center of shape
        /// </summary>
        public List<Vector2> RelativeVertices;

        /// <summary>
        /// Vertex coordinates relative to screen
        /// </summary>
        public List<Vector2> AbsoluteVertices;

        /// <summary>
        /// Initializes a RotatableShape from a list of vertices
        /// </summary>
        /// <param name="x">X position of shape center</param>
        /// <param name="y">Y position of shape center</param>
        /// <param name="width">Maximum width</param>
        /// <param name="height">Maximum height</param>
        /// <param name="rotation">Rotation in radians</param>
        /// <param name="vertices">Vertex coordinates relative to shape center</param>
        public RotatableShape(float x, float y, float width, float height, float rotation, List<Vector2> vertices)
        {
            IsCircle = false;
            Radius = float.NaN;
            X = x;
            Y = y;
            MaxWidth = width; 
            MaxHeight = height;
            RelativeVertices = vertices;
            Rotation = rotation;

            // Create absolute vertices with regards to rotation matrix
            AbsoluteVertices = new List<Vector2>();
            RelativeVertices.ForEach(vert => AbsoluteVertices.Add(new Vector2(X + (vert.X * MathF.Cos(Rotation) + vert.Y * MathF.Sin(Rotation)), Y + (vert.X * MathF.Sin(Rotation) - vert.Y * MathF.Cos(Rotation)))));
        }

        /// <summary>
        /// Initializes a RotatableShape from a Rectangle
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="rotation">Rotation in radians</param>
        public RotatableShape(Rectangle rect, float rotation)
        {
            IsCircle = false;
            Radius = float.NaN;
            X = rect.X;
            Y = rect.Y;
            MaxWidth = rect.Width;
            MaxHeight = rect.Height;
            Rotation = rotation;
            RelativeVertices = new List<Vector2>
            {
                new(-MaxWidth / 2, MaxHeight / 2),
                new(MaxWidth / 2, MaxHeight / 2),
                new(MaxWidth / 2, -MaxHeight / 2),
                new(-MaxWidth / 2, -MaxHeight / 2)
            };

            // Create absolute vertices with regards to rotation matrix
            AbsoluteVertices = new List<Vector2>();
            RelativeVertices.ForEach(vert => AbsoluteVertices.Add(new Vector2(X + (vert.X * MathF.Cos(Rotation) + vert.Y * MathF.Sin(Rotation)), Y + (vert.X * MathF.Sin(Rotation) - vert.Y * MathF.Cos(Rotation)))));
        }

        /// <summary>
        /// Initializes a RotatableShape as a circle
        /// </summary>
        /// <param name="x">X position of circle center</param>
        /// <param name="y">Y position of circle center</param>
        /// <param name="radius"></param>
        public RotatableShape(float x, float y, float radius)
        {
            IsCircle = true;
            Radius = radius;
            MaxWidth = radius / 2;
            MaxHeight = radius / 2;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Updates the position of a RotatableShape with regards to rotation
        /// </summary>
        /// <param name="newPosition"></param>
        public void Move(Vector2 newPosition)
        {
            X = newPosition.X;
            Y = newPosition.Y;
            if (!IsCircle)
            {
                AbsoluteVertices.Clear();
                RelativeVertices.ForEach(vert => AbsoluteVertices.Add(new Vector2(X + (vert.X * MathF.Cos(Rotation) + vert.Y * MathF.Sin(Rotation)), Y + (vert.X * MathF.Sin(Rotation) - vert.Y * MathF.Cos(Rotation)))));
            }
        }
    }
}

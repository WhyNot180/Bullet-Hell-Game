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
        private double rotation = 0;
        public double Rotation {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
                for (int i = 0; i < OriginalVertices.Count; i++)
                {
                    double angle = Math.Acos(Vector2.Dot(OriginalVertices[i].ToVector2(), Vector2.UnitY) / (OriginalVertices[i].ToVector2().Length() * Vector2.UnitY.Length()));
                    double vectorLength = OriginalVertices[i].ToVector2().Length();
                    Vertices[i] = new Point((int) Math.Round(vectorLength * Math.Cos(rotation + angle)), (int) Math.Round(vectorLength * Math.Sin(rotation + angle)));
                }
            }
        }

        public List<Point> OriginalVertices;
        public List<Point> Vertices;

        public RotatableShape(int x, int y, int width, int height, double radians, List<Point> vertices)
        {
            X = x;
            Y = y;
            MaxWidth = width; 
            MaxHeight = height;
            OriginalVertices = vertices;
            Vertices = vertices;
            Rotation = radians;
        }

        public RotatableShape(Rectangle rect, double radians)
        {
            X = rect.X;
            Y = rect.Y;
            MaxWidth = rect.Width;
            MaxHeight = rect.Height;
            OriginalVertices = new List<Point>
            {
                new((int)Math.Round(Math.Sqrt((Math.Pow(X - MaxWidth, 2) + Math.Pow(Y - MaxHeight, 2)) / 4) * Math.Cos(Rotation)),
                    (int)Math.Round(Math.Sqrt((Math.Pow(X - MaxWidth, 2) + Math.Pow(Y - MaxHeight, 2)) / 4) * Math.Sin(Rotation))),
                new((int)Math.Round(Math.Sqrt((Math.Pow(X + MaxWidth, 2) + Math.Pow(Y - MaxHeight, 2)) / 4) * Math.Cos(Rotation)),
                    (int)Math.Round(Math.Sqrt((Math.Pow(X + MaxWidth, 2) + Math.Pow(Y - MaxHeight, 2)) / 4) * Math.Sin(Rotation))),
                new((int)Math.Round(Math.Sqrt((Math.Pow(X + MaxWidth, 2) + Math.Pow(Y + MaxHeight, 2)) / 4) * Math.Cos(Rotation)),
                    (int)Math.Round(Math.Sqrt((Math.Pow(X + MaxWidth, 2) + Math.Pow(Y + MaxHeight, 2)) / 4) * Math.Sin(Rotation))),
                new((int)Math.Round(Math.Sqrt((Math.Pow(X - MaxWidth, 2) + Math.Pow(Y + MaxHeight, 2)) / 4) * Math.Cos(Rotation)),
                    (int)Math.Round(Math.Sqrt((Math.Pow(X - MaxWidth, 2) + Math.Pow(Y + MaxHeight, 2)) / 4) * Math.Sin(Rotation)))
            };
            Rotation = radians;
        }

    }
}

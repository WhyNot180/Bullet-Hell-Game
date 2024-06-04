using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullet_Hell_Game
{
    public struct RotatableRectangle
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public double Rotation;

        public RotatableRectangle(int x, int y, int width, int height, double rotation)
        {
            X = x;
            Y = y;
            Width = width; 
            Height = height;
            Rotation = rotation;
        }

        public readonly Point CornerTopLeft
        {
            get
            {
                return new Point((int)Math.Round(Math.Sqrt((Math.Pow(X - Width, 2) + Math.Pow(Y - Height, 2)) / 4) * Math.Cos(Rotation)),
                                 (int)Math.Round(Math.Sqrt((Math.Pow(X - Width, 2) + Math.Pow(Y - Height, 2)) / 4) * Math.Sin(Rotation)));
            }
        }
        public readonly Point CornerTopRight
        {
            get
            {
                return new Point((int)Math.Round(Math.Sqrt((Math.Pow(X + Width, 2) + Math.Pow(Y - Height, 2)) / 4) * Math.Cos(Rotation)),
                                 (int)Math.Round(Math.Sqrt((Math.Pow(X + Width, 2) + Math.Pow(Y - Height, 2)) / 4) * Math.Sin(Rotation)));
            }
        }
        public readonly Point CornerBottomRight
        {
            get
            {
                return new Point((int)Math.Round(Math.Sqrt((Math.Pow(X + Width, 2) + Math.Pow(Y + Height, 2)) / 4) * Math.Cos(Rotation)),
                                 (int)Math.Round(Math.Sqrt((Math.Pow(X + Width, 2) + Math.Pow(Y + Height, 2)) / 4) * Math.Sin(Rotation)));
            }
        }
        public readonly Point CornerBottomLeft
        {
            get
            {
                return new Point((int)Math.Round(Math.Sqrt((Math.Pow(X - Width, 2) + Math.Pow(Y + Height, 2)) / 4) * Math.Cos(Rotation)),
                                 (int)Math.Round(Math.Sqrt((Math.Pow(X - Width, 2) + Math.Pow(Y + Height, 2)) / 4) * Math.Sin(Rotation)));
            }
        }


    }
}

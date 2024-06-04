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
        public int x;
        public int y;
        public int width;
        public int height;
        public double rotation;
        public readonly Point CornerTopLeft
        {
            get
            {
                return new Point((int)Math.Round(Math.Sqrt((Math.Pow(x - width, 2) + Math.Pow(y - height, 2)) / 4) * Math.Cos(rotation)),
                                 (int)Math.Round(Math.Sqrt((Math.Pow(x - width, 2) + Math.Pow(y - height, 2)) / 4) * Math.Sin(rotation)));
            }
        }
        public readonly Point CornerTopRight
        {
            get
            {
                return new Point((int)Math.Round(Math.Sqrt((Math.Pow(x + width, 2) + Math.Pow(y - height, 2)) / 4) * Math.Cos(rotation)),
                                 (int)Math.Round(Math.Sqrt((Math.Pow(x + width, 2) + Math.Pow(y - height, 2)) / 4) * Math.Sin(rotation)));
            }
        }
        public readonly Point CornerBottomRight
        {
            get
            {
                return new Point((int)Math.Round(Math.Sqrt((Math.Pow(x + width, 2) + Math.Pow(y + height, 2)) / 4) * Math.Cos(rotation)),
                                 (int)Math.Round(Math.Sqrt((Math.Pow(x + width, 2) + Math.Pow(y + height, 2)) / 4) * Math.Sin(rotation)));
            }
        }
        public readonly Point CornerBottomLeft
        {
            get
            {
                return new Point((int)Math.Round(Math.Sqrt((Math.Pow(x - width, 2) + Math.Pow(y + height, 2)) / 4) * Math.Cos(rotation)),
                                 (int)Math.Round(Math.Sqrt((Math.Pow(x - width, 2) + Math.Pow(y + height, 2)) / 4) * Math.Sin(rotation)));
            }
        }


    }
}

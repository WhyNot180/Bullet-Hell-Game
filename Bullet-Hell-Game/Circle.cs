using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullet_Hell_Game
{
    public struct Circle
    {
        public int x;
        public int y;
        public int radius;

        public Circle(int x, int y, int radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
        }
    }
}

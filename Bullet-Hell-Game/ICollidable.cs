using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullet_Hell_Game
{
    public interface ICollidable
    {
        bool IsCollidable { get; }
        Rectangle BoundingBox { get; }
        bool IsColliding(Rectangle rect);
        void OnCollision();
    }
}

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
        CollisionArea.CollisionType CollisionType { get; }
        RotatableShape BoundingBox { get; }
        void OnCollision(CollisionArea.CollisionType collisionType);
    }
}

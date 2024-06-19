using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullet_Hell_Game
{
    public interface ICollidable : IKillable
    {
        /// <summary>
        /// Set to false if you do not want collision at certain times on an ICollidable
        /// </summary>
        bool IsCollidable { get; }

        CollisionArea.CollisionType CollisionType { get; }

        RotatableShape BoundingBox { get; }

        void OnCollision(CollisionArea.CollisionType collisionType, Vector2 minimumTranslationVector);

        /// <summary>
        /// Set to true when collision has been accounted for during updates
        /// </summary>
        bool CollisionChecked { get; set; }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullet_Hell_Game
{
    public class CollisionArea
    {
        public enum CollisionType
        {
            EnemyProjectile,
            PlayerProjectile,
            Obstacle,
            Enemy,
            Player
        }

        public List<ICollidable> colliders = new List<ICollidable>();

        private QuadTree quadTree;

        public CollisionArea(Rectangle bounds)
        {
            quadTree = new QuadTree(0, bounds);
        }

        public void Update()
        {
            quadTree.Clear();
            colliders.ForEach(collider => quadTree.Insert(collider));

            List<ICollidable> returnColliders = new List<ICollidable>();
            foreach (var collider in colliders)
            {
                returnColliders.Clear();
                quadTree.Retrieve(returnColliders, collider.BoundingBox);
                foreach (var returnCollider in returnColliders)
                {
                    if (IsColliding(collider.BoundingBox, returnCollider.BoundingBox))
                    {
                        returnCollider.OnCollision(collider.CollisionType);
                        collider.OnCollision(collider.CollisionType);
                    }
                }
            }

        }

        public bool IsColliding(Rectangle collisionRectA, Rectangle collisionRectB)
        {

        }
    }
}

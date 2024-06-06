using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    if (!returnCollider.Equals(collider))
                    {
                        if (IsColliding(collider.BoundingBox, returnCollider.BoundingBox))
                        {
                            returnCollider.OnCollision(collider.CollisionType);
                            collider.OnCollision(returnCollider.CollisionType);
                        }
                    }
                }
            }

        }

        private bool IsColliding(RotatableShape collisionShapeA, RotatableShape collisionShapeB)
        {
            Vector2[] axesA = GetAxes(collisionShapeA);
            Vector2[] axesB = GetAxes(collisionShapeB);

            for (int i = 0; i < axesA.Length; i++)
            {
                Vector2 axis = axesA[i];

                Vector2 projectionA = MinMaxProjectVector(collisionShapeA, axis);
                Vector2 projectionB = MinMaxProjectVector(collisionShapeB, axis);
                
                if (projectionA.X > projectionB.Y || projectionB.X > projectionA.Y)
                {
                    return false;
                }
            }

            for (int i = 0; i < axesB.Length; i++)
            {
                Vector2 axis = axesB[i];

                Vector2 projectionA = MinMaxProjectVector(collisionShapeA, axis);
                Vector2 projectionB = MinMaxProjectVector(collisionShapeB, axis);
                
                if (projectionA.X > projectionB.Y || projectionB.X > projectionA.Y)
                {
                    return false;
                }
            }

            return true;
            
        }

        private Vector2 MinMaxProjectVector(RotatableShape collisionShape, Vector2 axis)
        {
            List<float> projections = new List<float>();
            collisionShape.Vertices.ForEach(vert =>  projections.Add(Vector2.Dot(axis, new Vector2(vert.X + collisionShape.X, vert.Y + collisionShape.Y))));
            
            double min = projections.Min();
            double max = projections.Max();

            return new Vector2((float) min, (float) max);
        }

        private Vector2[] GetAxes(RotatableShape collisionShape)
        {
            Vector2[] axes = new Vector2[collisionShape.Vertices.Count];
            for (int i = 0; i < axes.Length; i++)
            {
                Vector2 sideVect = Vector2.Subtract(collisionShape.Vertices[i], collisionShape.Vertices[i + 1 == collisionShape.Vertices.Count ? 0 : i + 1]);
                axes[i] = new Vector2(-sideVect.Y, sideVect.X);
                axes[i].Normalize();
            }
            return axes;
        }
    }
}

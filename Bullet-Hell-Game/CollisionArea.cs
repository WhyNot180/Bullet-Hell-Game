using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

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
                if (collider.IsCollidable)
                {
                    collider.CollisionChecked = true;
                    returnColliders.Clear();
                    quadTree.Retrieve(returnColliders, collider.BoundingBox);
                    foreach (var returnCollider in returnColliders)
                    {
                        if (!returnCollider.Equals(collider) && returnCollider.IsCollidable && !returnCollider.CollisionChecked)
                        {
                            if (IsColliding(collider.BoundingBox, returnCollider.BoundingBox, out Vector2 minTranslationVect))
                            {
                                returnCollider.OnCollision(collider.CollisionType, -minTranslationVect);
                                collider.OnCollision(returnCollider.CollisionType, minTranslationVect);
                            }
                        }
                    }
                }
            }
            colliders.ForEach(collider => collider.CollisionChecked = false);

        }

        private bool IsColliding(RotatableShape collisionShapeA, RotatableShape collisionShapeB, out Vector2 minimumTranslationVector)
        {
            minimumTranslationVector = Vector2.Zero;

            float overlap = float.MaxValue;
            Vector2 overlapDirection = Vector2.Zero;

            Vector2[] axesA = GetAxes(collisionShapeA);
            Vector2[] axesB = GetAxes(collisionShapeB);

            for (int i = 0; i < axesA.Length; i++)
            {
                Vector2 axis = axesA[i];

                Vector2 projectionA = MinMaxProjectVector(collisionShapeA, axis);
                Vector2 projectionB = MinMaxProjectVector(collisionShapeB, axis);

                if (projectionA.X >= projectionB.Y || projectionB.X >= projectionA.Y)
                {
                    return false;
                } else
                {
                    float projectionOverlap = MathF.Min(projectionB.Y - projectionA.X, projectionA.Y - projectionB.X);

                    if (projectionOverlap < overlap)
                    {
                        overlap = projectionOverlap;
                        overlapDirection = axis;
                    }
                }
            }

            for (int i = 0; i < axesB.Length; i++)
            {
                Vector2 axis = axesB[i];

                Vector2 projectionA = MinMaxProjectVector(collisionShapeA, axis);
                Vector2 projectionB = MinMaxProjectVector(collisionShapeB, axis);

                if (projectionA.X >= projectionB.Y || projectionB.X >= projectionA.Y)
                {
                    return false;
                } else
                {
                    float projectionOverlap = MathF.Min(projectionB.Y - projectionA.X, projectionA.Y - projectionB.X);

                    if (projectionOverlap < overlap)
                    {
                        overlap = projectionOverlap;
                        overlapDirection = axis;
                    }
                }
            }

            Vector2 centerCollidersAB = new Vector2(collisionShapeB.X  - collisionShapeA.X, collisionShapeB.Y - collisionShapeA.Y);

            if (Vector2.Dot(centerCollidersAB, overlapDirection) > 0f)
            {
                overlapDirection = -overlapDirection;
            }

            minimumTranslationVector = Vector2.Multiply(overlapDirection, overlap);

            return true;
            
        }

        private Vector2 MinMaxProjectVector(RotatableShape collisionShape, Vector2 axis)
        {
            float min = float.MaxValue;
            float max = float.MinValue;

            for (int i = 0; i < collisionShape.AbsoluteVertices.Count; i++)
            {
                float projection = Vector2.Dot(collisionShape.AbsoluteVertices[i], axis);
                if (projection < min) { min = projection; }
                else if (projection > max) { max = projection; }
            }

            return new Vector2((float) min, (float) max);
        }

        private Vector2[] GetAxes(RotatableShape collisionShape)
        {
            Vector2[] axes = new Vector2[collisionShape.RelativeVertices.Count];
            for (int i = 0; i < axes.Length; i++)
            {
                Vector2 sideVect = Vector2.Subtract(collisionShape.RelativeVertices[(i + 1) % axes.Length], collisionShape.RelativeVertices[i]);
                axes[i] = new Vector2(-sideVect.Y, sideVect.X);
                axes[i].Normalize();
            }
            return axes;
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        public ObservableCollection<ICollidable> colliders = new ObservableCollection<ICollidable>();

        private QuadTree quadTree;

        public CollisionArea(Rectangle bounds)
        {
            quadTree = new QuadTree(0, bounds);
        }

        public void Update()
        {
            quadTree.Clear();
            colliders.AsEnumerable().ToList().ForEach(collider => quadTree.Insert(collider));

            List<ICollidable> returnColliders = new List<ICollidable>();
            foreach (var collider in colliders.AsEnumerable().ToList())
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
            colliders.AsEnumerable().ToList().ForEach(collider => collider.CollisionChecked = false);

        }

        private bool IsColliding(RotatableShape collisionShapeA, RotatableShape collisionShapeB, out Vector2 minimumTranslationVector)
        {
            minimumTranslationVector = Vector2.Zero;

            float overlap = float.MaxValue;
            Vector2 overlapDirection = Vector2.Zero;

            if(collisionShapeA.IsCircle && collisionShapeB.IsCircle)
            {
                Vector2[] axis = new Vector2[1];
                axis[0] = new Vector2(collisionShapeB.X - collisionShapeA.X, collisionShapeB.Y - collisionShapeA.Y);
                axis[0].Normalize();
                if (!AxesCheck(axis, collisionShapeA, collisionShapeB, ref overlap, ref overlapDirection))
                {
                    return false;
                }
            } else if (!collisionShapeA.IsCircle && !collisionShapeB.IsCircle)
            {
                Vector2[] axesA = GetAxes(collisionShapeA);
                Vector2[] axesB = GetAxes(collisionShapeB);
                if (!AxesCheck(axesA, collisionShapeA, collisionShapeB, ref overlap, ref overlapDirection) || !AxesCheck(axesB, collisionShapeA, collisionShapeB, ref overlap, ref overlapDirection))
                {
                    return false;
                }
            } else
            {
                Vector2[] axes;

                if (collisionShapeA.IsCircle)
                {
                    Vector2[] polygonAxes;
                    polygonAxes = GetAxes(collisionShapeB);
                    axes = new Vector2[polygonAxes.Length + 1];
                    axes[0] = Vector2.Normalize(collisionShapeB.AbsoluteVertices[FindClosestPointIndex(collisionShapeA, collisionShapeB)] - new Vector2(collisionShapeA.X, collisionShapeA.Y));
                    polygonAxes.CopyTo(axes, 1);
                } else
                {
                    Vector2[] polygonAxes;
                    polygonAxes = GetAxes(collisionShapeA);
                    axes = new Vector2[polygonAxes.Length + 1];
                    axes[0] = Vector2.Normalize(collisionShapeB.AbsoluteVertices[FindClosestPointIndex(collisionShapeB, collisionShapeA)] - new Vector2(collisionShapeB.X, collisionShapeB.Y));
                    polygonAxes.CopyTo(axes, 1);
                }

                if (!AxesCheck(axes, collisionShapeA, collisionShapeB, ref overlap, ref overlapDirection))
                {
                    return false;
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

        private bool AxesCheck(Vector2[] axes, RotatableShape collisionShapeA, RotatableShape collisionShapeB, ref float overlap, ref Vector2 overlapDirection)
        {
            for (int i = 0; i < axes.Length; i++)
            {
                Vector2 axis = axes[i];

                Vector2 projectionA;
                if (collisionShapeA.IsCircle) projectionA = ProjectCircle(collisionShapeA, axis);
                else projectionA = ProjectPolygon(collisionShapeA, axis);

                Vector2 projectionB;
                if (collisionShapeB.IsCircle) projectionB = ProjectCircle(collisionShapeB, axis);
                else projectionB = ProjectPolygon(collisionShapeB, axis);

                if (projectionA.X >= projectionB.Y || projectionB.X >= projectionA.Y)
                {
                    return false;
                }
                else
                {
                    float projectionOverlap = MathF.Min(projectionB.Y - projectionA.X, projectionA.Y - projectionB.X);

                    if (projectionOverlap < overlap)
                    {
                        overlap = projectionOverlap;
                        overlapDirection = axis;
                    }
                }
            }
            return true;
        }

        private int FindClosestPointIndex(RotatableShape collisionShape, RotatableShape collisionPolygon)
        {
            int vertexIndex = -1;
            float minDistance = float.MaxValue;

            for (int i = 0; i < collisionPolygon.AbsoluteVertices.Count; i++)
            {
                Vector2 vertex = collisionPolygon.AbsoluteVertices[i];
                float distance = Math.Abs((vertex - new Vector2(collisionShape.X, collisionShape.Y)).Length());

                if (distance < minDistance)
                {
                    minDistance = distance;
                    vertexIndex = i;
                }

            }
            return vertexIndex;
        }

        private Vector2 ProjectCircle(RotatableShape collisionShape, Vector2 axis)
        {
            Vector2 radiusVector = axis * collisionShape.Radius;
            Vector2 point1 = new Vector2(collisionShape.X, collisionShape.Y) + radiusVector;
            Vector2 point2 = new Vector2(collisionShape.X, collisionShape.Y) - radiusVector;

            float min = Vector2.Dot(point1, axis);
            float max = Vector2.Dot(point2, axis);

            if (min > max)
            {
                float t = min;
                min = max;
                max = t;
            }

            return new Vector2(min, max);
        }

        private Vector2 ProjectPolygon(RotatableShape collisionShape, Vector2 axis)
        {
            float min = float.MaxValue;
            float max = float.MinValue;

            for (int i = 0; i < collisionShape.AbsoluteVertices.Count; i++)
            {
                float projection = Vector2.Dot(collisionShape.AbsoluteVertices[i], axis);
                if (projection < min) { min = projection; }
                else if (projection > max) { max = projection; }
            }

            return new Vector2(min, max);
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

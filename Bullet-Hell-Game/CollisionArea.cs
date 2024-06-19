using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bullet_Hell_Game
{
    public class CollisionArea : IFixedUpdatable
    {
        /// <summary>
        /// Available types of collisions
        /// </summary>
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

        public event EventHandler? Kill;

        /// <summary>
        /// Initializes a CollisionArea to set bounds
        /// </summary>
        /// <param name="bounds">Bounds of QuadTree</param>
        public CollisionArea(Rectangle bounds)
        {
            quadTree = new QuadTree(0, bounds);
        }

        public void FixedUpdate()
        {
            // Reset quad tree
            quadTree.Clear();
            colliders.AsEnumerable().ToList().ForEach(collider => quadTree.Insert(collider));

            List<ICollidable> returnColliders = new List<ICollidable>();
            foreach (var collider in colliders.AsEnumerable().ToList())
            {
                // Check if collisions are enabled for collider
                if (collider.IsCollidable)
                {
                    // Make sure collision is not checked more than once
                    collider.CollisionChecked = true;

                    // Get nearby colliders for each collider
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

            // Circle-Circle collisions
            if(collisionShapeA.IsCircle && collisionShapeB.IsCircle)
            {
                // Find vector from centers of circles
                Vector2[] axis = new Vector2[1];
                axis[0] = new Vector2(collisionShapeB.X - collisionShapeA.X, collisionShapeB.Y - collisionShapeA.Y);
                axis[0].Normalize();

                // Check if centers + radii overlap
                if (!AxesCheck(axis, collisionShapeA, collisionShapeB, ref overlap, ref overlapDirection))
                {
                    return false;
                }

            // Polygon-Polygon collisions
            } else if (!collisionShapeA.IsCircle && !collisionShapeB.IsCircle)
            {
                // Get normals of each side for both polygons
                Vector2[] axesA = GetAxes(collisionShapeA);
                Vector2[] axesB = GetAxes(collisionShapeB);

                // Check if any projections on axes overlap
                if (!AxesCheck(axesA, collisionShapeA, collisionShapeB, ref overlap, ref overlapDirection) || !AxesCheck(axesB, collisionShapeA, collisionShapeB, ref overlap, ref overlapDirection))
                {
                    return false;
                }

            // Circle-Polygon collisions
            } else
            {
                Vector2[] axes;

                if (collisionShapeA.IsCircle)
                {
                    Vector2[] polygonAxes;
                    polygonAxes = GetAxes(collisionShapeB);
                    axes = new Vector2[polygonAxes.Length + 1];
                    
                    // Get vector from closest point on polygon to center of circle
                    axes[0] = Vector2.Normalize(collisionShapeB.AbsoluteVertices[FindClosestPointIndex(collisionShapeA, collisionShapeB)] - new Vector2(collisionShapeA.X, collisionShapeA.Y));
                    polygonAxes.CopyTo(axes, 1);
                } else
                {
                    Vector2[] polygonAxes;
                    polygonAxes = GetAxes(collisionShapeA);
                    axes = new Vector2[polygonAxes.Length + 1];

                    // Get vector from closest point on polygon to center of circle
                    axes[0] = Vector2.Normalize(collisionShapeB.AbsoluteVertices[FindClosestPointIndex(collisionShapeB, collisionShapeA)] - new Vector2(collisionShapeB.X, collisionShapeB.Y));
                    polygonAxes.CopyTo(axes, 1);
                }

                // Check for projection overlap
                if (!AxesCheck(axes, collisionShapeA, collisionShapeB, ref overlap, ref overlapDirection))
                {
                    return false;
                }
            }

            Vector2 centerCollidersAB = new Vector2(collisionShapeB.X  - collisionShapeA.X, collisionShapeB.Y - collisionShapeA.Y);

            // Get proper min translation vector direction
            if (Vector2.Dot(centerCollidersAB, overlapDirection) > 0f)
            {
                overlapDirection = -overlapDirection;
            }

            minimumTranslationVector = Vector2.Multiply(overlapDirection, overlap);

            return true;
            
        }

        /// <summary>
        /// Checks for overlap of projections on an axis
        /// </summary>
        /// <param name="axes"></param>
        /// <param name="collisionShapeA"></param>
        /// <param name="collisionShapeB"></param>
        /// <param name="overlap"></param>
        /// <param name="overlapDirection"></param>
        /// <returns></returns>
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

                // No overlap if min of one is bigger than max of the other
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

        /// <summary>
        /// Finds the index of the closest point of a Polygon to the center of another shape
        /// </summary>
        /// <param name="collisionShape"></param>
        /// <param name="collisionPolygon"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Projects a circle onto an axis
        /// </summary>
        /// <param name="collisionShape">Circle</param>
        /// <param name="axis"></param>
        /// <returns>A vector of the minimum and maximum projection (min, max)</returns>
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

        /// <summary>
        /// Projects a polygon onto an axis
        /// </summary>
        /// <param name="collisionShape">Polygon</param>
        /// <param name="axis"></param>
        /// <returns>A vector of the minimum and maximum projection (min, max)</returns>
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

        /// <summary>
        /// Returns the normals of each side on a polygon
        /// </summary>
        /// <param name="collisionShape">Polygon</param>
        /// <returns>Array of each normal vector</returns>
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

        public void OnKill(EventArgs e)
        {
            Kill.Invoke(this, e);
        }
    }
}

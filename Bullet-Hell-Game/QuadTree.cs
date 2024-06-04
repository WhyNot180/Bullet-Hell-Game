using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Bullet_Hell_Game
{
    public class QuadTree
    {
        private const int MaxObjects = 10;
        private const int MaxLevels = 5;

        private int level;
        private List<ICollidable> objects;
        private Rectangle bounds;
        private QuadTree[] nodes;

        public QuadTree(int level, Rectangle bounds)
        {
            this.level = level;
            this.bounds = bounds;
            objects = new List<ICollidable>();
            nodes = new QuadTree[4];
        }

        public void Clear()
        {
            objects.Clear();
            for(int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null)
                {
                    nodes[i].Clear();
                    nodes[i] = null;
                }
            }
        }

        private void Split()
        {
            int subWidth = bounds.Width / 2;
            int subHeight = bounds.Height / 2;
            int x = bounds.X;
            int y = bounds.Y;

            nodes[0] = new QuadTree(level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
            nodes[1] = new QuadTree(level + 1, new Rectangle(x, y, subWidth, subHeight));
            nodes[2] = new QuadTree(level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
            nodes[3] = new QuadTree(level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        private int GetQuadrant(RotatableRectangle rect)
        {
            int quadrant = -1;
            double verticalMidpoint = bounds.Y + bounds.Height / 2;
            double horizontalMidpoint = bounds.X + bounds.Width / 2;

            bool topQuadrants = (rect.Y <  verticalMidpoint && rect.Y + rect.Height < verticalMidpoint);
            bool bottomQuadrants = (rect.Y > verticalMidpoint);
            bool leftQuadrants = (rect.X < horizontalMidpoint && rect.X + rect.Width < horizontalMidpoint);
            bool rightQuadrants = (rect.X >  horizontalMidpoint);

            if (leftQuadrants)
            {
                if (topQuadrants)
                {
                    quadrant = 1;
                } else if (bottomQuadrants)
                {
                    quadrant = 2;
                }
            } else if (rightQuadrants)
            {
                if (topQuadrants)
                {
                    quadrant = 0;
                } else if (bottomQuadrants)
                {
                    quadrant = 3;
                }
            }

            return quadrant;
        }

        public void Insert(ICollidable collider)
        {
            if (nodes[0] != null)
            {
                int index = GetQuadrant(collider.BoundingBox);

                if (index != -1)
                {
                    nodes[index].Insert(collider);

                    return;
                }
            }

            objects.Add(collider);

            if (objects.Count > MaxObjects && level > MaxLevels)
            {
                if (nodes[0] == null)
                {
                    Split();
                }

                int i = 0;
                while (i < objects.Count)
                {
                    int index = GetQuadrant(objects[i].BoundingBox);
                    if (index != -1)
                    {
                        nodes[index].Insert(objects[i]);
                        objects.RemoveAt(i);
                    } else
                    {
                        i++;
                    }
                }
            }
        }

        public List<ICollidable> Retrieve(List<ICollidable> returnObjects, RotatableRectangle rect)
        {
            int index = GetQuadrant(rect);
            if (index != -1 && nodes[0] != null)
            {
                nodes[index].Retrieve(returnObjects, rect);
            }

            returnObjects.AddRange(objects);

            return returnObjects;
        }
    }
}

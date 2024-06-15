﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullet_Hell_Game
{
    public class Player : Character, ICollidable
    {
        private int lives = 3;

        public CollisionArea.CollisionType CollisionType { get; } = CollisionArea.CollisionType.Player;

        public RotatableShape BoundingBox { get; private set; }
        public bool IsCollidable { get; set; } = true;
        public bool CollisionChecked { get; set; } = false;

        public Player(AnimatedSprite sprite, Vector2 position, float speed) : base(sprite, position, Vector2.Zero, speed) 
        {
            float width = sprite.Texture.Width * sprite.Scale;
            float height = sprite.Texture.Height * sprite.Scale;
            BoundingBox = new RotatableShape(position.X + (width/2), position.Y + (height/2), width/2);
        }

        public override void Update(float deltaSeconds)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                MoveVelocity = new Vector2(MoveVelocity.X, -1);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                MoveVelocity = new Vector2(MoveVelocity.X, 1);
            }
            else
            {
                MoveVelocity = new Vector2(MoveVelocity.X, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                MoveVelocity = new Vector2(1, MoveVelocity.Y);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                MoveVelocity = new Vector2(-1, MoveVelocity.Y);
            }
            else
            {
                MoveVelocity = new Vector2(0, MoveVelocity.Y);
            }

            if (!MoveVelocity.Equals(Vector2.Zero))
            {
                MoveVelocity = Vector2.Normalize(MoveVelocity);
            }
            base.Update(deltaSeconds);
            BoundingBox.Move(new Vector2(BoundingBox.X, BoundingBox.Y) + MoveVelocity*Speed);
        }

        public void OnCollision(CollisionArea.CollisionType collisionType, Vector2 minimumTranslationVector)
        {
            Position = Vector2.Add(Position, minimumTranslationVector/2);
            BoundingBox.Move(new Vector2(BoundingBox.X, BoundingBox.Y) + minimumTranslationVector / 2);
            MoveVelocity = Vector2.Zero;
        }

    }
}

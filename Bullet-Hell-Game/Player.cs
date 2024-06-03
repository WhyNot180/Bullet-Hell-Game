using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullet_Hell_Game
{
    public class Player : Character
    {
        private int lives;

        public Player(AnimatedSprite sprite, Vector2 position, float speed) : base(sprite, position, Vector2.Zero, speed) { }


    }
}

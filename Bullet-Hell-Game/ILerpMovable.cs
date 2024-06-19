using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullet_Hell_Game
{
    /// <summary>
    /// Used for any object that does not guarantee smooth movement (i.e. movement on a fixed framerate)
    /// </summary>
    public interface ILerpMovable : IKillable
    {
        Vector2 Position { get; }
        Vector2 PreviousPosition { get; }
        Vector2 LerpPosition { get; }
        void LerpDraw(SpriteBatch spriteBatch, float ALPHA);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Bullet_Hell_Game
{
    public interface ILerpMovable
    {
        Vector2 Position { get; }
        Vector2 PreviousPosition { get; }
    }
}

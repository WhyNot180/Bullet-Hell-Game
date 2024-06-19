using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullet_Hell_Game
{
    /// <summary>
    /// Used by objects that must be updated at a fixed framerate
    /// </summary>
    public interface IFixedUpdatable : IKillable
    {
        void FixedUpdate();
    }
}

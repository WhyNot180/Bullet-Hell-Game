using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullet_Hell_Game
{
    /// <summary>
    /// Used for any objects that might not be automatically collected by the garbage collector.
    /// I.e. any object that is in a list not accessible directly by the object
    /// </summary>
    public interface IKillable
    {
        /// <summary>
        /// Call when killing a killable entity
        /// </summary>
        /// <param name="e"></param>
        void OnKill(EventArgs e);

        event EventHandler? Kill;
    }
}

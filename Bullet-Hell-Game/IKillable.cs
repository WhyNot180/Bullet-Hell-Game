using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullet_Hell_Game
{
    public interface IKillable
    {
        void OnKill(EventArgs e);
        event EventHandler? Kill;
    }
}

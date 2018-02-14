using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeonGeneratorPoC
{
    interface ICollidable<T>
    {
        T GetCollisionBox();
        Boolean CheckCollision(T t);
    }
}

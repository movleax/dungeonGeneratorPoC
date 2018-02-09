using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dungeonGeneratorPoC
{
    static class RandomManager
    {
        static Random rand = new Random();

        static public Random GetRandomInstance()
        {
            return rand;
        }
    }
}

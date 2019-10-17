using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciphers.Extensions
{
    class Position
    {
        public int x { get; set; }
        public int y { get; set; }

        public Position(int y, int x)
        {
            this.x = x;
            this.y = y;
        }
    }
}

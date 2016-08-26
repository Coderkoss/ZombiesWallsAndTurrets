using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZombiesWallsAndTurrets
{
    public class WorldCell
    {
        public string  occupiedType;
        public Rectangle cellRect;
        public int index;
        //c-10  c-1  c+8
        //c-9   c    c+9
        //c-8   c+1  c+10
        public WorldCell(string occupiedtype,Rectangle rect)
        {
            this.occupiedType = occupiedtype;
            this.cellRect = rect;
        }
    }
}

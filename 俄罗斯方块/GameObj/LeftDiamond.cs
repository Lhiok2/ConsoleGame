using System;
using System.Collections.Generic;
using System.Text;

namespace 俄罗斯方块
{
    class LeftDiamond : BaseSquare
    {

        public LeftDiamond(int x, int y) : base(x, y, ConsoleColor.DarkBlue)
        {
            units = new List<List<Vector>>()
            {
                { new List<Vector>() { new Vector(0, -1), new Vector(0, 0), new Vector(2, 0), new Vector(2, 1) } },
                { new List<Vector>() { new Vector(0, 0), new Vector(2, 0), new Vector(-2, 1), new Vector(0, 1) } },
                { new List<Vector>() { new Vector(-2, -1), new Vector(-2, 0), new Vector(0, 0), new Vector(0, 1) } },
                { new List<Vector>() { new Vector(0, -1), new Vector(2, -1), new Vector(-2, 0), new Vector(0, 0) } }
            };

            InitTrans();
        }
    }
}

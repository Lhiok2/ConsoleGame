using System;
using System.Collections.Generic;
using System.Text;

namespace 飞行棋
{
    class Unit
    {
        public string tag;
        public ConsoleColor color;
        public string describe;

        public Unit(string tag, string describe, ConsoleColor color)
        {
            this.tag = tag;
            this.color = color;
            this.describe = describe;
        }
    }
}

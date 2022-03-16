using System;
using System.Collections.Generic;
using System.Text;

namespace 俄罗斯方块
{
    enum E_Limit
    {
        None = 0,
        Left = 1,
        Right = 1 << 1,
        Down = 1 << 2,
        All = Left + Right + Down,
    }
}

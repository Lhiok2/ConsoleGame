using System;
using System.Collections.Generic;
using System.Text;

namespace 俄罗斯方块
{
    interface IScene : IDraw
    {
        void Show();
        void Update();
        void Hidden();
    }
}

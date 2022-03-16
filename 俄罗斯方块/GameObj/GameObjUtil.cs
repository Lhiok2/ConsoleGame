using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace 俄罗斯方块
{
    static class GameObjUtil
    {
        public static Random random = new Random();

        private static Type[] _types = new Type[]
        {
            typeof(Square),
            typeof(Rectangle),
            typeof(Triangle),
            typeof(LeftDiamond),
            typeof(RightDiamond),
            typeof(LeftTrapezoid),
            typeof(RightTrapezoid)
        };

        private static Type[] _constructorParams = new Type[]
        {
            typeof(int), typeof(int)
        };

        public static int RandInt(int maxVal)
        {
            return random.Next(maxVal);
        }

        public static BaseSquare RandSquare(int x, int y)
        {
            Type type = _types[random.Next(_types.Length)];
            ConstructorInfo ctor = type.GetConstructor(_constructorParams);
            return ctor.Invoke(new object[] { x, y }) as BaseSquare;
        }
    }
}

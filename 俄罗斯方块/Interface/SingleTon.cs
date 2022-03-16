using System;
using System.Collections.Generic;
using System.Text;

namespace 俄罗斯方块
{
    class SingleTon<T> where T : new()
    {
        private static T _instance;

        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }

                return _instance;
            }
        }
    }
}

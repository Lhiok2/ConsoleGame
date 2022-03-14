using System;

namespace 飞行棋
{
    abstract class IScene
    {
        public E_Scene Enter()
        {
            Init();
            return Run();
        }

        protected ConsoleKey Input => Console.ReadKey(true).Key;

        protected abstract E_Scene Run();

        protected abstract void Init();
    }
}

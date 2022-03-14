using System;

namespace 飞行棋
{
    class ExitScene : IScene
    {
        protected override void Init()
        {
            Console.Clear();
        }

        protected override E_Scene Run()
        {
            Environment.Exit(0);
            return E_Scene.Start;
        }
    }
}

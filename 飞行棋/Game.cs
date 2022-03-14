using System;
using System.Collections.Generic;

namespace 飞行棋
{
    static class Game
    {
        public static string Game_Name = "飞行棋";
        public static int Console_Width = 80;
        public static int Console_Height = 50;

        public static E_Scene Cur_Scene;

        private static Dictionary<E_Scene, IScene> dict = new Dictionary<E_Scene, IScene>()
        {
            { E_Scene.Start, new StartScene() },
            { E_Scene.Main, new MainScene() },
            { E_Scene.Result, new ResultScene() },
            { E_Scene.Exit, new ExitScene() }
        };

        public static void Start()
        {
            Init();

            while (true)
            {
                Cur_Scene = dict[Cur_Scene].Enter();
            }
        }

        private static void Init()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(Console_Width, Console_Height + 1);
            Console.SetBufferSize(Console_Width, Console_Height + 1);
            Console.Clear();

            Cur_Scene = E_Scene.Start;
        }
    }
}

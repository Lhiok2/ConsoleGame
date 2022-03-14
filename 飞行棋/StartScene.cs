using System;
using System.Collections.Generic;

namespace 飞行棋
{
    class StartSceneData
    {
        public string gameName => Game.Game_Name;

        public List<string> sceneName = new List<string>()
        {
            "开始游戏",
            "退出游戏"
        };

        public List<E_Scene> sceneList = new List<E_Scene>()
        {
            E_Scene.Main,
            E_Scene.Exit 
        };

        private int _nextSceneIndex = 0;

        public void pressW()
        {
            if (--_nextSceneIndex < 0)
            {
                _nextSceneIndex += sceneName.Count;
            }
        }

        public void pressS()
        {
            if (++_nextSceneIndex >= sceneName.Count)
            {
                _nextSceneIndex -= sceneName.Count;
            }
        }

        public string nextSceneName => sceneName[_nextSceneIndex];
        public E_Scene nextScene => sceneList[_nextSceneIndex];

        public int namePosX => Math.Max(0, (Game.Console_Width - gameName.Length) / 2);
        public int namePosY => Math.Max(0, Game.Console_Height / 3);

        public int namePosYInterval = 1;

        public int scenePosX(string name)
        {
            return Math.Max(0, (Game.Console_Width - name.Length - 1) / 2);
        }

        public int scenePosY => Math.Max(1, Game.Console_Height / 2);

        public void Init()
        {
            _nextSceneIndex = 0;
        }
    }

    class StartScene : IScene
    {
        private StartSceneData data = new StartSceneData();

        protected override void Init()
        {
            Console.Clear();

            data.Init();

            Console.SetCursorPosition(data.namePosX, data.namePosY);
            Console.Write(data.gameName);
        }

        protected override E_Scene Run()
        {
            while (true)
            {
                Draw();
                switch (Input)
                {
                    case ConsoleKey.W:
                        data.pressW();
                        break;
                    case ConsoleKey.S:
                        data.pressS();
                        break;
                    case ConsoleKey.J:
                        return data.nextScene;
                }
            }
        }

        private void Draw()
        {
            int posY = data.scenePosY;

            data.sceneName.ForEach((name) =>
            {
                Console.SetCursorPosition(data.scenePosX(name), posY);
                Console.ForegroundColor = data.nextSceneName == name ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write(name);
                posY += data.namePosYInterval + 1;
            });
        }

    }
}

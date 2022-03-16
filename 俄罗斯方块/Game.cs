using System;
using System.Threading;

namespace 俄罗斯方块
{
    class Game : SingleTon<Game>
    {
        public const int Window_Width = 60;
        public const int Window_Height = 40;
        private const int Speed = 100;

        public int score;

        private IScene _curScene;

        private void Init()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(Window_Width, Window_Height);
            Console.SetBufferSize(Window_Width, Window_Height);

            scene = E_Scene.Start;
        }

        public void Start()
        {
            Init();
            Loop();
        }

        private void Loop()
        {
            while (true)
            {
                if (_curScene == null)
                {
                    Exit();
                    return;
                }

                _curScene?.Update();

                Thread.Sleep(Speed);
            }
        }

        public void DrawUnit(int x, int y, string tag)
        {
            if (x < 0 || x >= Game.Window_Width || y < 0 || y >= Game.Window_Height)
            {
                return;
            }

            Console.SetCursorPosition(x, y);
            Console.Write(tag);
        }

        #region 切换场景
        public E_Scene scene
        {
            set
            {
                _curScene?.Hidden();

                switch (value)
                {
                    case E_Scene.None:
                        Exit();
                        _curScene = null;
                        break;
                    case E_Scene.Start:
                        _curScene = new StartScene();
                        break;
                    case E_Scene.Main:
                        _curScene = new MainScene();
                        break;
                    case E_Scene.Result:
                        _curScene = new ResultScene();
                        break;
                }

                _curScene?.Show();
            }
        }
        #endregion

        private void Exit()
        {
            Environment.Exit(0);
        }
    }
}

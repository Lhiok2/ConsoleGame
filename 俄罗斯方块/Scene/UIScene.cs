using System;
using System.Threading;

namespace 俄罗斯方块
{
    class UIScene : IScene
    {
        protected string titleName;
        protected string optionNameFirst;
        protected string optionNameSecond;

        private const int OptionName_Interval = 3;

        private int _selectedIndex;

        #region 接口逻辑
        public virtual void Draw()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;
            Game.instance.DrawUnit(Game.Window_Width / 2 - titleName.Length, Game.Window_Height / 3, titleName);

            Console.ForegroundColor =  _selectedIndex == 0? ConsoleColor.Red: ConsoleColor.White;
            Game.instance.DrawUnit(Game.Window_Width / 2 - optionNameFirst.Length, Game.Window_Height / 2, optionNameFirst);

            Console.ForegroundColor = _selectedIndex == 1 ? ConsoleColor.Red : ConsoleColor.White;
            Game.instance.DrawUnit(Game.Window_Width / 2 - optionNameSecond.Length, Game.Window_Height / 2 + OptionName_Interval, optionNameSecond);
        }

        public virtual void Hidden()
        {
            InputThread.instance.action -= InputCheck;
            Console.Clear();
        }

        public virtual void Show()
        {
            _selectedIndex = 0;
            Draw();

            InputThread.instance.action += InputCheck;
        }

        public virtual void Update()
        {
        }
        #endregion

        #region 输入检测
        private void InputCheck()
        {
            if (!Console.KeyAvailable)
            {
                return;
            }

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    _selectedIndex = 0;
                    Draw();
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    _selectedIndex = 1;
                    Draw();
                    break;
                case ConsoleKey.Enter:
                case ConsoleKey.J:
                    if (_selectedIndex == 0)
                        OptionFirst();
                    else
                        OptionSecond();
                    break;
            }
        }
        #endregion

        #region 选项操作
        protected virtual void OptionFirst()
        {

        }

        protected virtual void OptionSecond()
        {

        }
        #endregion
    }
}

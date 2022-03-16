using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace 俄罗斯方块
{
    class MainScene : IScene
    {
        private bool _active;

        private Thread _inputCheckThread;

        private int _maxCount;
        private List<HashSet<int>> _bricks = new List<HashSet<int>>();

        private BaseSquare _curSquare;

        public void Draw()
        {
            Console.Clear();

            // 墙体绘制
            Console.ForegroundColor = ConsoleColor.Red;
            for (int x = 0, y = Game.Window_Height - 1; x < Game.Window_Width; x += 2)
            {
                Game.instance.DrawUnit(x, y, "■");
            }
            for (int x = 0, x2 = Game.Window_Width - 2, y = 0; y < Game.Window_Height; ++y)
            {
                Game.instance.DrawUnit(x, y, "■");
                Game.instance.DrawUnit(x2, y, "■");
            }

            // 砖块绘制
            Console.ForegroundColor = ConsoleColor.White;
            for (int y = 0; y < _bricks.Count; ++y)
            {
                foreach (int x in _bricks[y])
                {
                    Game.instance.DrawUnit(x, y, "■");
                }
            }

            // 方块绘制
            _curSquare.Draw();
        }

        public void Hidden()
        {
            _active = false;
            _bricks.Clear();
            _inputCheckThread = null;

            Console.Clear();
        }

        public void Show()
        {
            Console.Clear();

            _active = true;
            Game.instance.score = 0;

            _maxCount = (Game.Window_Width >> 1) - 2;

            _bricks.Clear();
            for (int i = Game.Window_Height - 2; i >= 0; --i)
            {
                _bricks.Add(new HashSet<int>());
            }

            GenerateSquare();

            _inputCheckThread = new Thread(InputCheck);
            _inputCheckThread.IsBackground = true;
            _inputCheckThread.Start();
        }

        public void Update()
        {
            lock (_curSquare)
            {
                _curSquare.Update(this);
                // 检测方块是否变成砖块
                BrickCheck();
                ScoreCheck();
                Draw();
            }
        }

        private void GenerateSquare()
        {
            _curSquare = GameObjUtil.RandSquare(Game.Window_Width / 2, 0);

            _curSquare.LimitCheck(this);
            if (!_curSquare.CanMove(E_Limit.All))
            {
                Game.instance.scene = E_Scene.Result;
            }
        }

        public void BrickCheck()
        {
            if (_curSquare.CanMove(E_Limit.Down))
            {
                return;
            }
            // 当前方块无法继续下落则变成砖块
            foreach (Vector pos in _curSquare)
            {
                if (_bricks.Count > pos.y && pos.y > 0)
                {
                    _bricks[pos.y].Add(pos.x);
                }
            }

            GenerateSquare();
        }

        public int CheckLimit(Vector pos)
        {
            int res = 0;

            if (pos.y < 0)
            {
                return res;
            }

            if (pos.x <= 2 || pos.y >= _bricks.Count || _bricks[pos.y].Contains(pos.x - 2))
            {
                res |= (int)E_Limit.Left;
            }

            if (pos.x + 4 >= Game.Window_Width || pos.y >= _bricks.Count || _bricks[pos.y].Contains(pos.x + 2))
            {
                res |= (int)E_Limit.Right;
            }

            if (_bricks.Count <= pos.y + 1 || _bricks[pos.y + 1].Contains(pos.x))
            {
                res |= (int)E_Limit.Down;
            }
            return res;
        }

        public bool HasBrick(Vector pos)
        {
            if (pos.x < 2 || pos.x + 4 > Game.Window_Width || pos.y >= _bricks.Count)
            {
                return true;
            }

            return pos.y >= 0 && _bricks[pos.y].Contains(pos.x);
        }

        private void ScoreCheck()
        {
            for (int y = _bricks.Count - 1; y >= 0; --y)
            {
                while (_bricks[y].Count == _maxCount)
                {
                    for (int y2 = 0; y2 < y; ++y2)
                    {
                        HashSet<int> tmp = _bricks[y2];
                        _bricks[y2] = _bricks[y];
                        _bricks[y] = tmp;
                    }

                    _bricks[0].Clear();
                    ++Game.instance.score;
                }
            }
        }

        #region 输入检测
        private void InputCheck()
        {
            while (_active)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        lock (_curSquare)
                        {
                            _curSquare.MoveToLeft();
                        }
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        lock (_curSquare)
                        {
                            _curSquare.MoveToRight();
                        }
                        break;
                    case ConsoleKey.W:
                    case ConsoleKey.S:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.J:
                    case ConsoleKey.Enter:
                        lock (_curSquare)
                        {
                            _curSquare.LimitCheck(this);
                            _curSquare.TakeTrans();
                            _curSquare.LimitCheck(this);
                            BrickCheck();
                        }
                        break;
                }
            }
        }
        #endregion
    }
}

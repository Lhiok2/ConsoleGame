using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace 俄罗斯方块
{
    class MainScene : IScene
    {
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
            _curSquare?.Draw();
        }

        public void Hidden()
        {
            InputThread.instance.action -= InputCheck;

            _curSquare = null;
            _bricks.Clear();
            Console.Clear();
        }

        public void Show()
        {
            _curSquare = null;
            Game.instance.score = 0;
            _maxCount = (Game.Window_Width >> 1) - 2;

            _bricks.Clear();
            for (int i = Game.Window_Height - 2; i >= 0; --i)
            {
                _bricks.Add(new HashSet<int>());
            }

            GenerateSquare();
            Draw();

            InputThread.instance.action += InputCheck;
        }

        public void Update()
        {
            lock (_curSquare)
            {
                _curSquare.Update(this);
            }
        }

        #region 得分检测 检测是否存在可消除的行
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
                    Draw();
                }
            }
        }
        #endregion

        #region 生成新方块 老方块变成砖块
        public void GenerateSquare()
        {
            // 原方块不为空则变成砖块
            if (_curSquare != null)
            {
                foreach (Vector pos in _curSquare)
                {
                    if (_bricks.Count > pos.y && pos.y > 0)
                    {
                        _bricks[pos.y].Add(pos.x);
                    }
                }
            }

            _curSquare = GameObjUtil.RandSquare(Game.Window_Width / 2, 0);
            Draw();

            // 方块消除
            ScoreCheck();

            // 新生成的方块进行一次检测 如果与砖块重合则判定为游戏结束
            foreach (Vector pos in _curSquare)
            {
                if (HasBrick(pos))
                {
                    Game.instance.scene = E_Scene.Result;
                    return;
                }
            }
        }
        #endregion

        #region 移动限制检测
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
        #endregion

        #region 检测某个位置是否有方块 除上方越界外其它越界均判定为true
        public bool HasBrick(Vector pos)
        {
            if (pos.x < 2 || pos.x + 4 > Game.Window_Width || pos.y >= _bricks.Count)
            {
                return true;
            }

            return pos.y >= 0 && _bricks[pos.y].Contains(pos.x);
        }
        #endregion

        #region 输入检测
        private void InputCheck()
        {
            if (!Console.KeyAvailable)
            {
                return;
            }

            lock (_curSquare)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        _curSquare.MoveLeft(this);
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        _curSquare.MoveRight(this);
                        break;
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.J:
                    case ConsoleKey.Enter:
                        _curSquare.TakeTrans(this);
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        _curSquare.MoveDown(this);
                        break;
                }

            }
        }
        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

namespace 俄罗斯方块
{
    class BaseSquare : IEnumerable
    {
        private Vector _pos;

        private int _transIndex;
        protected List<List<Vector>> units;

        private ConsoleColor _color;

        private int _moveLimit;
        private bool _taketTransLimit;

        private int _xMove;

        public BaseSquare(int x, int y, ConsoleColor color)
        {
            _xMove = 0;
            _transIndex = 0;
            this._color = color;
            _pos = new Vector(x, y);
        }

        public void Update(MainScene scene)
        {
            // 再次判断 避免变化造成不可移动
            if ((_xMove < 0 && CanMove(E_Limit.Left))
                || (_xMove > 0 && CanMove(E_Limit.Right)))
            {
                _pos.x += _xMove;
            }

            ++_pos.y;
            _xMove = 0;

            LimitCheck(scene);
        }

        public void LimitCheck(MainScene scene)
        {
            _moveLimit = 0;
            _taketTransLimit = false;
            foreach (Vector v in units[_transIndex])
            {
                _moveLimit |= scene.CheckLimit(_pos + v);
            }

            foreach (Vector v in units[(_transIndex + 1) % units.Count])
            {
                if (scene.HasBrick(_pos + v))
                {
                    _taketTransLimit = true;
                    return;
                }
            }
        }

        public bool CanTakeTrans() => !_taketTransLimit;

        public bool CanMove(E_Limit limit)
        {
            return (_moveLimit & (int) limit) == 0;
        }

        public void MoveToLeft()
        {
            if ((_moveLimit & (int) E_Limit.Left) == 0)
            {
                _xMove = -2;
            }
        }

        public void MoveToRight()
        {
            if ((_moveLimit & (int) E_Limit.Right) == 0)
            {
                _xMove = 2;
            }
        }

        public void Draw()
        {
            Console.ForegroundColor = _color;

            foreach (Vector v in units[_transIndex])
            {
                Game.instance.DrawUnit(v.x + _pos.x, v.y + _pos.y, "■");
            }
        }

        public void TakeTrans()
        {
            if (!_taketTransLimit)
            {
                _transIndex = (_transIndex + 1) % units.Count;
                _taketTransLimit = true;
            }
        }

        protected void InitTrans()
        {
            _transIndex = GameObjUtil.RandInt(units.Count);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (Vector v in units[_transIndex])
            {
                yield return _pos + v;
            };
        }
    }
}

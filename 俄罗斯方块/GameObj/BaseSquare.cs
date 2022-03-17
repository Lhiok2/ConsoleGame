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

        public BaseSquare(int x, int y, ConsoleColor color)
        {
            _transIndex = 0;
            this._color = color;
            _pos = new Vector(x, y);
        }

        public void Update(MainScene scene)
        {
            MoveDown(scene);
        }

        private void LimitCheck(MainScene scene)
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

        public bool CanMove(E_Limit limit)
        {
            return (_moveLimit & (int) limit) == 0;
        }

        public void MoveDown(MainScene scene)
        {
            LimitCheck(scene);
            
            if (CanMove(E_Limit.Down))
            {
                Clear();
                ++_pos.y;
                Draw();
                return;
            }

            scene.GenerateSquare();
        }

        public void MoveLeft(MainScene scene)
        {
            LimitCheck(scene);

            if (CanMove(E_Limit.Left))
            {
                Clear();
                _pos.x -= 2;
                Draw();
            }
        }

        public void MoveRight(MainScene scene)
        {
            LimitCheck(scene);

            if (CanMove(E_Limit.Right))
            {
                Clear();
                _pos.x += 2;
                Draw();
            }
        }

        public void TakeTrans(MainScene scene)
        {
            LimitCheck(scene);

            if (!_taketTransLimit)
            {
                Clear();
                _transIndex = (_transIndex + 1) % units.Count;
                Draw();
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

        public void Clear()
        {
            foreach (Vector v in units[_transIndex])
            {
                Game.instance.DrawUnit(v.x + _pos.x, v.y + _pos.y, "  ");
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

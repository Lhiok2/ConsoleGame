using System;
using System.Collections.Generic;

namespace 飞行棋
{
    class MainSceneData
    {
        private int Width = 10;
        private int startPosX => Game.Console_Width / 2 - Width;
        private int startPosY = 3;

        private E_UnitType[] map = new E_UnitType[100];
        private E_UnitType[] baseMap = new E_UnitType[100];

        private int playerIndex1;
        private int playerIndex2;

        private bool canMovePlayer1 = true;
        private bool canMovePlayer2 = true;

        private Random random = new Random();

        private string Wall = "■";

        private Dictionary<E_UnitType, Unit> dict = new Dictionary<E_UnitType, Unit>()
        {
            { E_UnitType.Normal, new Unit("□", "普通格子", ConsoleColor.White) },
            { E_UnitType.Stop, new Unit("‖", "暂停一回合", ConsoleColor.Blue) },
            { E_UnitType.Swap, new Unit("∏", "交换位置", ConsoleColor.Cyan) },
            { E_UnitType.Trap, new Unit("○", "后退5格", ConsoleColor.Red) },
            { E_UnitType.Player_1, new Unit("※", "玩家1", ConsoleColor.Green) },
            { E_UnitType.Player_2, new Unit("※", "玩家2", ConsoleColor.Magenta) },
            { E_UnitType.Overlap, new Unit("※", "玩家重合", ConsoleColor.Yellow) },
        };

        private int TipWallPosY
        {
            get
            {
                int y;
                GetPos(map.Length - 1, out int x, out y);
                return y += startPosY;
            }
        }

        private int OperateWallPosY
        {
            get
            {
                return TipWallPosY + dict.Count + 3;
            }
        }

        private int nextPosY;

        private void DrawOperate(string operate)
        { 

            Console.SetCursorPosition(5, nextPosY++);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(operate);
        }

        private void ClearOperate()
        {
            nextPosY = OperateWallPosY + 2;

            for (int i = nextPosY; i < Game.Console_Height - 1; ++i)
            {
                for (int j = 5; j < Game.Console_Width - 2; ++j)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(' ');
                }
            }
        }

        public bool PlayerMove(E_UnitType player)
        {
            ClearOperate();
            DrawOperate($"{dict[player].describe}开始行动：");

            if (player == E_UnitType.Player_1)
            {
                return PlayerMove(E_UnitType.Player_1, ref playerIndex1, ref canMovePlayer1);
            }
            else if (player == E_UnitType.Player_2)
            {
                return PlayerMove(E_UnitType.Player_2, ref playerIndex2, ref canMovePlayer2);
            }
            else
            {
                DrawOperate($"{dict[player].describe}无法行动!!!");
                return false;
            }
        }

        private bool PlayerMove(E_UnitType player, ref int index, ref bool canMove)
        {
            if (!canMove)
            {
                DrawOperate($"{dict[player].describe}无法行动!!!");
                canMove = true;
                return false;
            }

            Leave(player, index);

            int step = random.Next(1, 7);
            DrawOperate($"{dict[player].describe}掷出{step}点!!!");
            DrawOperate($"{dict[player].describe}前进{Math.Min(step, map.Length - 1 - index)}步!!!");

            index = Math.Min(map.Length - 1, index + step);

            Reach(player, ref index, ref canMove);

            if (index == map.Length - 1)
            {
                DrawOperate($"{dict[player].describe}到达终点!!!");
                return true;
            }

            return false;
        }

        private void Leave(E_UnitType player, int index)
        {
            if (map[index] == E_UnitType.Overlap)
            {
                map[index] = player == E_UnitType.Player_1? E_UnitType.Player_2: E_UnitType.Player_1;
            }
            else
            {
                map[index] = baseMap[index];
            }
            DrawGrid(index);
        }

        private void Reach(E_UnitType player, ref int index, ref bool canMove)
        {
            switch (baseMap[index])
            {
                case E_UnitType.Stop:
                    DrawOperate($"{dict[player].describe}暂停一回合!!!");
                    canMove = false;
                    UpdateGrid(index, player);
                    break;
                case E_UnitType.Swap:
                    DrawOperate($"两位玩家互换位置!!!");
                    // 当前格子绘制
                    map[index] = player == E_UnitType.Player_1 ? E_UnitType.Player_2 : E_UnitType.Player_1;
                    DrawGrid(index);
                    // 坐标交换
                    playerIndex1 ^= playerIndex2;
                    playerIndex2 ^= playerIndex1;
                    playerIndex1 ^= playerIndex2;
                    // 被交换方位置清空
                    Leave(player ==E_UnitType.Player_1 ? E_UnitType.Player_2: E_UnitType.Player_1, index);
                    // 到达对方位置 可能触发事件
                    Reach(player, ref index, ref canMove);
                    break;
                case E_UnitType.Trap:
                    DrawOperate($"{dict[player].describe}后退5步!!!");
                    index -= 5;
                    Reach(player, ref index, ref canMove);
                    break;
                case E_UnitType.Normal:
                    UpdateGrid(index, player);
                    break;
            }
        }

        private void UpdateGrid(int index, E_UnitType player)
        {
            if (map[index] == E_UnitType.Player_1 || map[index] == E_UnitType.Player_2)
            {
                map[index] = E_UnitType.Overlap;
            }
            else
            {
                map[index] = player;
            }

            DrawGrid(index);
        }

        public void Init()
        {
            for (int i = 1; i < map.Length; ++i)
            {
                int num = random.Next(0, 100);
                if (num >= 96)
                {
                    map[i] = baseMap[i] = E_UnitType.Swap;
                }
                else if (num >= 90)
                {
                    map[i] = baseMap[i] = E_UnitType.Stop;
                }
                else if (num >= 85 && i >= 5)
                {
                    map[i] = baseMap[i] = E_UnitType.Trap;
                }
                else
                {
                    map[i] = baseMap[i] = E_UnitType.Normal;
                }
            }

            playerIndex1 = playerIndex2 = 0;
            canMovePlayer1 = canMovePlayer2 = true;
            baseMap[0] = baseMap[map.Length-1] = map[map.Length-1] = E_UnitType.Normal;
            map[0] = E_UnitType.Overlap;

            Draw();
        }

        private void Draw()
        {
            Console.Clear();

            // 墙
            Console.ForegroundColor = ConsoleColor.Red;

            int wallPosY = TipWallPosY;
            int wallPosY2 = OperateWallPosY;

            for (int i = 0; i < Game.Console_Width; i += 2)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write(Wall);
                Console.SetCursorPosition(i, wallPosY);
                Console.Write(Wall);
                Console.SetCursorPosition(i, wallPosY2);
                Console.Write(Wall);
                Console.SetCursorPosition(i, Game.Console_Height - 1);
                Console.Write(Wall);
            }
            for (int i = 0; i < Game.Console_Height; ++i)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(Wall);
                Console.SetCursorPosition(Game.Console_Width - 2, i);
                Console.Write(Wall);
            }

            // 提示
            wallPosY += 2;
            foreach (var val in dict.Values)
            {
                Console.SetCursorPosition(5, wallPosY++);
                Console.ForegroundColor = val.color;
                Console.Write($"{val.tag}: {val.describe}");
            }

            // 地图
            for (int i = 0; i < map.Length; ++i)
            {
                DrawGrid(i);
            }
        }
        private void DrawGrid(int index)
        {
            int x, y;

            GetPos(index, out x, out y);
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = dict[map[index]].color;
            Console.Write(dict[map[index]].tag);
        }

        #region 下标转坐标
        public void GetPos(int index, out int x, out int y)
        {
            if (index == 0)
            {
                x = startPosX;
                y = startPosY;
                return;
            }

            --index;
            x = index % (Width + 1) + 1;
            y = index / (Width + 1);

            if ((y & 1) == 0)
            {
                y *= 2;
                if (x > Width)
                {
                    ++y;
                    x = Width;
                }
            }
            else
            {
                y *= 2;
                x = Width - x + 1;
                if (x == 0)
                {
                    ++y;
                    x = 1;
                }
            }

            x = x * 2 + startPosX;
            y += startPosY;
        }
        #endregion
    }

    class MainScene : IScene
    {
        private MainSceneData data = new MainSceneData();

        protected override void Init()
        {
            Console.Clear();
        }

        protected override E_Scene Run()
        {
            data.Init();
            while (true)
            {
                ConsoleKey key = Input;
                if (data.PlayerMove(E_UnitType.Player_1))
                {
                    return E_Scene.Result;
                }
                key = Input;
                if (data.PlayerMove(E_UnitType.Player_2))
                {
                    return E_Scene.Result;
                }
            }
        }
    }
}

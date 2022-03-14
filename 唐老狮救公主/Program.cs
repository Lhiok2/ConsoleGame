using System;

namespace 唐老狮救公主
{
    class Program
    {

        static void Main(string[] args)
        {
            Game_2 game = new Game_2();
            game.Start();
        }
    }

    /// <summary>
    /// 通过WASD控制黄色方块移动
    /// </summary>
    class Game
    {
        private const int MAX_X = 80;
        private const int MAX_Y = 40;

        private int x;
        private int y;

        public void Start()
        {
            Init();
            while (true)
            {
                Draw();
                Read();
            }
        }

        #region 初始化
        private void Init()
        {
            // 设置尺寸
            Console.SetWindowSize(MAX_X, MAX_Y + 1);
            Console.SetBufferSize(MAX_X, MAX_Y + 1);
            // 隐藏光标
            Console.CursorVisible = false;

            x = 0;
            y = 0;
        }
        #endregion

        #region 绘制
        private void Draw()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(x, y);
            Console.Write("  ");
        }
        #endregion

        #region 读入
        private void Read()
        {
            while (true)
            {
                char ch = Console.ReadKey(true).KeyChar;

                switch (Char.ToUpper(ch))
                {
                    case 'W':
                        if (y > 0)
                        {
                            --y;
                            return;
                        }
                        break;
                    case 'A':
                        if (x > 0)
                        {
                            x -= 2;
                            return;
                        }
                        break;
                    case 'S':
                        if (y + 1 < MAX_Y)
                        {
                            ++y;
                            return;
                        }
                        break;
                    case 'D':
                        if (x + 2 < MAX_X)
                        {
                            x += 2;
                            return;
                        }
                        break;
                    case 'Q':
                        Environment.Exit(0);
                        break;
                }
            }
        }
        #endregion

    }

    /// <summary>
    /// 唐老狮救公主
    /// </summary>
    class Game_2
    {
        #region 全局常量/变量
        private const int MAX_X = 80;
        private const int MAX_Y = 40;

        private const string STAGE_MENU = "Menu";
        private const string STAGE_BATTLE = "Battle";
        private const string STAGE_WIN = "Win";
        private const string STAGE_FAIL = "Fail";
        private const string STAGE_Exit = "Exit";

        private char ch;
        private bool wPlay;

        private Random random = new Random();
        #endregion

        #region 开始菜单常量/变量
        private const string TEXT_NAME = "唐老狮救公主";
        private const string TEXT_START = "开始游戏";
        private const string TEXT_EXIT = "退出游戏";

        private const int POS_NAME_X = 34;
        private const int POS_NAME_Y = 10;

        private const int POS_START_X = 36;
        private const int POS_START_Y = 15;

        private const int POS_EXIT_X = 36;
        private const int POS_EXIT_Y = 18;
        #endregion

        #region 战斗常量/变量
        private const int MIN_HP = 80;
        private const int MAX_HP = 100;

        private const int MIN_ATTACK = 5;
        private const int MAX_ATTACK = 10;

        private const int POS_MESS_Y = 30;

        private const string TEXT_EDGE = "■";
        private const string TEXT_PLAYER = "●";
        private const string TEXT_BOSS = "■";
        private const string TEXT_TIP = "开始和Boss战斗";
        private const string TEXT_TIP_HP = "你当前的血量为: {0}";
        private const string TEXT_TIP_BOSS_HP = "Boss当前的血量为: {0}";
        private const string TEXT_ATTACK = "你对Boss造成{0}点伤害,Boss剩余血量为{1}";
        private const string TEXT_BEATTACK = "Boss对你造成{0}点伤害,你剩余血量为{1}";

        private int x;
        private int y;
        private int hp;

        private int boss_x;
        private int boss_y;
        private int boss_hp;
        #endregion

        #region 结算常量/变量
        private const string TEXT_WIN = "营救成功";
        private const string TEXT_FAIL = "营救失败";
        private const string TEXT_RESTART = "再玩一次";

        private const int POS_WIN_X = 36;
        private const int POS_WIN_Y = 10;

        private const int POS_FAIL_X = 36;
        private const int POS_FAIL_Y = 10;

        private const int POS_RESTART_X = 36;
        private const int POS_RESTART_Y = 15;
        #endregion

        #region 游戏入口
        public void Start()
        {
            Init();

            string stage = STAGE_MENU;

            while (true)
            {
                switch (stage)
                {
                    case STAGE_MENU:
                        stage = Menu();
                        break;
                    case STAGE_BATTLE:
                        stage = Battle();
                        break;
                    case STAGE_WIN:
                        stage = Result(true);
                        break;
                    case STAGE_FAIL:
                        stage = Result(false);
                        break;
                    case STAGE_Exit:
                        Exit();
                        break;
                }
            }
        }

        private void Init()
        {
            Console.SetWindowSize(MAX_X, MAX_Y + 1);
            Console.SetBufferSize(MAX_X, MAX_Y + 1);
            Console.CursorVisible = false;
        }

        private void Read()
        {
            ch = Console.ReadKey(true).KeyChar;
        }
        #endregion

        #region 菜单
        private string Menu()
        {
            InitMenu();

            while (true)
            {
                DrawMenu();
                Read();

                switch (Char.ToUpper(ch))
                {
                    case 'W':
                        wPlay = true;
                        break;
                    case 'S':
                        wPlay = false;
                        break;
                    case 'J':
                        return wPlay ? STAGE_BATTLE : STAGE_Exit;
                }
            }
        }

        private void InitMenu()
        {
            wPlay = true;

            Console.Clear();

            // 游戏名
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(POS_NAME_X, POS_NAME_Y);
            Console.Write(TEXT_NAME);
        }

        private void DrawMenu()
        {
            // 开始
            Console.ForegroundColor = wPlay ? ConsoleColor.Red : ConsoleColor.White;
            Console.SetCursorPosition(POS_START_X, POS_START_Y);
            Console.Write(TEXT_START);

            // 退出
            Console.ForegroundColor = wPlay ? ConsoleColor.White : ConsoleColor.Red;
            Console.SetCursorPosition(POS_EXIT_X, POS_EXIT_Y);
            Console.Write(TEXT_EXIT);
        }
        #endregion

        #region 战斗
        private string Battle()
        {
            InitBattle();

            while (true)
            {
                Read();

                switch (Char.ToUpper(ch))
                {
                    case 'W':
                        if (y > 1 && (x != boss_x || boss_y + 1 != y))
                        {
                            ClearPlayer();
                            --y;
                            DrawPlayer();
                        }
                        break;
                    case 'A':
                        if (x > 2 && (y != boss_y || boss_x + 2 != x))
                        {
                            ClearPlayer();
                            x -= 2;
                            DrawPlayer();
                        }
                        break;
                    case 'S':
                        if (y + 2 < POS_MESS_Y && (x != boss_x || y + 1 != boss_y))
                        {
                            ClearPlayer();
                            ++y;
                            DrawPlayer();
                        }
                        break;
                    case 'D':
                        if (x + 4 < MAX_X && (y != boss_y || x + 2 != boss_x))
                        {
                            ClearPlayer();
                            x += 2;
                            DrawPlayer();
                        }
                        break;
                    case 'J':
                        if ((x == boss_x && Math.Abs(y - boss_y) == 1) || (y == boss_y && Math.Abs(x - boss_x) == 2))
                        {
                            DoAttack();
                        }
                        break;
                }

                if (hp == 0)
                {
                    return STAGE_WIN;
                }

                if (boss_hp == 0)
                {
                    return STAGE_FAIL;
                }
            }
        }

        private void DoAttack()
        {
            int attackVal = random.Next(MIN_ATTACK, MAX_ATTACK + 1);
            int beAttackVal = random.Next(MIN_ATTACK, MAX_ATTACK + 1);

            hp = Math.Max(0, hp - beAttackVal);
            boss_hp = Math.Max(0, boss_hp - attackVal);

            // 清空消息栏
            for (int i = 2; i + 2 < MAX_X; ++i)
            {
                Console.SetCursorPosition(i, POS_MESS_Y + 1);
                Console.Write(" ");
                Console.SetCursorPosition(i, POS_MESS_Y + 2);
                Console.Write(" ");
            }

            // 输出新消息
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(2, POS_MESS_Y + 1);
            Console.Write(TEXT_ATTACK, attackVal, boss_hp);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(2, POS_MESS_Y + 2);
            Console.Write(TEXT_BEATTACK, beAttackVal, hp);
        }

        private void InitBattle()
        {
            Console.Clear();

            // 绘制边界
            Console.ForegroundColor = ConsoleColor.Red;

            for (int i = 0; i < MAX_X; i += 2)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write(TEXT_EDGE);
                Console.SetCursorPosition(i, POS_MESS_Y - 1);
                Console.Write(TEXT_EDGE);
                Console.SetCursorPosition(i, MAX_Y - 1);
                Console.Write(TEXT_EDGE);
            }
            for (int i = 0; i < MAX_Y; ++i)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(TEXT_EDGE);
                Console.SetCursorPosition(MAX_X - 2, i);
                Console.Write(TEXT_EDGE);
            }

            // 绘制提示
            hp = random.Next(MIN_HP, MAX_HP + 1);
            boss_hp = random.Next(MIN_HP, MAX_HP + 1);

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(2, POS_MESS_Y);
            Console.Write(TEXT_TIP);
            Console.SetCursorPosition(2, POS_MESS_Y + 1);
            Console.Write(TEXT_TIP_HP, hp);
            Console.SetCursorPosition(2, POS_MESS_Y + 2);
            Console.Write(TEXT_TIP_BOSS_HP, boss_hp);

            // 绘制角色
            x = 2;
            y = 1;
            DrawPlayer();

            boss_x = random.Next(4, MAX_X);
            boss_y = random.Next(1, POS_MESS_Y - 1);
            if ((boss_x & 1) == 1)
            {
                boss_x ^= 1;
            }
            DrawBoss();
        }

        private void ClearPlayer()
        {
            Console.SetCursorPosition(x, y);
            Console.Write("  ");
        }

        private void DrawPlayer()
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(TEXT_PLAYER);
        }
        private void DrawBoss()
        {
            Console.SetCursorPosition(boss_x, boss_y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(TEXT_BOSS);
        }
        #endregion

        #region 结算
        private string Result(bool bWin)
        {
            InitResult(bWin);

            while (true)
            {
                DrawResult();
                Read();

                switch (Char.ToUpper(ch))
                {
                    case 'W':
                        wPlay = true;
                        break;
                    case 'S':
                        wPlay = false;
                        break;
                    case 'J':
                        return wPlay ? STAGE_BATTLE : STAGE_Exit;
                }
            }
        }

        private void InitResult(bool bWin)
        {
            wPlay = true;

            Console.Clear();

            if (bWin)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(POS_WIN_X, POS_WIN_Y);
                Console.Write(TEXT_WIN);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(POS_FAIL_X, POS_FAIL_Y);
                Console.Write(TEXT_FAIL);
            }
        }

        private void DrawResult()
        {
            Console.ForegroundColor = wPlay ? ConsoleColor.Red : ConsoleColor.White;
            Console.SetCursorPosition(POS_RESTART_X, POS_RESTART_Y);
            Console.Write(TEXT_RESTART);

            Console.ForegroundColor = wPlay ? ConsoleColor.White : ConsoleColor.Red;
            Console.SetCursorPosition(POS_EXIT_X, POS_EXIT_Y);
            Console.Write(TEXT_EXIT);
        }
        #endregion

        #region 退出游戏
        private void Exit()
        {
            Environment.Exit(0);
        }
        #endregion
    }
}

namespace 飞行棋
{
    enum E_Scene
    {
        Start,
        Main,
        Result,
        Exit,
    }

    enum E_UnitType
    {
        Normal,     // 普通格子
        Stop,       // 时停
        Trap,       // 陷阱
        Swap,       // 交换位置
        Player_1,   // 玩家1
        Player_2,   // 玩家2
        Overlap,    // 重叠
    }
}

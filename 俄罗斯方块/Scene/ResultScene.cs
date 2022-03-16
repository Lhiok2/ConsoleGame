using System;
using System.Collections.Generic;
using System.Text;

namespace 俄罗斯方块
{
    class ResultScene : UIScene
    {

        public override void Show()
        {
            base.Show();

            titleName = $"你的分数为：{Game.instance.score}";
            optionNameFirst = "返回主菜单";
            optionNameSecond = "退出游戏";
        }

        protected override void OptionFirst()
        {
            Game.instance.scene = E_Scene.Start;
        }

        protected override void OptionSecond()
        {
            Game.instance.scene = E_Scene.None;
        }
    }
}

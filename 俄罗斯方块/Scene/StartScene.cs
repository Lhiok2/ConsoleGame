using System;
using System.Collections.Generic;
using System.Text;

namespace 俄罗斯方块
{
    class StartScene : UIScene
    {
        public override void Show()
        {
            titleName = "俄罗斯方块";
            optionNameFirst = "开始游戏";
            optionNameSecond = "退出游戏";

            base.Show();
        }

        protected override void OptionFirst()
        {
            Game.instance.scene = E_Scene.Main;
        }

        protected override void OptionSecond()
        {
            Game.instance.scene = E_Scene.None;
        }
    }
}

using System;

namespace 飞行棋
{
    class Program
    {
        static void Main(string[] args)
        {
            Game.Start();
        }
    }

    partial class C
    {
        partial void Speak();
    }

    partial class C
    {
        partial void Speak()
        {
            Console.WriteLine("wdnmd");
        }

        public void Speak(string str)
        {
            Speak();
            Console.WriteLine(str);
        }
    }
}

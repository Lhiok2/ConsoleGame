using System;
using System.Threading;

namespace 俄罗斯方块
{
    class InputThread : SingleTon<InputThread>
    {
        private Thread inputCheckThread;

        public event Action action;


        public InputThread()
        {
            inputCheckThread = new Thread(InputCheck);
            inputCheckThread.IsBackground = true;
            inputCheckThread.Start();
        }

        private void InputCheck()
        {
            while (true)
            {
                action?.Invoke();
            }
        }
    }
}

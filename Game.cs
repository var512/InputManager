using System;
using System.Threading;
using System.Threading.Tasks;

namespace InputManager
{
    public class Game
    {
        public static void GameCallback(object state)
        {
            Timers.gameTimer.Change(1000, Timeout.Infinite);
        }
    }
}

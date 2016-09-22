using System;
using System.Threading;

namespace InputManager
{
    public class Timers
    {
        public static Timer consoleMinimizeTimer;
        public static Timer gameTimer;

        public static void start()
        {
            // detects when the console window is minimized and hide it
            consoleMinimizeTimer = new Timer(WinTray.ConsoleMinimizeCallback, null, 500, Timeout.Infinite);

            // main game callback
            gameTimer = new Timer(Game.GameCallback, null, 1000, Timeout.Infinite);
        }
    }
}

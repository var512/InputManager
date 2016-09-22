using System;
using System.Globalization;
using System.Windows.Forms;

namespace InputManager
{
    internal class Program : WinTray
    {
        public static AbstractEvents events;
        private static Settings settings = new Settings();

        private static void Main(string[] args)
        {
            Init();
            events = new SetWindowsHookEvents();
            Timers.start();
            Run();
            events.OnExit();
        }
    }
}

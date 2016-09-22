using System;

namespace InputManager
{
    public class Log
    {
        public static void Message(string m)
        {
            if (!Settings.ShowLogs) return;

            Console.Write(DateTime.Now.ToString("[dd/MM/yyyy][HH:mm:ss]"));
            Console.Write("[" + Window.GetActiveWindowTitle() + "]");
            Console.Write(" " + m);

            Console.WriteLine();
        }
    }
}

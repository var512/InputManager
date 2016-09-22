using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace InputManager
{
    public class WinTray
    {
        protected struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public Rectangle rcNormalPosition;
        }

        protected enum ShowWindowCommands
        {
            Hide = 0,
            Normal = 1,
            ShowMinimized = 2,
            Maximize = 3,
            ShowMaximized = 3,
            ShowNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActive = 7,
            ShowNA = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimize = 11
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("kernel32.dll")]
        protected static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        protected static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        protected static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        [DllImport("user32.dll")]
        protected static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        protected const uint SC_CLOSE = 0xF060;
        protected const uint MF_ENABLED = 0x00000000;
        protected const uint MF_DISABLED = 0x00000002;

        protected static NotifyIcon Tray = default(NotifyIcon);

        protected static readonly IntPtr ConsoleWindowHandle = GetConsoleWindow();

        public static void Init()
        {
            Console.Title = "InputManager";
            Console.SetWindowSize(
                Math.Min(130, Console.LargestWindowWidth),
                Math.Min(40, Console.LargestWindowHeight)
            );

            // Disable Close Button (X)
            EnableMenuItem(GetSystemMenu(ConsoleWindowHandle, false), SC_CLOSE, (uint) (MF_ENABLED | MF_DISABLED));

            MenuItem mExit = new MenuItem("Exit", new EventHandler(TrayExit));
            ContextMenu Menu = new ContextMenu(new MenuItem[] { mExit });

            Tray = new NotifyIcon()
            {
                Icon = new Icon(SystemIcons.Application, 40, 40),
                Visible = true,
                Text = Console.Title,
                ContextMenu = Menu
            };

            Tray.Click += TrayClick;
        }

        public static void Run()
        {
            Application.Run();
        }

        public static void ConsoleMinimizeCallback(object state)
        {
            // Log.Message("ConsoleMinimizeCallback called");

            WINDOWPLACEMENT wPlacement = new WINDOWPLACEMENT();
            GetWindowPlacement(ConsoleWindowHandle, ref wPlacement);

            if (wPlacement.showCmd == (int) ShowWindowCommands.ShowMinimized)
            {
                ShowWindow(ConsoleWindowHandle, (int) ShowWindowCommands.Hide);
            }

            Timers.consoleMinimizeTimer.Change(500, Timeout.Infinite);
        }

        protected static void TrayClick(object sender, EventArgs e)
        {
            ShowWindow(ConsoleWindowHandle, (int) ShowWindowCommands.Restore);
        }

        protected static void TrayExit(object sender, EventArgs e)
        {
            Tray.Dispose();
            Application.Exit();
        }
    }
}

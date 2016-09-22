using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace InputManager
{
    public class Window
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }

            return null;
        }

        public static bool IsValidTarget()
        {
            try
            {
                string activeWindowTitle = GetActiveWindowTitle();

                foreach (string target in Settings.TargetWindows)
                {
                    if (activeWindowTitle.Contains(target))
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Message(e.Message);
            }

            return false;
        }

        public static Rectangle WinGetRect(IntPtr hWnd)
        {
            Rectangle myRect = new Rectangle();

            RECT rct;

            if (!GetWindowRect(new HandleRef(hWnd, hWnd), out rct))
            {
                throw new Exception("WinGetRectByHandle HandleRef error");
            }

            myRect.X = rct.Left;
            myRect.Y = rct.Top;

            myRect.Width = rct.Right - rct.Left;
            myRect.Height = rct.Bottom - rct.Top;

            return myRect;
        }

        public static IntPtr WinGetHandle(string wName)
        {
            var titles = Process.GetProcesses().Where(pList => pList.MainWindowTitle.ToLower().Contains(wName)).ToList();

            switch (titles.Count)
            {
                case 1:
                    return titles[0].MainWindowHandle;
                case 0:
                    throw new ApplicationException("No application was found");
                default:
                    throw new ApplicationException("More than one application was found");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
            {
            }

            public int X
            {
                get { return Left; }
                set
                {
                    Right -= (Left - value);
                    Left = value;
                }
            }

            public int Y
            {
                get { return Top; }
                set
                {
                    Bottom -= (Top - value);
                    Top = value;
                }
            }

            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }

            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }

            public Point Location
            {
                get { return new Point(Left, Top); }
                set
                {
                    X = value.X;
                    Y = value.Y;
                }
            }

            public Size Size
            {
                get { return new Size(Width, Height); }
                set
                {
                    Width = value.Width;
                    Height = value.Height;
                }
            }

            public static implicit operator Rectangle(RECT r)
            {
                return new Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                {
                    return Equals((RECT) obj);
                }
                else if (obj is Rectangle)
                {
                    return Equals(new RECT((Rectangle) obj));
                }

                return false;
            }

            public override int GetHashCode()
            {
                return ((Rectangle) this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture,
                    "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }
    }
}

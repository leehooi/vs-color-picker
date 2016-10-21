using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ColorPicker
{
    public sealed class WinApi
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect lpRect);
        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool GetGUIThreadInfo(uint hTreadID, ref GUITHREADINFO lpgui);

        [DllImport("user32.dll")]
        public static extern bool GetCaretPos(out Point lpPoint);

        [DllImport("user32.dll")]
        public static extern int GetDC(int hwnd);

        [DllImport("user32.dll")]
        public static extern int WindowFromPoint(int xPoint, int yPoint);

        [DllImport("user32.dll")]
        public static extern int ScreenToClient(int hwnd, ref Point lpPoint);

        [DllImport("gdi32")]
        public static extern int GetPixel(int hdc, int X, int y);

        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public struct GUITHREADINFO
        {
            public int cbSize;
            public int flags;
            public IntPtr hwndActive;
            public IntPtr hwndFocus;
            public IntPtr hwndCapture;
            public IntPtr hwndMenuOwner;
            public IntPtr hwndMoveSize;
            public IntPtr hwndCaret;
            public Rect rectCaret;
        }

        public struct Point
        {
            public int X;
            public int Y;
        }
    }
}

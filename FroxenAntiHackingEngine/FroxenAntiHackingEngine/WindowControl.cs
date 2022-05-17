using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FroxenAntiHackingEngine
{
    public class WindowControl
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWnd, IntPtr hParent);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetWindowLongA(IntPtr hWnd, int nIndex, long dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int Flags);

        public static void SetWindowParent(IntPtr WindowHandle, IntPtr NewParent)
        {
            SetWindowLongA(WindowHandle, -16, 0x02000000L);
            SetParent(WindowHandle, NewParent);
            ShowWindow(WindowHandle, 3);
        }
    }
}

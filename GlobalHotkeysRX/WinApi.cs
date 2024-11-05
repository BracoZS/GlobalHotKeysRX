using System;
using System.Runtime.InteropServices;

namespace GlobalHotkeysRX
{   
    public static class WinApi
    {
        /// <summary>
        /// Posted when the user presses a hot key registered by the RegisterHotKey function. 
        /// </summary>
        public const int WM_HOTKEY = 0x0312;

        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}

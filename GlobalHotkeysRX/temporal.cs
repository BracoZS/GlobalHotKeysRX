using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalHotkeysRX
{
    internal class temporal
    {
        public void dalePrueba()
        {
            Hotkey h = new Hotkey(Modifier.Alt, VKey.Q);
            GlobalHotkeys z = new GlobalHotkeys(IntPtr.Zero, h, h, h, h);
        }
    }
}

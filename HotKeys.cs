
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Input;

namespace GlobalHotKeysRX
{
    [Flags]
    public enum Modifiers
    {
        None = 0,
        Alt = 1,
        Control = 2,
        NoRepeat = 0x4000,
        Shift = 4,
        Win = 8
    }
    public struct Combo
    {
        public Modifiers modifier { get; set; }
        public Key key { get; set; }

        public Combo( Modifiers modifiers, Key key )
        {
            this.modifier = modifiers;
            this.key = key;
        }
    }

    public class HotKeys
    {
        #region Windows API
        const int WM_HOTKEY = 0x0312;

        [DllImport("user32.dll")]
        static extern bool RegisterHotKey( IntPtr hWnd, int id, uint fsModifiers, uint vk );
        [DllImport("user32.dll")]
        static extern bool UnregisterHotKey( IntPtr hWnd, int id );
        #endregion

        private Dictionary<int, Action> _methods;
        public HotKeys()
        {
            ComponentDispatcher.ThreadFilterMessage += new ThreadMessageEventHandler(OnThreadFilterMessage);
            _methods = new Dictionary<int, Action>();
        }
        public HotKeys(Combo c, Action a):this()     // - :this() - !important
        {
            AddMethod(c, a);
        }
        private int getID(Combo c)
        {
            return (int)c.modifier + (int)c.key;     
        }
        public bool AddMethod(Combo combo, Action action)       //implement exception...
        {
            var id = getID(combo);
            var vKeyCode = KeyInterop.VirtualKeyFromKey(combo.key);
            bool okRegister = RegisterHotKey(IntPtr.Zero, id, (uint)combo.modifier, (uint)vKeyCode);

            if (okRegister) {_methods.Add(id, action);};
            return okRegister;
        }
        public bool DeleteMethod(Combo c)
        {
            bool okDelete = false;     
            var id = getID(c);
            if (_methods.ContainsKey(id))
            {
                okDelete = UnregisterHotKey(IntPtr.Zero, id);
                _methods.Remove(id);
            }
            return okDelete;    
        }
        public void ClearAll()
        {
            foreach (var item in _methods)
            {
                UnregisterHotKey(IntPtr.Zero, item.Key);
            }
            _methods.Clear();
        }
        private void OnThreadFilterMessage(ref MSG msg, ref bool handled)
        {
            if (!handled && msg.message == WM_HOTKEY)
            {
                var id = msg.wParam.ToInt32();
                _methods[id]?.Invoke();     

                handled = true;
            }
        }

        #region Dispose
        bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose( bool disposing )
        {
            if(!_disposed)
            {
                if(disposing)
                {
                    ClearAll();
                }

                _disposed = true;
            }
        }
        #endregion
    }
    
}

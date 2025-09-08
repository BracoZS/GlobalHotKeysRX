using System;
using System.Collections.Generic;
using System.Windows.Interop;
using static ApplicationTypeChecker;
using static GlobalHotkeysRX.WinApi;

namespace GlobalHotkeysRX
{
    public class GlobalHotkeys
    {
        private readonly Dictionary<int, Hotkey> _shortcutCatalog = new Dictionary<int, Hotkey>();

        public delegate void HotkeyPressedEventHandler(object sender, HotkeyEventArgs e);
        public event HotkeyPressedEventHandler HotkeyPressed;

        public IntPtr ClientHandle { get; set; }

        /// <summary>
        /// Accepts any number of parameters of type Hotkey.
        /// </summary>
        public GlobalHotkeys(IntPtr handle = default, params Hotkey[] hotKey) 
        {
            ClientHandle = handle;

            foreach(var htk in hotKey)
            {
                Add(htk);
            }

            if(IsWPF())
                ComponentDispatcher.ThreadFilterMessage += OnthreadFilterMessage;
        }
        public GlobalHotkeys(params Hotkey[] hotKeys) : this(default, hotKeys) { }

        public bool Add(Hotkey hotKey)
        {
            if(RegisterHotKey(ClientHandle, hotKey.ID, (uint)hotKey.Modifier, (uint)hotKey.Key))
            {
                _shortcutCatalog.Add(hotKey.ID, hotKey);

                return true;
            }

            return false;
        }
        /// <summary>
        /// <remarks>
        ///     <para><b>Note:</b> If you don't pass an Action as an argument, you should subscribe to the HotkeyPressed event to process it.</para>
        /// </remarks> 
        /// </summary>
        public bool Add(Modifier modifiers, VKey key, Action action = null) => Add(new Hotkey(modifiers, key, action));

        public bool Remove(Hotkey hotKey)
        {
            if(_shortcutCatalog.Remove(hotKey.ID) && UnregisterHotKey(ClientHandle, hotKey.ID))
            {
                return true;
            }

            return false;
        }
        public bool Remove(Modifier modifiers, VKey key)
        {
            return Remove(_ = new Hotkey(modifiers, key));
        }

        private void OnthreadFilterMessage(ref MSG msg, ref bool handled)
        {
            if(!handled && msg.message == WM_HOTKEY)
            {
                var hotkeyId = msg.wParam.ToInt32();

                if(_shortcutCatalog.TryGetValue(hotkeyId, out Hotkey htk))
                {
                    htk?.Action?.Invoke();
                    HotkeyPressed?.Invoke(this, new HotkeyEventArgs(htk));
                }

                handled = true;
            }
        }

        /// <summary>
        /// <remarks>
        ///     <para><b>Note:</b>This function has been specifically created for Windows Forms environments.</para>
        /// </remarks> 
        /// </summary>  
        public void OnWinFormMessage(object msg)
        {
            var wParam = msg.GetType().GetProperty("WParam").GetValue(msg, null);       // WParam

            if(wParam is IntPtr hotkeyId)
            {
                if(_shortcutCatalog.TryGetValue((int)hotkeyId, out Hotkey htk))
                {
                    htk?.Action?.Invoke();
                    HotkeyPressed?.Invoke(this, new HotkeyEventArgs(htk));
                }
            }
        }

        #region IDisposable implementation
        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
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
        private void ClearAll()
        {
            foreach(var shortcut in _shortcutCatalog)
            {
                _ = _shortcutCatalog.Remove(shortcut.Key) && UnregisterHotKey(IntPtr.Zero, shortcut.Key);
            }
        }
        #endregion
    }
    public class HotkeyEventArgs : EventArgs
    {
        private Hotkey Hotkey { get; }
        public Modifier Modifier { get { return Hotkey.Modifier; } }
        public VKey Key { get { return Hotkey.Key; } }
        public int ID { get { return Hotkey.ID; } }
        public HotkeyEventArgs(Hotkey msg)
        {
            Hotkey = msg;
        }
    }
}

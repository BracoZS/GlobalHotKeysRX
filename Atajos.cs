
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

        public Combo( Modifiers modificador, Key tecla )
        {
            this.modifier = modificador;
            this.key = tecla;
        }
        public Combo( Modifiers modificador, Modifiers modificador2, Key tecla )
        {
            this.modifier = modificador | modificador2;
            this.key = tecla;
        }
    }

    public class Atajos
    {
        #region Windows API
        const int WM_HOTKEY = 0x0312;

        [DllImport("user32.dll")]
        static extern bool RegisterHotKey( IntPtr hWnd, int id, uint fsModifiers, uint vk );
        [DllImport("user32.dll")]
        static extern bool UnregisterHotKey( IntPtr hWnd, int id );
        #endregion

        private Dictionary<int, Action> _metodos;
        public Atajos()
        {
            ComponentDispatcher.ThreadFilterMessage += new ThreadMessageEventHandler(OnThreadFilterMessage);
            _metodos = new Dictionary<int, Action>();
        }
        public Atajos( Combo c, Action a ):this()     // - :this() - !important
        {
            AñadirMetodo(c, a);
        }
        private int getID( Combo c )
        {
            return (int)c.modifier + (int)c.key;     // + 12345?
        }
        public bool AñadirMetodo(Combo combo, Action action)       //implement exception by using this return
        {
            var id = getID(combo);
            var vKeyCode = KeyInterop.VirtualKeyFromKey(combo.key);
            bool okRegister = RegisterHotKey(IntPtr.Zero, id, (uint)combo.modifier, (uint)vKeyCode);

            if ( okRegister ) { _metodos.Add(id, action); };
            return okRegister;
        }
        public bool BorrarMetodo( Combo c )
        {
            bool okDelete = false;     
            var id = getID(c);
            if ( _metodos.ContainsKey(id) )
            {
                okDelete = UnregisterHotKey(IntPtr.Zero, id);
                _metodos.Remove(id);
            }
            return okDelete;    
        }
        public void BorrarTodoRegistro()
        {
            foreach ( var item in _metodos )
            {
                UnregisterHotKey(IntPtr.Zero, item.Key);
            }
            _metodos.Clear();
        }
        private void OnThreadFilterMessage( ref MSG msg, ref bool handled )
        {
            if ( !handled && msg.message == WM_HOTKEY )
            {
                var id = msg.wParam.ToInt32();
                _metodos[id]?.Invoke();     //cuidado con el null                    //:)!!!!!!

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
            if ( !_disposed )
            {
                if ( disposing )
                {
                    BorrarTodoRegistro();
                }

                _disposed = true;
            }
        }
        #endregion

    }


}

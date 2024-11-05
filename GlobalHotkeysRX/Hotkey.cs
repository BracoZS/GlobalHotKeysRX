using System;

namespace GlobalHotkeysRX
{
    public class Hotkey
    {
        public Modifier Modifier { get; set; }
        public VKey Key { get; set; }
        public Action Action { get; set; } = null;
        public int ID { get; private set; }
        public Hotkey(Modifier modifiers, VKey key, Action action = null)
        {
            this.Modifier = modifiers;
            this.Key = key;
            Action = action;

            // concatenates 2 numbers with the format num1909num2.
            // 909 in the middle is used as separator to avoid posible duplicates
            ID = int.Parse($"{(int)modifiers}909{(int)key}");        
        }
    }
}

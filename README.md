# GlobalHotKeysRX
A WPF C# library to create global keyboard shortcuts.
- :construction_worker::construction: WIP
## Sample usage

```
using GlobalHotKeysRX;
```

...initialization...
```
  Combo combo1 = new Combo(Modifiers.Alt, Key.C);

  Atajos hotkeys = new Atajos();
```
adding a hotkey
```
  hotkeys.AñadirMetodo(combo1, myfunction);     

  private void myfunction()
  {
     //code when hotkey occurs...
  }
```
removing
```
  hotkeys.BorrarMetodo(combo1);    //single
```
```
  hotkeys.BorrarTodoRegistro();    //all
```
simplest way
```
  Atajos hks = new Atajos(new Combo(Modifiers.Control, Key.Q), () => { /*code...*/ });
```

# GlobalHotKeysRX
A WPF C# library to create global keyboard shortcuts. It allows you to implement keyboard shortcuts globally in your applications. With this library, you can implement keyboard shortcuts to perform specific actions in your applications, regardless of the active window.
Works globally, allowing the activation of shortcuts on any window.

Supports combinations of:

-Modifier keys: Alt, Control, Shift, Win (Up to 3 at the same time).

-WPF Keys, listed at [System.Windows.Input  Key Enum](https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.key?view=windowsdesktop-8.0).
- :construction_worker::construction: WIP
## Sample usage
import
```
  using GlobalHotKeysRX;
```

initialization
```
  Combo combo1 = new Combo(Modifiers.Alt, Key.C);                          //dual combo
  Combo combo2 = new Combo(Modifiers.Alt | Modifiers.Control, Key.Z);      //triple combo

  Atajos hotKeys = new Atajos();
```
adding a hotey
```
  hotKeys.AñadirMetodo(combo1, myfunction);     

  void myfunction()
  {
     //code when hotkey occurs...
  }
```
removing
```
  hotKeys.BorrarMetodo(combo1);    //single
```
```
  hotKeys.BorrarTodoRegistro();    //all
```
simplest way
```
  Atajos hks = new Atajos(new Combo(Modifiers.Control, Key.Q), () => { /*code...*/ });
```
### Known issues

It does not work overwriting some known Windows shortcuts like 'alt + tab' or 'win + e'.

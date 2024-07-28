# GlobalHotKeysRX
A small C# WPF library to create global keyboard shortcuts. With this library, you can implement keyboard shortcuts to perform specific actions in your applications, regardless of the active window.
Works globally, allowing the activation of shortcuts on any window.

*No Window is required, so you can use it in background/windowless applications.

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
  Combo combo1 = new Combo(Modifiers.Alt, Key.C);                          //dual combo (alt + C)
  Combo combo2 = new Combo(Modifiers.Alt | Modifiers.Control, Key.Z);      //triple combo (Alt + Control + Z)

  HotKeys hks = new HotKeys();
```
> *Use the | operator to combine/add modifiers


adding a hotkey
```
  hks.AddMethod(combo1, myfunction);     

  void myfunction()
  {
     //code when a hotkey occurs...
  }
```
removing
```
  hks.DeleteMethod(combo1);    //single
```
```
  hks.ClearAll();    //all
```
cleaning up resources
```
  hks.Dispose();
```
simplest way
```
  HotKeys hks = new HotKeys(new Combo(Modifiers.Control, Key.Q), () => { /*code...*/ });
```
### Known issues

This library uses Win32 API. Typically, [RegisterHotKey](https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-registerhotkey) fails if the keystrokes specified for the hot key have already been registered for another hot key. However, some pre-existing, default hotkeys registered by the OS (such as PrintScreen, which launches the Snipping tool) may be overridden by another hot key registration when one of the app's windows is in the foreground.

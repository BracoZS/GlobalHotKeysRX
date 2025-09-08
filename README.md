# GlobalHotKeysRX

Simple C# library for creating system-wide hotkeys. It allows you to implement keyboard shortcuts to perform specific actions in your application, regardless of the active window.
You can use it in background/windowless applications.

It supports combinations of:

-Modifier keys: Alt, Control, Shift, Win (up to 3 at a time).

-Keys, listed at [Virtual-Key Codes](https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes).

## [Nuget Package](https://www.nuget.org/packages/GlobalHotkeysRX) <a href="https://www.nuget.org/packages/GlobalHotkeysRX"><img src="https://api.nuget.org/v3-flatcontainer/globalhotkeysrx/2.0.0/icon" width="32" height="32"/></a>

##  Usage

Import library
```
using GlobalHotKeysRX;
```

Initialization
```
// default constructor
GlobalHotkeys appHotkeys = new GlobalHotkeys();

// or you can call it with one or more 'Hotkey' parameters.
GlobalHotkeys appHotkeys = new GlobalHotkeys(
  new Hotkey(Modifier.Shift, VKey.P, myFunction),             
  new Hotkey(Modifier.Alt, VKey.S, () => {/* code */})   // lambda expression
  new Hotkey(Modifier.Control, VKey.Y)
);

```
> *Use the  `|`  operator to combine/add modifiers. For example, combine `Control` + `Alt` to get `AtlGr`
>
> `new Hotkey(Modifier.Control | Modifier.Alt, VKey.P, myFunction)  // AltGr + P`
> 
> *By using 'Modifiers.NoRepeat' with another modifier, the application will only receive another WM_HOTKEY message when the key is released and then pressed again while a modifier is held down.
>
> `new Hotkey(Modifier.Shift | Modifier.NoRepeat, VKey.P, myFunction)`
>

>
>### * If you are in a **Windows Form** proyect
>you need to specify the handle, either in constructor...
>
>`GlobalHotkeys appHotkeys = new GlobalHotkeys(this.Handle);` 
>
>...or through the ClientHandle property
>
>`appHotkeys.ClientHandle = this.Handle;`
>
>Also you must add this to the form
>
>```
> protected override void WndProc(ref Message m)
>{
>    if(m.Msg == WinApi.WM_HOTKEY)
>        appHotkeys.OnWinFormMessage(m);  // Replace by your own instance name
>
>    // (Optional) Stops any default or additional processing of the message.
>    m.Result = (IntPtr)1;
>
>    base.WndProc(ref m);
>}
>```


Adding a hotkey after initialization
```
appHotkeys.Add(Modifier.Win, VKey.NUM_0);

// or
var htk = new Hotkey(Modifier.Win, VKey.NUM_0, myFunction);
appHotkeys.Add(htk);   

void myFunction()
{
    // code when a hotkey occurs...
}
```
Removing
```
appHotkeys.Remove(Modifier.Shift, VKey.Z);

// or
appHotkeys.Remove(htk);
```
Subscribe to hotkeys
```
appHotkeys.HotkeyPressed += AppHotkeys_HotkeyPressed;
// ...
public void AppHotkeys_HotkeyPressed(object sender, HotkeyEventArgs e)
{
  MessageBox.Show($"Hotkey {e.Modifier} + {e.Key} was pressed!");
}
```
Cleaning up resources
```
appHotkeys.Dispose();
```
Simplest way 🙂 (wpf)
```
GlobalHotkeys ghk = new GlobalHotkeys(new Hotkey(Modifier.Shift, VKey.V, () => { /*code*/ }));
```

### Known issues

This library uses Win32 API. Typically, [RegisterHotKey](https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-registerhotkey) fails if the keystrokes specified for the hot key have already been registered for another hot key. However, some pre-existing, default hotkeys registered by the OS (such as PrintScreen, which launches the Snipping tool) may be overridden by another hot key registration when one of the app's windows is in the foreground.

[![Patreon](https://img.shields.io/badge/Support_on_patreon-ECEFF1?style=for-the-badge&logo=data%3Aimage%2Fpng%3Bbase64%2CiVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAACXBIWXMAAAsTAAALEwEAmpwYAAABxklEQVR4nO2WPWvbQBiAr6VEKNAxUEinbh2SmIZg%2BSOW7Uh3siXLli6aQ6Fbp06FLulUSqBD5uQfZEwyJ3RsofQnhCYdQu6kk9MO7ZA3KBDoUDfSnbfqgWcTvI%2Feg5MQKin5A%2F2ZacxUutGt%2BoL5GP0D4YZPUkLfjgn9lDr0PCXhr5TQ07FDD8Zk%2FQVEkY6KoC119rRKF27NIv72HETRjMD0g8A3A2GimJ4mhIZTDbjw%2FYfCDj6mOIQ8ChxeCSt4M5UA2Ny8L6xgP7UDKO5oQzkgsYLnwgpAztHlDzN6JB0ACN0T1vBEWCOQd7glHZBaA0OsDUFN%2FyR7EamApOO%2FEl0fVGVdd14qIG4PtpKOD6qKNW9FbgNt713SHoCyZr8iuQHvZdL2QNVx05mTCmCt%2FtPE9EDJlvd14vA89wBfdb%2FELRdk5S33NVIMIPFqH%2BTsnX1f9maVAjJ4o7cdN3tQRN7s%2FU7q%2FQ66Cy1HAJjmA9ZwdnjDgXySn6yO6Z3Di3yOM1jD3uB1fMbrBCZaw0dx1VlEedEKBGR8Mwyd1ex1XsO7rIaPuYE%2FMwMfMsN%2Bz6p2Nfdg2YCpo5UBS%2F%2F7EegFf8tLStAUuAZWoCwNWjXb3wAAAABJRU5ErkJggg%3D%3D&logoSize=auto)](https://www.patreon.com/Braco_ZS)
[![Ko-fi](https://img.shields.io/badge/Support_me_on_Ko--fi-29B6F6?style=for-the-badge&logo=data%3Aimage%2Fpng%3Bbase64%2CiVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAYAAABXAvmHAAAACXBIWXMAAAsTAAALEwEAmpwYAAAC90lEQVR4nO2YS0gUcRyAt6hTUdClSwQFnYIOBem6rm9dXxWYHoIOkolEUHQLO5nPOkTmoSJQjEgsekDH8pE9PBSEFVRQpmLszJblY7dVV%2FeLeSyuNfuYcWz3MB%2F82MP%2BZ%2Fi%2B4T%2B7s2uzWVhYWFhYWCQZQCXQB3xJwHwGXgL3gFPADr3y0kHJxCLQBeyKN2CE5GQGOBJLfh3JTRA4Fi1gPclPAHAmY8A3oEWdVuBFlLXvJVfDAfOfPjJz4zpTF1vwdrQTGPm69GYwyNzrV0y3tTLZ1ID3ZicLohDPaQc1fJxqmBYVhgK8nR14DpXiOViizIFi%2BdX34D4EAkw2NyIWFSAW5ivjypPXzD4b0B0gAewB5jTWP7TpDfD3PFmSl8RLi%2BQRSwrl%2BVV7dknelYdYkKtMfg6eYhcLY2O6AySAdo31bpvegImTJxRxVV4WL3YpE0FczMtGzM2SZ%2BryJaMBRyMcszHugKDfH1s8JB8SV%2BWFnEyE7AwmaqqNBpRHOGaDroDQVlkmHn7VJfGwqy6Lq%2FJClpOJ6iqjAY0a6%2F26t9CPqkpFPI7tEi4uT2Y6UxeadQcAmwGtj7HnugN83V1xb5dwcSHDgeBMY35oSFcAsAl4HGH9Od0Bwd8%2BvleURRcPyYfEVfmfZ04Tg7fAPnUKgFpgPMJaL7BVd4DE7MDTqPtcFg%2B76kK6XV4bGB3FROoi3SxxfRNPt12JuV0kccGhjL%2B3x0z5Xumhc0UBLC4yWV%2F373aRxUPyqbjTUvDevmWmfB%2BwRVNeV4CE9NjQcH7ZPg8Xdzvs%2BO50myXuB5o0H%2BAMB6j47nYrAY5UhLRU3PYUhJyseJ59YhFUb%2Bx6YHtU8ZUESMz29yPkZuO278dTXkZgeNjIad4AO9XZFvNqmxkgsTA%2Bzsy1qwR9PqOnGNQtbGaACQyaEbBW%2FcmWCPpWHKBGSDdOImgzK6AmAfIBYLdZAWsi%2FAJaTfnjpsj%2FFXIYeAR8WKW%2FEN8BncBe0%2BUtLCwsLCwsbP%2BHPz%2BHGuTTCs7IAAAAAElFTkSuQmCC&logoSize=auto)](https://ko-fi.com/A0A4G6LKI)

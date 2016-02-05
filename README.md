# InputReader
This is a library for handling keyboard input events.  Specifically it allows you to separate input from multiple "keyboards" (maybe more devices later).
To use it you must first identify the "keyboard" to track:

```C#
IntPtr deviceId = await DeviceManager.DetectKeyboard();
```
On the next "keypress" (magstripe, barcode, etc...) `deviceId` will be populated with the identifier of the keyboard - which you should be able to save for the next time the program is run.

Next we need to setup our listener:
```C#
BatchKeyboard scanner = new BatchKeyboard(deviceId);
scanner.BatchReceived += Scanner_BatchReceived;
DeviceManager.Listen(scanner);
```

That's it - your program is now listening (when it has focus - not globally) for any input from that "keyboard" device.  When a batch of keys are pressed (all the digits of the barcode, magstripe, rfid tag, etc...) then the `BatchReceived` is triggered.  The definition method definition is:
```C#
void Scanner_BatchReceived(object sender, string e){}
```
The second parameter `e` will contain the scanned data.

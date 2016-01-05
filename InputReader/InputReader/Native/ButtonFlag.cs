using System;

namespace Treorisoft.InputReader.Native
{
    [Flags]
    public enum ButtonFlag : ushort
    {
        LeftButtonDown = 0x0001,
        LeftButtonUp = 0x0002,
        MiddleButtonDown = 0x0010,
        MiddleButtonUp = 0x0020,
        RightButtonDown = 0x0004,
        RightButtonUp = 0x0008,

        Button1Down = LeftButtonDown,
        Button1Up = LeftButtonUp,
        Button2Down = RightButtonDown,
        Button2Up = RightButtonUp,
        Button3Down = MiddleButtonDown,
        Button3Up = MiddleButtonUp,
        Button4Down = 0x0040,
        Button4Up = 0x0080,
        Button5Down = 0x0100,
        Button5Up = 0x0200,

        Wheel = 0x0400
    }
}

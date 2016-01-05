using System;

namespace Treorisoft.InputReader.Native
{
    [Flags]
    public enum MouseFlag : ushort
    {
        MoveRelative = 0,
        MoveAbsolute = 1,
        VirtualDesktop = 2,
        AttributesChanged = 4
    }
}

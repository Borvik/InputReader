using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Treorisoft.InputReader.Native
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct RAWINPUT
    {
        [FieldOffset(0)]
        public RAWINPUTHEADER Header;
        [FieldOffset(16)]
        public RAWKEYBOARD Keyboard;
        [FieldOffset(16)]
        public RAWMOUSE Mouse;
        [FieldOffset(16)]
        public RAWHID Hid;
    }
}

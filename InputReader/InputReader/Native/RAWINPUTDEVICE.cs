using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Treorisoft.InputReader.Native
{
    [StructLayout(LayoutKind.Sequential)]
    struct RAWINPUTDEVICE
    {
        public ushort UsagePage;
        public ushort Usage;
        public int Flags;
        public IntPtr WindowHandle;

        public RAWINPUTDEVICE(ushort usagePage, ushort usage, int flags, IntPtr windowHandle)
        {
            UsagePage = usagePage;
            Usage = usage;
            Flags = flags;
            WindowHandle = windowHandle;
        }
    }
}

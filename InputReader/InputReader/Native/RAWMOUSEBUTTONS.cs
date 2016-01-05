using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Treorisoft.InputReader.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RAWMOUSEBUTTONS
    {
        [MarshalAs(UnmanagedType.U2)]
        public ButtonFlag usButtonFlags;
        [MarshalAs(UnmanagedType.U2)]
        public ushort usButtonData;
    }
}

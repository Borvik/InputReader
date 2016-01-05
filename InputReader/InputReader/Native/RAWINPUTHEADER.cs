using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Treorisoft.InputReader.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RAWINPUTHEADER
    {
        public RawInputType Type;
        public int Size;
        public IntPtr Device;
        public IntPtr wParam;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Treorisoft.InputReader.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RAWHID
    {
        [MarshalAs(UnmanagedType.U4)]
        public int dwSizHid;
        [MarshalAs(UnmanagedType.U4)]
        public int dwCount;
        public IntPtr ipRawData;
        /*********************************************
         *             To get the data               *
         *********************************************
         * *Note* - UNTESTED                         *
         *                                           *
         * size = dwSizHid * dwCount;                *
         * byte[] buffer = new byte[size];           *
         * Marshal.Copy(buffer, 0, ipRawData, size); *
         *********************************************/
    }
}

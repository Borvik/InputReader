using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Treorisoft.InputReader.Native;

namespace Treorisoft.InputReader
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterRawInputDevices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)]RAWINPUTDEVICE[] pRawInputDevices, int uiNumDevices, int cbSize);

        [DllImport("user32.dll")]
        private static extern int GetRawInputData(IntPtr hRawInput, RawInputCommand uiCommand, out RAWINPUT pData, ref int pcbSize, int cbSizeHeader);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int MapVirtualKey(int uCode, VirtualKeyMapType uMapType);

        public static bool RegisterRawInputDevices(RAWINPUTDEVICE[] devices)
        {
            bool result = RegisterRawInputDevices(devices, devices.Length, Marshal.SizeOf(typeof(RAWINPUTDEVICE)));
            if (!result)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return result;
        }
        public static RAWINPUT GetRawInputData(ref Message m)
        {
            RAWINPUT data = new RAWINPUT();
            int pcbSize = Marshal.SizeOf(typeof(RAWINPUT));

            int result = GetRawInputData(m.LParam, RawInputCommand.Input, out data, ref pcbSize, Marshal.SizeOf(typeof(RAWINPUTHEADER)));
            if (result == -1)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            if (result == 0)
                return new RAWINPUT();
            return data;
        }

        public static readonly IntPtr RIM_INPUT = new IntPtr(0); //foreground
        public static readonly IntPtr RIM_INPUTSINK = new IntPtr(1); //background

        internal static class WindowsMessages
        {
            public const int WM_INPUT = 0x00ff;
            public const int WM_KEYDOWN = 0x0100;
            public const int WM_KEYUP = 0x0101;
            public const int WM_SYSKEYDOWN = 0x0104;
            public const int WM_SYSKEYUP = 0x0105;

            public const int WM_CHAR = 0x0102;
            public const int WM_SYSCHAR = 0x0106;
            public const int WM_IME_CHAR = 0x0286;
        }
        internal static class RIDEV
        {
            public const int APPKEYS = 0x0400;
            public const int CAPTUREMOUSE = 0x0200;
            public const int DEVNOTIFY = 0x2000;
            public const int EXCLUDE = 0x0010;
            public const int EXINPUTSINK = 0x1000; //tends to fail (might require permissions)
            public const int INPUTSINK = 0x0100;
            public const int NOHOTKEYS = 0x0200;
            public const int NOLEGACY = 0x0030;
            public const int PAGEONLY = 0x0020;
            public const int REMOVE = 0x0001;
        }

        internal enum RawInputCommand
        {
            Input = 0x10000003,
            Header = 0x10000005
        }
    }
}

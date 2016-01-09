using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Treorisoft.InputReader.Native;

namespace Treorisoft.InputReader.Devices
{
    public abstract class Keyboard
    {
        protected IntPtr CurrentInputDevice = IntPtr.Zero;

        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x00ff: // WM_INPUT
                    ProcessRawInput(m);
                    break;
                case 0x0100: // WM_KEYDOWN
                    return ProcessKeyDown(m);
                case 0x0102: // WM_CHAR
                    return ProcessChar(m);
            }
            return false;
        }

        // remember we can't abort a WM_INPUT message
        private void ProcessRawInput(Message m)
        {
            RAWINPUT data = NativeMethods.GetRawInputData(ref m);
            if (data.Header.Type != RawInputType.Keyboard)
                return;

            if (data.Keyboard.VirtualKey == 255)
                return;

            switch (data.Keyboard.Message)
            {
                case 0x0100: // WM_KEYDOWN
                case 0x0104: // WM_SYSKEYDOWN
                    OnKeyDown(data);
                    break;
                case 0x0101: // WM_KEYUP
                case 0x0105: // WM_SYSKEYUP
                    OnKeyUp(data);
                    break;
            }
        }

        protected virtual bool ProcessKeyDown(Message m)
        {
            return false;
        }

        protected virtual bool ProcessChar(Message m)
        {
            return false;
        }

        private void OnKeyDown(RAWINPUT data)
        {
            lock (this)
            {
                CurrentInputDevice = data.Header.Device;
            }
        }
        private void OnKeyUp(RAWINPUT data)
        {
            lock (this)
            {
                CurrentInputDevice = IntPtr.Zero;
            }
        }
    }
}

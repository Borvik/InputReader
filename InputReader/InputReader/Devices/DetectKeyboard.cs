using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Treorisoft.InputReader.Devices
{
    public class DetectKeyboard : Keyboard
    {
        private System.Timers.Timer timer = null;
        private IntPtr DetectedKeyboard = IntPtr.Zero;

        public event EventHandler<IntPtr> KeyboardDetected;

        public DetectKeyboard()
        {
            timer = new System.Timers.Timer()
            {
                AutoReset = false,
                Enabled = false,
                Interval = 50
            };
            timer.Elapsed += batchTimer_Elapsed;
        }

        private void batchTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            IntPtr device = IntPtr.Zero;
            lock (this)
            {
                device = DetectedKeyboard;
            }
            if (device != IntPtr.Zero)
                OnKeyboardDetected(device);
        }

        protected void OnKeyboardDetected(IntPtr device)
        {
            lock (this)
            {
                var handler = KeyboardDetected;
                if (handler != null)
                    handler(this, device);
            }
        }

        protected override bool ProcessKeyDown(Message m)
        {
            timer.Stop();
            lock (this)
            {
                if (CurrentInputDevice != IntPtr.Zero && DetectedKeyboard == IntPtr.Zero)
                {
                    DetectedKeyboard = CurrentInputDevice;
                }
            }
            timer.Start();
            return true;
        }
    }
}

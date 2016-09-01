using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Treorisoft.InputReader.Devices
{
    public class DebugKeyboard : Keyboard
    {
        private IntPtr DeviceToWatch = IntPtr.Zero;
        private StringBuilder CapturedChars = null;
        private System.Timers.Timer timer = null;

        public event EventHandler<DebugBatchReceivedEventArgs> BatchReceived;

        public class DebugBatchReceivedEventArgs
        {
            public IntPtr Device { get; private set; }
            public string Data { get; private set; }

            public DebugBatchReceivedEventArgs(IntPtr device, string data)
            {
                Device = device;
                Data = data;
            }
        }

        public DebugKeyboard()
        {
            CapturedChars = new StringBuilder();
            timer = new System.Timers.Timer()
            {
                AutoReset = false,
                Enabled = false,
                Interval = 50
            };
            timer.Elapsed += batchTimer_Elapsed;
        }

        protected void OnBatchReceived(IntPtr device, string data)
        {
            var args = new DebugBatchReceivedEventArgs(device, data);
            BatchReceived?.Invoke(this, args);
        }

        private void batchTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            BatchFinished();
        }

        private void BatchFinished()
        {
            string data = string.Empty;
            lock (this)
            {
                if (CapturedChars.Length > 0)
                    data = CapturedChars.ToString();
                CapturedChars.Clear();
            }

            if (data != string.Empty)
                OnBatchReceived(DeviceToWatch, data);
        }

        protected override bool ProcessChar(Message m)
        {
            lock (this)
            {
                if (CurrentInputDevice != DeviceToWatch)
                    return false;
            }
            CapturedChars.Append((char)m.WParam);
            return true;
        }

        protected override bool ProcessKeyDown(Message m)
        {
            lock (this)
            {
                if (CurrentInputDevice != DeviceToWatch)
                {
                    BatchFinished();
                    DeviceToWatch = CurrentInputDevice;
                }
            }

            timer.Stop();
            Keys k = (Keys)(int)m.WParam;
            char? data = null;
            switch (k)
            {
                case Keys.Tab:
                    data = '\t';
                    break;
            }
            if (data.HasValue)
                CapturedChars.Append(data.Value);
            timer.Start();
            return data.HasValue;
        }
    }
}

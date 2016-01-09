using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Treorisoft.InputReader;
using Treorisoft.InputReader.Devices;

namespace InputReaderDemo
{
    public partial class Form1 : Form
    {
        BatchKeyboard scanner = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Log(string message)
        {
            txtLog.AppendText(message + Environment.NewLine);
        }
        private void Log(string message, params object[] args)
        {
            Log(string.Format(message, args));
        }

        private void cmdAction_Click(object sender, EventArgs e)
        {
            switch (cmdAction.Text)
            {
                case "Capture Device":
                    CaptureDevice();
                    break;
                case "Stop Capture":
                    StopCapture();
                    break;
                case "Release Device":
                    ReleaseDevice();
                    break;
            }
        }

        private async void CaptureDevice()
        {
            try
            {
                cmdAction.Text = "Stop Capture";
                IntPtr deviceId = await DeviceManager.DetectKeyboard();
                if (deviceId == IntPtr.Zero)
                    Log("Failed to capture device");
                else
                {
                    scanner = new BatchKeyboard(deviceId);
                    scanner.BatchReceived += Scanner_BatchReceived;
                    DeviceManager.Listen(scanner);
                    Log("Captured device ({0})", deviceId);
                    cmdAction.Text = "Release Device";
                }
            }
            catch (Exception ex)
            {
                Log("Error capturing device: " + ex.Message);
            }
        }

        private void StopCapture()
        {
            DeviceManager.CancelDetectKeyboard();
            cmdAction.Text = "Capture Device";
        }

        private void ReleaseDevice()
        {
            DeviceManager.Listen(null);
            scanner.BatchReceived -= Scanner_BatchReceived;
            scanner = null;
            cmdAction.Text = "Capture Device";
        }

        private void Scanner_BatchReceived(object sender, string e)
        {
            if (txtLog.InvokeRequired)
                txtLog.Invoke((MethodInvoker)delegate { Log("Batch Input: {0}", e); });
            else
                Log("Batch Input: {0}", e);
        }
    }
}

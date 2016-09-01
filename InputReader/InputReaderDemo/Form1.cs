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

        private enum LogType
        {
            Test,
            Debug
        }

        private void Log(LogType logType, string message)
        {
            switch (logType)
            {
                case LogType.Debug:
                    txtDebugLog.AppendText(message + Environment.NewLine);
                    break;
                case LogType.Test:
                    txtLog.AppendText(message + Environment.NewLine);
                    break;
            }
        }
        private void Log(LogType logType, string message, params object[] args)
        {
            Log(logType, string.Format(message, args));
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
            txtTestBox.Focus();
        }

        private async void CaptureDevice()
        {
            try
            {
                cmdAction.Text = "Stop Capture";
                IntPtr deviceId = await DeviceManager.DetectKeyboard();
                if (deviceId == IntPtr.Zero)
                    Log(LogType.Test, "Failed to capture device");
                else
                {
                    scanner = new BatchKeyboard(deviceId);
                    scanner.BatchReceived += Scanner_BatchReceived;
                    StartListen(scanner);
                    Log(LogType.Test, "Captured device ({0})", deviceId);
                    cmdAction.Text = "Release Device";
                }
            }
            catch (Exception ex)
            {
                Log(LogType.Test, "Error capturing device: " + ex.Message);
            }
        }

        private void StopCapture()
        {
            DeviceManager.CancelDetectKeyboard();
            cmdAction.Text = "Capture Device";
        }

        private void StartListen(Keyboard device)
        {
            if (DeviceManager.IsKeyboardListening())
                ResetButtons();
            DeviceManager.Listen(device);
        }

        private void ReleaseDevice()
        {
            DeviceManager.Listen(null);
            if (scanner != null)
            {
                scanner.BatchReceived -= Scanner_BatchReceived;
                scanner = null;
            }
            if(debugScanner != null)
            {
                debugScanner.BatchReceived -= Debug_BatchReceived;
                debugScanner = null;
            }
            ResetButtons();
        }

        private void ResetButtons()
        {
            cmdAction.Text = "Capture Device";
            cmdStartStopDebug.Text = "Start Debug";
        }

        private void Scanner_BatchReceived(object sender, string e)
        {
            if (txtLog.InvokeRequired)
                txtLog.Invoke((MethodInvoker)delegate { Log(LogType.Test, "Batch Input: {0}", e); });
            else
                Log(LogType.Test, "Batch Input: {0}", e);
        }

        private void cmdStartStopDebug_Click(object sender, EventArgs e)
        {
            switch (cmdStartStopDebug.Text)
            {
                case "Start Debug":
                    StartDebug();
                    break;
                case "Stop Debug":
                    ReleaseDevice();
                    break;
            }
        }

        DebugKeyboard debugScanner = null;
        private void StartDebug()
        {
            cmdStartStopDebug.Enabled = false;
            try
            {
                debugScanner = new DebugKeyboard();
                debugScanner.BatchReceived += Debug_BatchReceived;
                StartListen(debugScanner);
                Log(LogType.Debug, "Debugging Started");
                cmdStartStopDebug.Text = "Stop Debug";
            }
            catch (Exception ex)
            {
                Log(LogType.Debug, "Error capturing device: " + ex.Message);
            }
            cmdStartStopDebug.Enabled = true;
        }
        private void Debug_BatchReceived(object sender, DebugKeyboard.DebugBatchReceivedEventArgs e)
        {
            if (txtLog.InvokeRequired)
                txtLog.Invoke((MethodInvoker)delegate { Log(LogType.Debug, "Batch Input ({0}): {1}", e.Device, e.Data); });
            else
                Log(LogType.Debug, "Batch Input ({0}): {1}", e.Device, e.Data);
        }

    }
}

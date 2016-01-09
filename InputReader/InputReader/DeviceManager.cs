using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Treorisoft.InputReader.Devices;
using Treorisoft.InputReader.Native;

namespace Treorisoft.InputReader
{
    public class DeviceManager : IMessageFilter
    {
        private static DeviceManager manager = null;
        private static Keyboard keyboard = null;

        static DeviceManager()
        {
            manager = new DeviceManager();
            Application.AddMessageFilter(manager);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (keyboard != null)
                return keyboard.PreFilterMessage(ref m);
            return false;
        }

        public static Keyboard Listen(Keyboard device)
        {
            var oldKeyboard = keyboard;
            keyboard = device;
            if (oldKeyboard != null && device == null)
            {
                //Stop receiving WM_INPUT
                RAWINPUTDEVICE[] devices =
                {
                    new RAWINPUTDEVICE()
                    {
                        UsagePage = 1, // Generic Desktop Controls
                        Usage = 6, // Keyboard
                        Flags = 0x0001, // RIDEV_REMOVE
                        WindowHandle = IntPtr.Zero
                    }
                };
                if (!NativeMethods.RegisterRawInputDevices(devices))
                    throw new Exception("Failed to unregister raw input");
            }
            else if (oldKeyboard == null && device != null)
            {
                // Start listening to WM_INPUT
                RAWINPUTDEVICE[] devices =
                {
                    new RAWINPUTDEVICE()
                    {
                        UsagePage = 1, // Generic Desktop Controls
                        Usage = 6, // Keyboard
                        Flags = 0x0100, // RIDEV_INPUTSINK
                        WindowHandle = HiddenWindow.StaticHandle
                    }
                };
                if (!NativeMethods.RegisterRawInputDevices(devices))
                    throw new Exception("Failed to register for raw input");
            }
            return oldKeyboard;
        }

        private TaskCompletionSource<IntPtr> DetectKeyboardTask = null;
        public static async Task<IntPtr> DetectKeyboard()
        {
            var kbd = new DetectKeyboard();
            kbd.KeyboardDetected += (_, device) =>
            {
                manager.DetectKeyboardTask.SetResult(device);
            };

            var origKeyboard = Listen(kbd);
            var task = new TaskCompletionSource<IntPtr>();

            IntPtr detectedDevice = IntPtr.Zero;
            manager.DetectKeyboardTask = task;
            await task.Task;
            if (!task.Task.IsCanceled && !task.Task.IsFaulted)
                detectedDevice = task.Task.Result;
            manager.DetectKeyboardTask = null;

            Listen(origKeyboard);
            return detectedDevice;
        }

        public static void CancelDetectKeyboard()
        {
            if (manager.DetectKeyboardTask != null && !manager.DetectKeyboardTask.Task.IsCompleted)
                manager.DetectKeyboardTask.SetResult(IntPtr.Zero);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Treorisoft.InputReader.Native
{
    internal class HiddenWindow : NativeWindow
    {
        private static HiddenWindow _instance = null;

        public HiddenWindow()
        {
            this.CreateHandle(new CreateParams());
        }

        public static IntPtr StaticHandle
        {
            get
            {
                if (_instance == null)
                    _instance = new HiddenWindow();
                return _instance.Handle;
            }
        }
    }
}

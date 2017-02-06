using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorPicker
{
    public class ColorDialog : System.Windows.Forms.ColorDialog
    {
        public event EventHandler Deactivated;

        private const int WM_ACTIVATEAPP = 0x001C;

        protected override IntPtr HookProc(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            switch (msg)
            {
                case WM_ACTIVATEAPP:
                    if (wparam.ToInt32() == 0)
                    {
                        if (Deactivated != null)
                        {
                            Deactivated(this, null);
                        }
                    }
                    break;
            }
            return base.HookProc(hWnd, msg, wparam, lparam);
        }
    }
}

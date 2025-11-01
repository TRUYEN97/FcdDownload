using System;
using System.Windows.Forms;

namespace CPEI_MFG
{
    public static class InvokeUtil
    {

        public static void SafeInvoke(Control control, Action updateAction)
        {
            try
            {
                if (control == null)
                {
                    return;
                }
                if (control.InvokeRequired)
                {
                    control.Invoke(updateAction);
                }
                else
                {
                    updateAction();
                }
            }
            catch
            {
            }
        }
    }
}

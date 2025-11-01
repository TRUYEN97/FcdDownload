using System;
using System.Threading;
using System.Windows.Forms;

namespace WebControl_WinForm
{
    internal static class Program
    {
        static Mutex newMutex;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool canCreate;
            newMutex = new Mutex(true, "FcdProgram", out canCreate);
            if (!canCreate)
            {
                MessageBox.Show("Test Program already running", "Warning");
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new UIDL());
            }
        }
    }
}

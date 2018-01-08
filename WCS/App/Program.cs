using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace App
{
    static class Program
    {
        public static Main mainForm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new Main();
            
            bool ExisFlag = false;
            System.Diagnostics.Process currentProccess = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process[] currentProccessArray = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in currentProccessArray)
            {
                if (p.ProcessName == currentProccess.ProcessName && p.Id != currentProccess.Id)
                {
                    ExisFlag = true;
                    break;
                }
            }

            if (ExisFlag)
            {
                MessageBox.Show("仓储调度监控系统！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                Application.Run(mainForm);
            }
        }
    }
}

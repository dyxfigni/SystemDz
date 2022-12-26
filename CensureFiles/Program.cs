using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace CensureFiles
{
    internal static class Program
    {
        private static Mutex _mutex;

        [STAThread]
        static void Main()
        {
            bool isNew;
            _mutex = new Mutex(true, "Form1", out isNew);
            if (!isNew)
            {
                MessageBox.Show("Another copy is running");
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

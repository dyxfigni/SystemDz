using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportDirectoryAndStringByTasks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //openFileDialog1.ShowDialog();
            //string path = openFileDialog1.InitialDirectory;

            

            string dirName = "E:\\kod\\SystemProgramming\\SystemDz\\ReportDirectoryAndStringByTasks\\bin\\Debug\\SourceDir";

            DirectoryInfo dirInfo = new DirectoryInfo(dirName);

            if (dirInfo.Exists)
            {
                FileInfo[] files = dirInfo.GetFiles("(*.");
            }
            

        }
    }
}

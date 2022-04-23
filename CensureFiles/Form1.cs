using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CensureFiles
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread((() =>
            {
                Manager manager = new Manager(@"E:\test", edText.Text);
                manager.ReadChildren();

                Stopwatch stopwatch = Stopwatch.StartNew();

                manager.ReadChildren();

                Action action = () => MessageBox.Show($"First - {stopwatch.Elapsed.TotalSeconds}");

                Invoke(action);
                
                stopwatch.Restart();

                manager.ReadChildren();

                action = () => MessageBox.Show($"Second - {stopwatch.Elapsed.TotalSeconds}");

                Invoke(action);

                stopwatch.Stop();

                manager.SaveToFile(@"E:\test\TestResults.xml");
            }));

            thread.Start();
        }

        private void edText_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

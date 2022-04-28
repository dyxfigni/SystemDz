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
        private Thread thread;

        private static CancellationTokenSource cts = new CancellationTokenSource();

        private CancellationToken ct = cts.Token;

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnStart_Click(object sender, EventArgs e)
        { 
            thread = new Thread((() =>
            {
                if (ed2Path.Text == null)
                    ed2Path.Text = "E:\\test";
                if (edText.Text == null)
                    edText.Text = "Blabla";

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

        async void StartAlgorithm()
        {
            await Task.Run(async () =>
            {
                
            });
        }


        private void edText_TextChanged(object sender, EventArgs e)
        {

        }

        private void ed2Path_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

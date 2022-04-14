using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SortingNumbersByEventsDz
{
    public partial class Form1 : Form
    {
        static AutoResetEvent myEvent = new AutoResetEvent(true);
        private static Dictionary<int, int> pair = new Dictionary<int, int>();
        private static XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<int>));

        public Form1()
        {
            InitializeComponent();

            btnStop.Enabled = false;
            btnStart.Enabled = true;

            lstBox1.Items.Clear();
            lstBox2.Items.Clear();
            lstBox3.Items.Clear();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(GenNumForPairs);
            Thread thread2 = new Thread(GetSumInPairs);
            Thread thread3 = new Thread(GetProductInPairs);


            thread1.Start();
            thread2.Start();
            thread3.Start();

            MessageBox.Show("Completed", "Message");

            FillMyListBox(lstBox1);
        }

        static void GenNumForPairs()
        {
            myEvent.WaitOne();
            Random random = new Random();

            //Dictionary<int, int> pair = new Dictionary<int, int>();

            for (int i = 0; i < new Random().Next(50, 200); i++)
            {
                int key = random.Next(0, 1000);
                while(pair.ContainsKey(key))
                {
                    key = random.Next(0, 1000);
                }

                pair.Add(key, random.Next(0, 1000));
            }

            myEvent.Reset();
        }

        static void GetSumInPairs()
        {
            myEvent.WaitOne();
            int Sum = 0;

            foreach (KeyValuePair<int, int> KeyValuePair in pair)
            {
                Sum = KeyValuePair.Value + KeyValuePair.Key;
            }

            myEvent.Reset();
        }

        static void GetProductInPairs()
        {
            myEvent.WaitOne();

            int Product = 0;

            foreach (KeyValuePair<int, int> KeyValuePair in pair)
            {
                Product = KeyValuePair.Value * KeyValuePair.Key;
            }

            myEvent.Reset();
        }

        private void FillMyListBox(ListBox myListBox, string path)
        {
            //Полная чистка перед добавлением элементов   
            myListBox.Items.Clear();
            List<int> temp;
            using (StreamReader reader = new StreamReader(path))
            {
                temp = (List<int>)xmlSerializer.Deserialize(reader);
            }

            foreach (int number in temp)
            {
                myListBox.Items.Add(number);
            }

            temp.Clear();
            temp = null;
        }
        private void FillMyListBox(ListBox myListBox)
        {
            //Полная чистка перед добавлением элементов   
            myListBox.Items.Clear();
            //List<int> temp;

            foreach (KeyValuePair<int, int> KeyValuePair in pair)
            {
                myListBox.Items.Add($"First: {KeyValuePair.Key} Second: {KeyValuePair.Value}");
            }
        }
    }
}


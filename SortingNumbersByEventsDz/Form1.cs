using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SortingNumbersByEventsDz
{
    public partial class Form1 : Form
    {
        private static ManualResetEvent myResetEvent = new ManualResetEvent(false);
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
            FillMyListBox(lstBox2, "SumOfPairs.xml");
            FillMyListBox(lstBox3, "ProductOfPairs.xml");

            btnStop.Enabled = true;
            btnStart.Enabled = false;
        }

        static void GenNumForPairs()
        {
            Random random = new Random();

            for (int i = 0; i < new Random().Next(50, 200); i++)
            {
                int key = random.Next(0, 1000);
                while(pair.ContainsKey(key))
                {
                    key = random.Next(0, 1000);
                }

                pair.Add(key, random.Next(0, 1000));
            }

            using (StreamWriter writer = new StreamWriter("Pairs.xml", false))
            {
                foreach (KeyValuePair<int, int> entry in pair)
                    writer.WriteLine("[{0} {1}]", entry.Key, entry.Value);
            }

            myResetEvent.Set();
        }

        static void GetSumInPairs()
        {
            myResetEvent.Reset();
            myResetEvent.WaitOne();

            List<int> sumList = new List<int>();

            foreach (KeyValuePair<int, int> entry in pair)
            {
                sumList.Add(entry.Key + entry.Value);
            }

            using (StreamWriter writer = new StreamWriter("SumOfPairs.xml", false))
            {
                xmlSerializer.Serialize(writer, sumList);
            }

            sumList.Clear();
            sumList = null;
            myResetEvent.Set();
        }

        static void GetProductInPairs()
        {
            myResetEvent.Reset();
            myResetEvent.WaitOne();
            List<int> productsList = new List<int>();

            foreach (KeyValuePair<int, int> entry in pair)
            {
                productsList.Add(entry.Value * entry.Key);
            }

            using (StreamWriter writer = new StreamWriter("ProductOfPairs.xml", false))
            {
                xmlSerializer.Serialize(writer, productsList);
            }

            productsList.Clear();
            productsList = null;
            myResetEvent.Set();
        }

        private void FillMyListBox(ListBox myListBox, string path)
        {
            //Полная чистка перед добавлением элементов   
            myListBox.Items.Clear();
            List<int> temp = new List<int>();
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
            foreach (KeyValuePair<int, int> keyValuePair in pair)
            {
                myListBox.Items.Add($"First: {keyValuePair.Key} Second: {keyValuePair.Value}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = true;

            lstBox1.Items.Clear();
            lstBox2.Items.Clear();
            lstBox3.Items.Clear();
        }
    }
}
using System;
using System.Collections;
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
        private static ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private static XmlSerializer _xmlSerializer = new XmlSerializer(typeof(List<int>));
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

            FillListBox(lstBox1);
            FillListBox(lstBox2, "SumOfPairs.xml");
            FillListBox(lstBox3, "ProductOfPairs.xml");

            btnStop.Enabled = true;
            btnStart.Enabled = false;
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = true;

            lstBox1.Items.Clear();
            lstBox2.Items.Clear();
            lstBox3.Items.Clear();
        }

        #region MethodsForThreads

        static void GenNumForPairs()
        {
            Dictionary<int, int> pairDictionary = new Dictionary<int, int>();
            Random random = new Random();

            for (int i = 0; i < new Random().Next(50, 200); i++)
            {
                int key = random.Next(0, 1000);
                while (pairDictionary.ContainsKey(key)) {
                    key = random.Next(0, 1000);
                }

                pairDictionary.Add(key, random.Next(0, 1000));
            }

            using (StreamWriter writer = new StreamWriter("Pairs.xml", false)) {
                Serialize(writer, pairDictionary);
            }
            _resetEvent.Set();
        }

        static void GetSumInPairs()
        {
            _resetEvent.Reset();
            _resetEvent.WaitOne();
            Dictionary<int, int> pairDictionary = new Dictionary<int, int>();

            using (StreamReader reader = new StreamReader("Pairs.xml")) {
                Deserialize(reader, pairDictionary);
            }

            List<int> sumList = new List<int>();
            foreach (KeyValuePair<int, int> entry in pairDictionary) {
                sumList.Add(entry.Key + entry.Value);
            }

            pairDictionary.Clear();
            pairDictionary = null;

            using (StreamWriter writer = new StreamWriter("SumOfPairs.xml", false)) {
                _xmlSerializer.Serialize(writer, sumList);
            }

            sumList.Clear();
            sumList = null;
            _resetEvent.Set();
        }

        static void GetProductInPairs()
        {
            _resetEvent.Reset();
            _resetEvent.WaitOne();

            Dictionary<int, int> pairDictionary = new Dictionary<int, int>();

            using (StreamReader reader = new StreamReader("Pairs.xml")) {
                Deserialize(reader, pairDictionary);
            }

            List<int> productsList = new List<int>();
            foreach (KeyValuePair<int, int> entry in pairDictionary) {
                productsList.Add(entry.Value * entry.Key);
            }

            pairDictionary.Clear();
            pairDictionary = null;

            using (StreamWriter writer = new StreamWriter("ProductOfPairs.xml", false)) {
                _xmlSerializer.Serialize(writer, productsList);
            }

            productsList.Clear();
            productsList = null;
            _resetEvent.Set();
        }


        #endregion

        #region MethodsForFillingListBoxes

        private void FillListBox(ListBox myListBox, string path)
        {
            //Полная чистка перед добавлением элементов   
            myListBox.Items.Clear();
            List<int> temp = new List<int>();
            using (StreamReader reader = new StreamReader(path))
            {
                temp = (List<int>)_xmlSerializer.Deserialize(reader);
            }

            foreach (int number in temp)
            {
                myListBox.Items.Add(number);
            }

            temp.Clear();
            temp = null;
        }
        private void FillListBox(ListBox myListBox)
        {
            //Полная чистка перед добавлением элементов   
            myListBox.Items.Clear();

            Dictionary<int, int> pairDictionary = new Dictionary<int, int>();
            using (StreamReader reader = new StreamReader("Pairs.xml"))
            {
                Deserialize(reader, pairDictionary);
            }

            foreach (KeyValuePair<int, int> keyValuePair in pairDictionary)
            {
                myListBox.Items.Add($"First: {keyValuePair.Key} Second: {keyValuePair.Value}");
            }
        }

        #endregion
        
        public static void Serialize(TextWriter writer, IDictionary dictionary)
        {
            List<Entry> entries = new List<Entry>(dictionary.Count);
            foreach (object key in dictionary.Keys)
            {
                entries.Add(new Entry(key, dictionary[key]));
            }
            XmlSerializer serializer = new XmlSerializer(typeof(List<Entry>));
            serializer.Serialize(writer, entries);
        }
        public static void Deserialize(TextReader reader, IDictionary dictionary)
        {
            dictionary.Clear();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Entry>));
            List<Entry> entries = (List<Entry>)serializer.Deserialize(reader);
            foreach (Entry entry in entries)
            {
                dictionary[entry.Key] = entry.Value;
            }
        }

        [Serializable]
        public class Entry
        {
            public object Key;
            public object Value;
            public Entry()
            {
                this.Key = null;
                this.Value = null;
            }

            public Entry(object key, object value)
            {
                Key = key;
                Value = value;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
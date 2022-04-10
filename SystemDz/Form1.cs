using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace SystemDz
{
    public partial class Form1 : Form
    {
        private static Mutex mutex;
        private static XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<int>));

        public Form1()
        {
            InitializeComponent();
            mutex = new Mutex();

            btnStop.Enabled = false;
            btnStart.Enabled = true;

            lstBox1.Items.Clear();
            lstBox2.Items.Clear();
            lstBox3.Items.Clear();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = true;
            btnStart.Enabled = false;
            Thread thread1 = new Thread(GenRandNum);
            Thread thread2 = new Thread(SimpleNum);
            Thread thread3 = new Thread(WhereNum7);
            thread1.Start();
            thread1.Join();

            thread2.Start();
            thread2.Join();

            thread3.Start();
            thread3.Join();

            MessageBox.Show("Completed", "Message");
            FillMyListBox(lstBox1, "Text.xml");
            FillMyListBox(lstBox2, "TextSimple.xml");
            FillMyListBox(lstBox3, "TextSimpleNum7.xml");

            Thread threadReport = new Thread(Report);
            threadReport.Start();
            threadReport.Join();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = true;

            lstBox1.Items.Clear();
            lstBox2.Items.Clear();
            lstBox3.Items.Clear();
        }

        #region methods

        static void GenRandNum()
        {
            mutex.WaitOne();
            Random random = new Random();
            List<int> temp = new List<int>();

            for (int i = 0; i < new Random().Next(50, 200); i++)
            {
                temp.Add(random.Next(0, 1000));
            }

            //запись в файл
            using (StreamWriter writer = new StreamWriter("Text.xml", false))
            {
                xmlSerializer.Serialize(writer, temp);
            }
            temp.Clear();
            temp = null;

            mutex.ReleaseMutex();
        }

        static void SimpleNum()
        {
            mutex.WaitOne();
            // чтение из файла
            List<int> buffer;
            using (StreamReader reader = new StreamReader("Text.xml"))
            {
                buffer = (List<int>)xmlSerializer.Deserialize(reader);
            }

            List<int> temp = new List<int>();
            //простое число
            foreach (int number in buffer)
            {
                if (number > 1)
                {
                    if ((number * number) % 24 == 1)
                        temp.Add(number);

                    if (number == 2 || number == 3)
                        temp.Add(number);
                }
                else
                    temp.Add(number);
            }

            buffer.Clear();
            buffer = null;

            // запись в файл
            using (StreamWriter writer = new StreamWriter("TextSimple.xml", false))
            {
                xmlSerializer.Serialize(writer, temp);
            }
            temp.Clear();
            temp = null;

            mutex.ReleaseMutex();
        }

        static void WhereNum7()
        {
            mutex.WaitOne();
            // чтение из файла
            List<int> buffer;
            using (StreamReader reader = new StreamReader("TextSimple.xml"))
            {
                buffer = (List<int>)xmlSerializer.Deserialize(reader);
            }

            List<int> temp = new List<int>();

            // если число оканчивается на 7
            foreach (int number in buffer)
            {
                if ((number % 10) == 7)
                    temp.Add(number);
            }

            buffer.Clear();
            buffer = null;

            // запись в файл
            using (StreamWriter writer = new StreamWriter("TextSimpleNum7.xml", false))
            {
                xmlSerializer.Serialize(writer, temp);
            }
            temp.Clear();
            temp = null;

            mutex.ReleaseMutex();
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

        private static void Report()
        {
            mutex.WaitOne();
            List<int> temp = new List<int>();

            List<int> NumberString = new List<int>();
            List<byte> FileLength = new List<byte>();
            string path = "Text.xml";

            NumberString.Capacity = 3;
            FileLength.Capacity = 3;
            using (StreamReader reader = new StreamReader(path)){
                temp = (List<int>)xmlSerializer.Deserialize(reader);
            }
            NumberString.Add(temp.Count());
            FileLength.Add((byte)File.ReadAllBytes(path).Length);

            temp.Clear();
            path = "TextSimple.xml";
            using (StreamReader reader = new StreamReader(path)) {
                temp = (List<int>)xmlSerializer.Deserialize(reader);
            }
            NumberString.Add(temp.Count());
            FileLength.Add((byte)File.ReadAllBytes(path).Length);

            temp.Clear();
            path = "TextSimpleNum7.xml";
            using (StreamReader reader = new StreamReader(path)) {
                temp = (List<int>)xmlSerializer.Deserialize(reader);
            }
            NumberString.Add(temp.Count());
            FileLength.Add((byte)File.ReadAllBytes(path).Length);

            using (StreamWriter writer = new StreamWriter("ReportFile.xml", false))
            {
                xmlSerializer.Serialize(writer, NumberString);
            }
            //using (StreamWriter writer = new StreamWriter("ReportFile.xml", true))
            //{
            //    xmlSerializer.Serialize(writer, FileLength);
            //}

            MessageBox.Show($"First file: amount is {NumberString[0].ToString()}\n" +
                            $"        length in bytes is {FileLength[0].ToString()}\n\n" +
                            $"Second file: {NumberString[1].ToString()}\n" +
                            $"        length in bytes is {FileLength[1].ToString()}\n\n" +
                            $"Third file: {NumberString[2].ToString()}\n" +
                            $"        length in bytes is {FileLength[2].ToString()}",
                            "Report from each file");

            path = null;
            temp.Clear();
            NumberString.Clear();
            FileLength.Clear();
            temp = null;
            NumberString = null;
            FileLength = null;
            mutex.ReleaseMutex();
        }

        #endregion

        private void lstBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void lstBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
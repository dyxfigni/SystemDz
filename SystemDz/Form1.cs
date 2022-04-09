using System;
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
using System.Xml.Serialization;

namespace SystemDz
{

    public partial class Form1 : Form
    {
        private static Mutex mutex;
        private static List<int> numbersList;
        static XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<int>));
        static int k = new Random().Next(50, 200);

        public Form1()
        {
            InitializeComponent();
            numbersList = new List<int>();
            mutex = new Mutex();
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
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = true;
        }

        static void GenRandNum()
        {
            mutex.WaitOne();
            Random random = new Random();

            for (int i = 0; i < k; i++)
            {
                numbersList.Add(random.Next(0, 1000));

            }

            //запись в файл
            using (StreamWriter writer = new StreamWriter("Text.xml", false))
            {
                xmlSerializer.Serialize(writer, numbersList);
            }

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
            for (int i = 0; i < buffer.Count; i++)
            {
                if (buffer[i] > 1)
                {
                    if ((buffer[i] * buffer[i]) % 24 == 1)
                        temp.Add(buffer[i]);

                    if (buffer[i] == 2 || buffer[i] == 3)
                        temp.Add(buffer[i]);
                }
                else
                    temp.Add(buffer[i]);
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
            for (int i = 0; i < buffer.Count; i++)
            {
                if ((buffer[i] % 10) == 7)
                {
                    temp.Add(buffer[i]);

                }
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

        private void lstBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

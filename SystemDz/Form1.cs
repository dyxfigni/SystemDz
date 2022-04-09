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
        private static int[] array;
        static XmlSerializer xmlSerializer = new XmlSerializer(typeof(int[]));

        public Form1()
        {
            InitializeComponent();
            array = new int[new Random().Next(50, 200)];
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

            for (int i = 0; i < array.Length; i++)
            {
                lstBox1.Items.Add(array[i]);
                Thread.Sleep(50);
            }


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
            for (int i = 0; i < array.Length; i++)
            {
                array[i] += new Random().Next(0, 1000);
            }

            //запись в файл
            using (StreamWriter writer = new StreamWriter("Text.xml", false))
            {
                xmlSerializer.Serialize(writer, array);
            }

            mutex.ReleaseMutex();
        }

        static void SimpleNum()
        {
            // чтение из файла
            int[] buffer;
            using (StreamReader reader = new StreamReader("Text.xml"))
            {
                buffer = (int[])xmlSerializer.Deserialize(reader);
            }

            int[] temp = new int[buffer.Length];
            
            //простое число
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] > 1)
                {
                    for (int j = 2; j < buffer[i]; j++)
                    {
                        if (buffer[i] % 1 != 0)
                        {
                            temp[j] = buffer[i];
                        }
                    }
                }
            }

            buffer = null;

            // запись в файл
            using (StreamWriter writer = new StreamWriter("TextSimple.xml", false))
            {
                xmlSerializer.Serialize(writer, temp);
            }
        }

        static void WhereNum7()
        {
            // чтение из файла
            int[] buffer;
            using (StreamReader reader = new StreamReader("TextSimple.xml"))
            {
                buffer = (int[])xmlSerializer.Deserialize(reader);
            }

            int[] temp = new int[buffer.Length];

            // если число оканчивается на 7
            for (int i = 0; i < buffer.Length; i++)
            {
                for (int j = 0; j < temp.Length; j++)
                {
                    if ((buffer[i] % 10) == 7)
                    {
                        temp[j] = buffer[i];
                    }
                }
            }

            // запись в файл
            using (StreamWriter writer = new StreamWriter("TextSimpleNum7.xml", false))
            {
                xmlSerializer.Serialize(writer, temp);
            }
        }

    private void lstBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

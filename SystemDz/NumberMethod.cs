using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SystemDz
{
    class NumberMethod
    {
        private bool _canceled;
        private int _progress;
        public event Action<int> ProgressChanged;
        public event Action<bool> WorkEnded;
        static XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<int>));
        public Mutex mutex;
        static int k = new Random().Next(50, 200);
        public List<int> numbersList;

        public void Canceled()
        {
            _canceled = true;
        }

        public void Reset()
        {
            _canceled = false;
            _progress = 0;
        }
        public void GenRandNum()
        {
            mutex.WaitOne();
            Random random = new Random();

            for (int i = 0; i < k; i++)
            {
                numbersList.Add(random.Next(0, 1000));
                _progress++;
            }

            //запись в файл
            using (StreamWriter writer = new StreamWriter("Text.xml", false))
            {
                xmlSerializer.Serialize(writer, numbersList);
            }

            mutex.ReleaseMutex();
        }
        public void SimpleNum()
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

                _progress++;
                Thread.Sleep(50);
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
        public void WhereNum7()
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

                _progress++;
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

        public void OnProgressChange(object i)
        {
            //if (ProgressChanged != null)
            //    ProgressChanged((int)i);
            ProgressChanged?.Invoke((int)i);
        }
        public void OnWorkEnded(object cancel)
        {
            if (WorkEnded != null)
            {
                WorkEnded((bool)cancel);
            }
        }
        public override string ToString()
        {
            return string.Format("Thread ID: {0}, Value: {1}", Thread.CurrentThread.ManagedThreadId, _progress);
        }
    }
}

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

namespace SortingNumbersByEventsDz
{
    public partial class Form1 : Form
    {
        static AutoResetEvent myEvent = new AutoResetEvent(false);
        private static List<int> numberList = new List<int>();
        public Form1()
        {
            InitializeComponent();

            lst
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {

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
    }
}

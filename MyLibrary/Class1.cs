using System.Windows.Forms;
using System.Xml.Serialization;

namespace MyLibrary
{
    static class FillListBoxClass
    {
        private static void FillListBox(ListBox ListBox, string path, XmlSerializer xmlSerializer)
        {
            //Полная чистка перед добавлением элементов   
            ListBox.Items.Clear();

            XmlSerializer _xmlSerializer = new XmlSerializer(typeof(List<int>));

            List<int> temp = new List<int>();
            using (StreamReader reader = new StreamReader(path))
            {
                temp = (List<int>)_xmlSerializer.Deserialize(reader);
            }

            foreach (int number in temp)
            {
                ListBox.Items.Add(number);
            }

            temp.Clear();
            temp = null;
        }
        private static void FillListBox(ListBox ListBox, IDictionary<object, object> dictionary)
        {
            //Полная чистка перед добавлением элементов   
            ListBox.Items.Clear();
            foreach (KeyValuePair<object, object> keyValuePair in dictionary)
            {
                ListBox.Items.Add($"First: {keyValuePair.Key} Second: {keyValuePair.Value}");
            }
        }
    }
}
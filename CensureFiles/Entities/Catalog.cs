using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CensureFiles.Entities
{
    public class Catalog
    {
        public string path { get; set; }

        object locker = null;  // объект-заглушка

        public List<File> Files { get; set; } = new List<File>();

        public List<Catalog> Catalogs { get; set; } = new List<Catalog>(); 
        public string SearchText { get; set; }

        public Catalog(string path)
        {

            if (Directory.Exists(path))
                this.path = path;
            else
                throw new Exception("Invalid argument");
        }

        public Catalog() { }


        public async Task ReadAllChildren(Catalog Curr = null)
        {
            if (Curr == null){
                Curr = this;
            }
            
            string[] Catalogs = null;
            try
            {
                Catalogs = Directory.GetDirectories(Curr.path);
            }
            catch(Exception) {
                return;
            }

            foreach (var catalog in Catalogs)
            {
                if (Curr.Catalogs.Count(dir => dir.path == catalog) == 0)
                {
                    if (Directory.Exists(catalog))
                    {
                        Catalog newDir = new Catalog(catalog);
                        Curr.Catalogs.Add(newDir);
                        ReadAllChildren(newDir);
                    }
                }
            }

            foreach (var item in Directory.GetFiles(Curr.path))
            {
                if (Curr.Files.Count(file => item == (path + file.FullName)) == 0)
                {
                    File file = new File()
                    {
                        Name = Path.GetFileNameWithoutExtension(item),
                        Extension = Path.GetExtension(item),
                        Length = new System.IO.FileInfo(item).Length
                    };
                    Curr.Files.Add(file);
                    if (file.Extension == ".txt")
                    {
                        await ReplaceInFile(item, "*******");
                    }
                }
            }
        }

        public async Task ReplaceInFile(string filePath, string replaceText)
        {
            await Task.Run(() =>
            {
                lock (this)
                {
                    if (SearchText == null)
                        return;

                    StringBuilder targetPath = new StringBuilder(@"E:\test\Replaced files");
                    StringBuilder sourcePath = new StringBuilder(Path.GetDirectoryName(filePath));

                    if (targetPath.Equals(sourcePath))
                        return;

                    StringBuilder sourceFile = new StringBuilder(System.IO.Path.Combine(sourcePath.ToString(),
                        filePath));
                    StringBuilder destFile = new StringBuilder(System.IO.Path.Combine(targetPath.ToString(),
                        Path.GetFileName(filePath)));

                    sourcePath.Clear();
                    targetPath.Clear();

                    string content;

                    using (StreamReader reader = new StreamReader(sourceFile.ToString()))
                    {
                        content = reader.ReadToEnd();
                    }

                    if (content.Contains(SearchText))
                    {
                        System.IO.File.Copy(sourceFile.ToString(), destFile.ToString(), true);
                    }
                    else
                        return;

                    content = Regex.Replace(content, SearchText, replaceText);

                    using (StreamWriter writer = new StreamWriter(destFile.ToString()))
                    {
                        writer.Write(content);
                    }
                    
                    sourceFile.Clear();
                    destFile.Clear();
                }
            });
        }

        public void print(Catalog path = null, string pref = " ")
        {
            if (path == null) path = this;

            foreach (var item in path.Catalogs)
            {
                try
                {
                    Console.WriteLine(pref + (item.path));
                    print(item, pref + "\t");
                }
                catch { }
            }

            foreach (var file in path.Files)
                Console.WriteLine(pref + file.Name + file.Extension);

        }
    }
}
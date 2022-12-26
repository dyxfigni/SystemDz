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
            {
                this.path = path;
                this.SearchText = null;
            }
            else
                throw new Exception("Invalid argument");
        }
        public Catalog(string path, string searchText)
        {
            if (Directory.Exists(path))
            {
                this.path = path;
                this.SearchText= searchText;
            }
            else
                throw new Exception("Invalid argument");
        }

        public Catalog() { }

        //
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
                        Catalog newDir = new Catalog(catalog, SearchText);
                        Curr.Catalogs.Add(newDir);
                        await ReadAllChildren(newDir);
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
                        await ReplaceInFile(item, "*******", SearchText);
                    }
                }
            }
        }


        public async Task ReplaceInFile(string filePath, string replaceText, string searchText)
        {
            //creating a new task for replacing documents with these banned words
            await Task.Run(() =>
            {
                lock (this)
                {
                    if (searchText == null)
                        return;

                    StringBuilder targetPath = new StringBuilder(@"D:\test\FileDetector\Replaced files");
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

        public void Print(Catalog path = null, string pref = " ")
        {
            if (path == null) path = this;

            foreach (var item in path.Catalogs)
            {
                try
                {
                    Console.WriteLine(pref + (item.path));
                    Print(item, pref + "\t");
                }
                catch { }
            }

            foreach (var file in path.Files)
                Console.WriteLine(pref + file.Name + file.Extension);
        }
    }
}
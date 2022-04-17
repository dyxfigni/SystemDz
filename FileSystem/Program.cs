using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string rootpath = @"E:\test";

            //string[] dirs = Directory.GetDirectories(rootpath, "*",
            //    SearchOption.AllDirectories);
            //foreach (string dir in dirs)
            //{
            //    Console.WriteLine(dir);
            //}

            //var files = Directory.GetFiles(rootpath, "*",
            //    SearchOption.AllDirectories);
            //foreach (string file in files)
            //{
            //    // Console.WriteLine(Path.GetFileName(file));
            //    ////Console.WriteLine(Path.GetFileNameWithoutExtension(file));
            //    // Console.WriteLine(Path.GetFullPath(file));

            //     var info = new FileInfo(file);

            //    // Размер файла в байтах
            //    //Console.WriteLine(info.Length);

            //    Console.WriteLine($"{Path.GetFileName(file)}: " +
            //                      $"{info.Length} bytes");
            //}

            string[] files = Directory.GetFiles(rootpath);
            string distinationFolder = @"E:\test\SubFolderA";

            //foreach (string file in files)
            //{
            //    File.Copy(file, distinationFolder, true);
            //}

            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i], distinationFolder, true);
            }

            Console.ReadLine();
        }
    }
}

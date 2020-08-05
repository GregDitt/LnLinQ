using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Linq1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\windows";
            ShowLargFileWithoutLinq(path);

            Console.WriteLine("*******************************");
            ShowLargeFilesWithLinq(path);
        }

        private static void ShowLargeFilesWithLinq(string path)
        {
            //var query = from file in new DirectoryInfo(path).GetFiles()
            //            orderby file.Length descending
            //            select file;


            var query = new DirectoryInfo(path).GetFiles()
                        .OrderByDescending(f => f.Length)
                        .Take(5);

            foreach (var file in query/*.Take(5)*/)
            {
                Console.WriteLine($"{file.Name,-20} : {file.Length,10:N0}");
            }
        }

        private static void ShowLargFileWithoutLinq(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo [] files = directory.GetFiles();

            Array.Sort(files, new FileInfoComparer());

            
            for(int i = 0; i<5; i++)
            {
                FileInfo file = files[i];
                Console.WriteLine($"{file.Name, - 20} : {file.Length, 10:N0}");
            
            }



                //foreach (FileInfo file in files)
                //{
                //    Console.WriteLine($"{file.Name} : {file.Length}");
                //}
            }
        }

    public class FileInfoComparer : IComparer<FileInfo>
    {
        public int Compare([AllowNull] FileInfo x, [AllowNull] FileInfo y)
        {
            return y.Length.CompareTo(x.Length);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirectoryFilesDisplay
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "../../../../";

            ShowLargeFilesWithoutLinq(path);

            Console.WriteLine("---");

            ShowFilesWithLinq(path);
        }

        public static void ShowFilesWithLinq(string path)
        {
            var query = new DirectoryInfo(path).GetFiles()
                                               .OrderByDescending(f => f.Length)
                                               .Take(5);

            foreach (var file in query)
            {
                Console.WriteLine($"{file.Name} : {file.Length}");
            }
        }

        public static void ShowLargeFilesWithoutLinq(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);

            FileInfo[] files = directory.GetFiles();

            Array.Sort(files, new FileInfoComparer());

            for (int i = 0; i < 5; i++)
            {
                FileInfo file = files[i];

                Console.WriteLine($"{file.Name} : {file.Length}");
            }
        }
    }

    public class FileInfoComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            return y.Length.CompareTo(x.Length);
        }
    }
}

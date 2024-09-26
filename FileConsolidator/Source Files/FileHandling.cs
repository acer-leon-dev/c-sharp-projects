using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FileConsolidator
{
    static class FileHandling
    {
        public static void AddSelectedFiles(OpenFileDialog dialog, ref string[] array)
        {
            List<string> temp = new(array);
            string[] files;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                files = dialog.FileNames;
                temp.AddRange(files);
                array = [.. temp];
                var fileNames = from path in files select Path.GetFileName(path);
                Console.WriteLine($"Added {English.Sequence(fileNames.ToArray(), "and", true)}");
            }
            else ConsoleExt.WriteError("There was a problem getting the files.");
        }

        public static void AddFilesInDirectory(FolderBrowserDialog dialog, ref string[] array)
        {
            List<string> temp = new(array);
            string[] files;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                files = Directory.GetFiles(dialog.SelectedPath);
                temp.AddRange(files);
                array = [.. temp];
                var fileNames = from path in files select Path.GetFileName(path);
                Console.WriteLine($"Added {English.Sequence(fileNames.ToArray(), "and", true)}");
            }
            else
            {
                ConsoleExt.WriteError("There was a problem getting the files.");
            }
        }

        public static void AddFilesInSubdirectories(FolderBrowserDialog dialog, ref string[] array)
        {
            List<string> temp = new(array);
            string[] files;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                files = Directory.GetFiles(dialog.SelectedPath, "*", SearchOption.AllDirectories);
                temp.AddRange(files);
                array = [.. temp];
                var fileNames = from path in files select Path.GetFileName(path);
                Console.WriteLine($"Added {English.Sequence(fileNames.ToArray(), "and", true)}");
            }
            else
            {
                ConsoleExt.WriteError("There was a problem getting the files.");
            }
        }

        public static void SaveFile(SaveFileDialog dialog, ref string output)
        {
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                output = dialog.FileName;
                Console.WriteLine($"Saved \"{output}\"");
            } else
            {
                ConsoleExt.WriteError("There was a problem saving the file.");
            }
        }
    }
}

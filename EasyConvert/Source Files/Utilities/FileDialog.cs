using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Utilities
{
    static class FileDialog
    {
        public static string SaveFile(SaveFileDialog dialog)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            } else
            {
                ConsoleE.WriteError("There was a problem saving the file.");
                return "";
            }
        }

        public static string[] GetSelectedFiles(OpenFileDialog dialog)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileNames;
            }
            else
            {
                ConsoleE.WriteError("There was a problem saving the file.");
                return [];
            }
        }

        public static string[] GetFilesInDirectory(FolderBrowserDialog dialog)
        {
            string[] files;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return Directory.GetFiles(dialog.SelectedPath);
            }
            else
            {
                ConsoleE.WriteError("There was a problem getting the files.");
                return [];
            }
        }

        public static string[] GetFilesInSubdirectories(FolderBrowserDialog dialog)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return Directory.GetFiles(dialog.SelectedPath, "*", SearchOption.AllDirectories);
            }
            else
            {
                ConsoleE.WriteError("There was a problem getting the files.");
                return [];
            }
        }

        public static string GetDirectory(FolderBrowserDialog dialog)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.SelectedPath;
            }
            else
            {
                ConsoleE.WriteError("There was a problem getting the files.");
                return "";
            }
        }
    }
}

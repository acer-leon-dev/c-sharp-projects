using Utilities;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFmpeg_EasyConvert
{

    internal class Program
    {
        static Program()
        {
            Types[Image] = [".bmp", ".dib", ".eps", ".gif", ".jpeg", ".jpg", ".jpg_large",
                ".pcd", ".pcx", ".png", ".ras", ".tga", ".tif", ".wmf"];
            Types[Video] = [".avi", ".gif", ".mov", ".mp4", ".mpeg", ".mpg", ".webm"];
            Types[Audio] = [".aac", ".aiff", ".alac", ".au", ".flac", ".mp3", ".ogg", ".ra", ".ram", ".wav", ".wma"];
        }

        public static string Image { get; } = "Image Files";
        public static string Video { get; } = "Video Files";
        public static string Audio { get; } = "Audio Files";

        public static Dictionary<string, string[]> Types { get; } = [];

        [STAThread]
        static void Main(string[] args)
        {
            /* Get user input */
            string order = "1234567890abcdefghijklmnopqrstuvwxyz";

            Console.WriteLine("What kind of files would you like to input?");
            Prompts.ShowOptions([.. Types.Keys], order);
            string selectedKind = Prompts.LoopReadOptionKey([.. Types.Keys], order);

            Console.WriteLine("What format are you converting the files to?");
            string[] extensions = Types[selectedKind];
            Prompts.ShowOptions(extensions, order);
            string outExtension = Prompts.LoopReadOptionKey(extensions, order);

            /* Get multiple input (-i) files with Windows Forms File Dialog */
            
            string filter(int? limit = null)
            {
                // Organizes a list of extesions into filters for FileDialogs
                return ';'.Join(extensions[..(limit ?? extensions.Length)].Select((str) => '*' + str));
            };
            Console.WriteLine("Select Input Files");
            OpenFileDialog inputOFDlg = new() // Set up dialog
            {
                Multiselect = true,
                Title = "Select Input Files",
                Filter = $"{selectedKind} ({filter(5)}...)|{filter()}"
            };
            string[] inputFiles = Utilities.FileDialog.GetSelectedFiles(inputOFDlg); // Get files

            foreach (string file in inputFiles)
                Console.WriteLine($"──── {Path.GetFileName(file)}");
            

            /* Get output directory (for all the converted files) with Windows Forms File Dialog */

            Console.WriteLine("Select Output Directory");
            FolderBrowserDialog outputFBDlg = new() // Set up dialog
            {
                Description = "Select Output Directory"
            };
            string outDirectory = Utilities.FileDialog.GetDirectory(outputFBDlg);
            Console.WriteLine($"─ Output directory path: {outDirectory}");

            /* Prepare paths to output files (files after conversion) */
            string[] outputFiles = inputFiles.Select((path) =>
            {
                return PathE.ChangeDirectory(outDirectory, Path.ChangeExtension(path, outExtension));
            }).ToArray();

            /* Convert the files */
            FFmpeg.Converter converter = new();
            converter.StartMultiple(inputFiles, outputFiles);
        }
    }
}

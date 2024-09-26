using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Utilities;

namespace FileConsolidator
{
	public static class SpecificFileTypes
	{
        public static string Text { get; } = ".txt";
        public static string CSharp { get; } = ".cs";
	}

	class Program
	{
        // Parses files without special considerations.
        public static string[] ParseRegular(string[] inputFiles, string outputFile)
        {
            List<string> lines = [];

            foreach (string path in inputFiles)
            {
                string[] content = File.ReadAllLines(path);
                lines.AddRange(content);
            }

            return [.. lines];
        }

        // Parses .cs files
        public static string[] ParseCSharp(string[] inputFiles, string outputFile)
        {
            List<string> lines = [];
            List<string> usingsStatements = [];

            foreach (string path in inputFiles)
            {
                string[] content = File.ReadAllLines(path);
                lines.AddRange(content);
            }
            
            foreach (string path in inputFiles)
            {
                string[] fileContents = File.ReadAllLines(path);
                foreach (string line in fileContents)
                    if (line.StartsWith("using"))
                        usingsStatements.Add(line);
            }

            List<string> parsedUsings = [];
            foreach (string line in lines.ToArray())
            {
                if (line.StartsWith("using"))
                {
                    foreach (string statement in usingsStatements)
                    {
                        if (line.Split(' ')[1] == statement.Split(' ')[1])
                        {
                            if (!parsedUsings.Contains(statement))
                            {
                                int end = statement.IndexOf('\\');
                                if (end == -1)
                                {
                                    end = statement.Length;
                                }
                                string newStatement = statement[..end];
                                parsedUsings.Add(newStatement);
                            }
                        }
                    }
                    lines.Remove(line);
                }
            }
            return [.. parsedUsings.Concat(lines)];
        }

        [STAThread]
		static void Main()
		{
			string[] inFiles = [];
			string outPath = String.Empty;

            // Dialogs for getting files, etc

            OpenFileDialog OPdlg = new()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "All files (*.*)|*.*",
                RestoreDirectory = true,
                Multiselect = true
            };

            FolderBrowserDialog FBdlg = new()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            SaveFileDialog SFdlg = new()
            {
                Filter = "All files (*.*)|*.*",
                RestoreDirectory = true
            };

            string[] options = 
				[
                "Show files",
                "Select files", 
				"Add files in directory", 
				"Add files in directory (including subdirectories)", 
				"Remove files",
				"Set location", 
				"Merge"
				];

            // Ask the user what to do
            while (true)
            {
                Prompts.ShowOptionsAlphanum(options);
                Prompts.LoopReadOptionKeyAlphanum(options, out string option);
                switch (option)
                {
					case "Show files":
                        Prompts.ShowValuesInOrder(inFiles, s => Path.GetFileName(s));
						continue;
                    case "Select files":
                        FileHandling.AddSelectedFiles(OPdlg, ref inFiles);
                        continue;
					case "Add files in directory":
						FileHandling.AddFilesInDirectory(FBdlg, ref inFiles);
                        continue;
                    case "Add files in directory (including subdirectories)":
                        FileHandling.AddFilesInSubdirectories(FBdlg, ref inFiles);
                        continue;
					case "Remove files":
						Prompts.PromptDeletion(ref inFiles, s => Path.GetFileName(s));
						continue;
                    case "Set location":
                        FileHandling.SaveFile(SFdlg, ref outPath);
                        continue;
                    case "Merge":
						if (outPath != String.Empty) 
							break;
						ConsoleExt.WriteError("Output file must be specified.");
						continue;
                    default:
                        ConsoleExt.WriteError("Invalid Option");
                        continue;
                }
                break;
            }

            // Parse Files (depending on type)
			Console.WriteLine($"Parsing and concatenating files...");
            string[] content;
            if (Path.GetExtension(outPath) == SpecificFileTypes.CSharp)
                content = ParseCSharp(inFiles, outPath);
            else
                content = ParseRegular(inFiles, outPath);
            Console.WriteLine($"Files parsed and concatenated successfully.");

			// Concatenate the using statements and other code
			Console.WriteLine($"Concatenating text...");
			Console.WriteLine($"Concatenated text successfully.");
			// Create File and Write the content
			Console.WriteLine($"Creating and writing to file \"{outPath}\"...");
			File.WriteAllLines(outPath, content);
			Console.WriteLine($"Created and wrote to file \"{outPath}\" successfully.");
		}
	}
}
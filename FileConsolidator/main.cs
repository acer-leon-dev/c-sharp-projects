using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace FileConsolidator
{
	class Program
	{
		static void Main(string[] args)
		{
			string outputFileName = "Program";
			string outputFilePath;
			if (Directory.Exists(args[0])) {
				outputFilePath = args[0] + '\\' + outputFileName;
			} else if (File.Exists(args[0])) {
				outputFilePath = args[0];
			} else if (args[0].EndsWith('\\') || args[0].EndsWith('/') ) {	
				Directory.CreateDirectory(args[0]);
				outputFilePath = args[0] + outputFileName;
			} else if (!args[0][1..].Contains('\\') && !args[0][1..].Contains('/')) {	
				if ( args[0].StartsWith('\\') || args[0].StartsWith('/') ) {
					outputFilePath = Directory.GetCurrentDirectory() + @"\" + args[0];
				} else {
					outputFilePath = Directory.GetCurrentDirectory() + @"" + args[0];
				}
			} else if (true) {
				Directory.CreateDirectory(Path.GetDirectoryName(args[0]));
				outputFilePath = args[0];
			} else {
				throw new Exception("Invalid Path Exception");
			}
			outputFilePath = Path.ChangeExtension(outputFilePath, ".cs");
			Console.WriteLine($"Output path: {outputFilePath}");

			// Parse input files
			Console.WriteLine($"Parsing files...");
			List<string> inputFilePaths = new(args[1..]);
			List<string> inputFileLines = [];
			List<string> usingStatements = [];
			
			foreach (string path in inputFilePaths)
			{
				string[] fileContent = File.ReadAllLines(path);
				inputFileLines.AddRange(fileContent);
				foreach (string inputFileLine in fileContent)
				{
					if (inputFileLine.StartsWith("using")) 
					{
						usingStatements.Add(inputFileLine);
					}
				}
			}

			// Parse using statements
			List<string> parsedUsingStatements = [];
			string[] inputLinesCopy = [.. inputFileLines];
			foreach (string line in inputLinesCopy)
			{
				if (line.StartsWith("using"))
				{
					foreach (string statement in usingStatements)
					{
						if (line.Split(' ')[1] == statement.Split(' ')[1])
						{
							if (!parsedUsingStatements.Contains(statement))
							{
								int end = statement.IndexOf('\\');
								if (end == -1){
									end = statement.Length;
								}
								string newStatement = statement[..end];
								parsedUsingStatements.Add(newStatement);
							}
						}	
					}
					inputFileLines.Remove(line);
				}
			}
			Console.WriteLine($"Files parsed.");

            // Concatenate the using statements and other code
			Console.WriteLine($"Concatenating text...");
			List<string> outputFileContent = new List<string>(parsedUsingStatements.Concat(inputFileLines));
			Console.WriteLine($"Concatenated text.");
            // Create File and Write the content
			Console.WriteLine($"Creating and writing to file \"{outputFilePath}\"...");
			File.WriteAllLines(outputFilePath, outputFileContent);
			Console.WriteLine($"Created and wrote to file \"{outputFilePath}\".");
		}
	}
}
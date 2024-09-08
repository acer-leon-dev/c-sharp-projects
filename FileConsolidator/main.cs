using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace FileConsolidator
{
	
class Program
{
	static string? ParseMod(string[] argumentList, string mod)
	{
		string? res = null;
		foreach (string arg in argumentList)
		{
			if (arg.StartsWith(mod)){
				res = arg.Split('=')[1];
				break;
			}
		}

		if (res == "")
			throw new Exception($"Exception: Modifer \"{mod[..mod.IndexOf('=')]}\" cannot be empty.");

		return res;
	}
	static string PathConditionInternal(string path)
	{
		char dirSep = Path.DirectorySeparatorChar;
		if (Directory.Exists(path))
			return "IsRealDirectory";
		else if (File.Exists(path))
			return "IsRealFile";
		else if (path.EndsWith(dirSep))
			return "IsFakeDirectory";
		else if ((!path.Contains(dirSep)) || (path.StartsWith(dirSep) && !path[1..].Contains(dirSep)))
			return "IsFileName";
		else 
			return "IsFakeFilePath";
	}
	
	static bool IsRealDirectory(string path) => nameof(IsRealDirectory).Equals(PathConditionInternal(path));
	static bool IsRealFile(string path) => nameof(IsRealFile).Equals(PathConditionInternal(path));
	static bool IsFakeDirectory(string path) => nameof(IsFakeDirectory).Equals(PathConditionInternal(path));
	static bool IsFileName(string path) => nameof(IsFileName).Equals(PathConditionInternal(path));
	static bool IsFakeFilePath(string path) => nameof(IsFakeFilePath).Equals(PathConditionInternal(path));
	
	static void Main(string[] args)
	{
		// Get input files
		string[] mods = [".ext=", ".extension=", ".o="];
		string[] filesToCombine = (from arg in args where !mods.Contains(arg[..(arg.IndexOf('=')+1)]) select arg).ToArray();
		
		// Set extension to out file
		string defaultFileExtension = ".txt";
		string fileExt = defaultFileExtension;
		foreach (string mod in new string[2] {".ext=", ".extension="})
		{
			fileExt = ParseMod(args, mod) ?? fileExt;
		}

		// Set path to output file
		string defaultFilePath = Directory.GetCurrentDirectory();
		string filePath = defaultFilePath;
		filePath = ParseMod(args, ".o=") ?? filePath;
		
		// Parse the output file path
		string defaultOutputFileName = "MergedFile";
		string outputFilePath;
		if (IsRealDirectory(filePath)) 
		{
			outputFilePath = Path.Join(filePath, defaultOutputFileName);
		} else if (IsRealFile(filePath)) 
		{
			outputFilePath = filePath;
		} else if (IsFakeDirectory(filePath)) 
		{	
			Console.WriteLine($"Creating Directory \"{filePath}\"...");
			Directory.CreateDirectory(filePath);
			Console.WriteLine($"Created Directory \"{filePath}\".");
			outputFilePath = Path.Join(filePath, defaultOutputFileName);
		} else if (IsFileName(filePath)) 
		{	
			outputFilePath = Path.Join(Directory.GetCurrentDirectory(), filePath);
		} else if (IsFakeFilePath(filePath)) {
			Console.WriteLine($"Creating Directory \"{Path.GetDirectoryName(filePath)}\"...");
			Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? defaultFilePath);
			Console.WriteLine($"Created Directory \"{Path.GetDirectoryName(filePath)}\".");
			outputFilePath = filePath;
		} else {
			throw new Exception($"Exception: Invalid path.");
		}
		
		try {
			outputFilePath = Path.ChangeExtension(outputFilePath, fileExt);
		} catch (Exception)
		{
			throw new Exception($"Exception: Invalid file extension.");
		}
		Console.WriteLine($"Output path: {outputFilePath}");

		// Combine input files
		Console.WriteLine($"Parsing files...");
		List<string> inputFilePaths = new(filesToCombine);
		List<string> inputFileLines = [];
		List<string> usingStatements = [];
		
		foreach (string path in inputFilePaths)
		{
			string[] fileContent;
			try 
			{
				fileContent = File.ReadAllLines(path);
			} catch (Exception) 
			{ 
				throw new Exception($"Exception: \"{path}\" is not a valid path."); 
			}
			inputFileLines.AddRange(fileContent);
			if (fileExt == ".cs" || fileExt == "cs"){
				foreach (string inputFileLine in fileContent)
				{
					if (inputFileLine.StartsWith("using")) 
					{
						usingStatements.Add(inputFileLine);
					}
				}
			}
		}
	
		// Parse using statements (if extension is ".cs")
		List<string> parsedUsingStatements = [];
		if (fileExt == ".cs" || fileExt == "cs"){
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
		}
		Console.WriteLine($"Files parsed.");

		// Concatenate the using statements and other code
		Console.WriteLine($"Concatenating text...");
		List<string> outputFileContent = new(parsedUsingStatements.Concat(inputFileLines));
		Console.WriteLine($"Concatenated text.");
		// Create File and Write the content
		Console.WriteLine($"Creating and writing to file \"{outputFilePath}\"...");
		File.WriteAllLines(outputFilePath, outputFileContent);
		Console.WriteLine($"Created and wrote to file \"{outputFilePath}\".");
	}
}
}
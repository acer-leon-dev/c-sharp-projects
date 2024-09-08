A command-line program that takes an output path and list of text-based files, and combines the files into a single, output file of a specified file extension. 

All of the arguments must be paths to the files that are to be merged/consolidated into the output file. An exception is thrown if any of the files/directories are invalid, inaccessible, etc.

The *".o="* modifer must be a string to the output file path. It can be a path to an existing/real directory (as *MergedFile.extension*), an existing file, a new directory (as *MergedFile.extension*), a new file in a new directory, or a file name (automatically placed in current working directory). The default value is the path to the current working directory. 

The *".extension="*, *".ext="* modifer must be a string that represents a valid file extension. It can be prepended with or without a period (*".txt"*, *"txt"*). The default value is *".txt"*.

Designed for Windows with .NET 8.0.401. Untested on macOS and Linux.
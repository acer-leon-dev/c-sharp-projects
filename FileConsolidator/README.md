A command-line program that takes an output path and list of .cs files, and combines the .cs files into a single, output .cs file. 

The 0th argument is the output file path. It can be a path to an existing directory (as Program.cs), an existing file, a new directory (as Program.cs), a new file in a new directory, or a file name (automatically placed in current working directory). The following arguments (1, 2..) must be paths to the files that are to be merged/consolidated into the output file. An exception is thrown if any of the files/directories are invalid, inaccessible, etc.

Designed for Windows. Untesed on Mac and linux.
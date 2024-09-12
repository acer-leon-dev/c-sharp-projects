### Debugging
```
dotnet run -r win-x86 -f net8.0
```

### Publishing
Windows:
```ps
echo "Publishing"
dotnet publish -r win-x86 -f net8.0 -o -c Release "./Release" 
mkdir "./Release/Documents"
copy "./Documents/help-page.txt" "./Release/Documents/help-page.txt"
del "./Release/PayrollManager.pdb"
echo "Publishing complete"
 
```
For macOS and Linux, change the "-r" option to "osx-x86" and "linux-x86", respectively. Change "x86" to "x64" for 64 bit.

using PayrollManager;
using Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace PayrollManager
{
class Program
{
    static void Main(string[] args)
    {
        Terminal terminal = new("Payroll Management");
        terminal.Run();
    }
}
}

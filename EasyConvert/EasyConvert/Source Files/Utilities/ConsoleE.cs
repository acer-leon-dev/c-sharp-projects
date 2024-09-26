using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class ConsoleE
    {
        public static void WriteWithColor(string? value, ConsoleColor color)
        {
            ConsoleColor original = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ForegroundColor = original;
        }

        public static void WriteLineWithColor(string? value, ConsoleColor color)
        {
            ConsoleColor original = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = original;
        }

        public static void WriteError(string message)
        {
            WriteLineWithColor("Error: " + message, ConsoleColor.Red);
        }
    }
}

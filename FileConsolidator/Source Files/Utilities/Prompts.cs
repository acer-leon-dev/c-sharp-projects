using System.Collections.Generic;

namespace Utilities
{
    public static class Prompts
    {
        // Selecting Options

        public static void ShowOptionsAlphanum<T>(T[] options)
        {
            string order = "1234567890abcedfghijklmnopqrstuvwxyz";
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{order[i]}. {options[i]}");
            }
        }

        public static void ShowOptionsAlphanum<T>(T[] options, Func<T, string> repr)
        {
            string order = "1234567890abcedfghijklmnopqrstuvwxyz";
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{order[i]}. {repr(options[i])}");
            }
        }

        public static T? GetOptionAlphanum<T>(char key, T[] options)
        {
            string order = "1234567890abcedfghijklmnopqrstuvwxyz"[..options.Length];
            int index = order.IndexOf(key);
            if (index != -1)
                return options[index];
            else
                return default;
        }

        public static void LoopReadOptionKeyAlphanum<T>(T[] options, out T value)
        {
            while (true)
            {
                Console.Write("> ");
                char key = Console.ReadKey().KeyChar;
                Console.WriteLine();
                T? temp = GetOptionAlphanum(key, options);
                if (temp == null)
                {
                    ConsoleExt.WriteError("Invalid Key");
                    continue;
                }
                value = temp;
                break;
            }
        }

        // Other

        public static void ShowValuesInOrder<T>(T[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {values[i]}");
            }
        }

        public static void ShowValuesInOrder<T>(T[] values, Func<T, string> repr)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {repr(values[i])}");
            }
        }

        public static void PromptDeletion<T>(ref T[] values, Func<T, string> repr)
        {
            List<T> tempValues = new(values);

            ShowValuesInOrder([.. values], repr);

            Console.Write("Values to delete> ");
            string input = Console.ReadLine() ?? "";

            string[] deletions = input.Split(',');
            List<int> indicesToDelete = [];
            try
            {
                foreach (string value in deletions)
                {
                    if (value.Count(s => s == '-') == 1)
                    {
                        int[] ends = value.Split('-').Select(n => Convert.ToInt32(n) - 1).ToArray();
                        indicesToDelete.AddRange(Enumerable.Range(ends[0], ends[1] - ends[0] + 1));
                    }
                    else
                    {
                        indicesToDelete.Add(Convert.ToInt32(value) - 1);
                    }
                }
                indicesToDelete.Sort();
                indicesToDelete.Reverse();
                foreach (int index in indicesToDelete)
                {
                    Console.Write(index + " ");
                    tempValues.RemoveAt(index);
                }
                values = [.. tempValues];
            }   
            catch (Exception)
            {
                ConsoleExt.WriteError("There was a problem removing the values.");
            }
        }
    }
}

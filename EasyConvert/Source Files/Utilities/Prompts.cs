using System.Collections.Generic;

namespace Utilities
{
    public static class Prompts
    {
        // Selecting Options
        
        public static void ShowOptions<T>(T[] options, string order)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{order[i]}. {options[i]}");
            }
        }

        public static void ShowOptions<T>(T[] options, Func<T, string> repr, string order)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{order[i]}. {repr(options[i])}");
            }
        }

        public static void ShowOptions<T>(T[] options, string[] order)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{order[i]}. {options[i]}");
            }
        }

        public static void ShowOptions<T>(T[] options, Func<T, string> repr, string[] order)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{order[i]}. {repr(options[i])}");
            }
        }

        public static T? GetOption<T>(char key, T[] options, string order)
        {
            order = order[..options.Length];
            int index = order.IndexOf(key);
            if (index != -1)
                return options[index];
            else
                return default;
        }

        public static T? GetOption<T>(string value, T[] options, string[] order)
        {
            order = order[..options.Length];
            int index = Array.IndexOf(order, value);
            if (index != -1)
                return options[index];
            else
                return default;
        }

        public static T LoopReadOptionKey<T>(T[] options, string order)
        {
            while (true)
            {
                Console.Write("> ");
                char key = Console.ReadKey().KeyChar;
                Console.WriteLine();
                T? temp = GetOption(key, options, order);
                if (temp == null)
                {
                    ConsoleE.WriteError("Invalid Key");
                    continue;
                }
                return temp;
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
                ConsoleE.WriteError("There was a problem removing the values.");
            }
        }
    }
}

using Utilities;

namespace PayrollManager
{
    
internal class Terminal
{
    public Terminal(string title) { Console.Title = title; }
    
    private bool looping;

    static private string[] ParseArguments(string str)
    {
        List<string> arguments = [];
        string word = "";
        bool quoted = false;
        foreach (char letter in str.Trim() + " ")
        {
            if (!quoted)
            {
                if (letter == '"') { quoted = true;  }
                else if (letter != ' ') 
                { 
                    word += letter;
                } 
                else {
                    arguments.Add(word);
                    word = "";
                }
            } else
            {
                if (letter == '"') { quoted = false; }
                else { word += letter; } 
            }
        }
        return [.. arguments];
    }

    static void Help(params string[] args)
    {
        Console.WriteLine
            (
                File.ReadAllText(Path.Join("Documents", "help-page.txt"))
            );
    }
    
    static void ChangeConsoleForegroundColor(params string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Error: Minimum 1 arguments, Maximum 2. ( consolecolor [System.ConsoleColor] )");
            return;
        }
        try
        {
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), args[0], true);
            Console.Clear();
        }
        catch (ArgumentException)
        {
            Console.WriteLine($"Error: \"{args[0]}\" is not a valid System.ConsoleColor.");
            return;
        }
    }

    static void ChangeConsoleBackgroundColor(params string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Error: Minimum 1 arguments, Maximum 2. ( consolecolor [System.ConsoleColor] )");
            return;
        }
        try
        {
            Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), args[1], true);
            Console.Clear();
        }
        catch (ArgumentException)
        {
            Console.WriteLine($"Error: \"{args[1]}\" is not a valid color. (System.ConsoleColor)");
            return;
        }
    }

    static Workday? GetDayData(params string[] args)
    {
        if (args.Length != 2)
        { 
            Console.WriteLine("Error: Minimum 1 arguments, Maximum 2. ( \"{nameof(GetDayData)}\" [double]) "); return null; 
        }

        DayType weekday;
        try 
        { 
            weekday = (DayType)Enum.Parse(typeof(DayType), args[0], true);
        } catch (ArgumentException) 
        {
            Console.WriteLine($"Error: Argument 1 is not a valid DayType."); return null;
        }

        double hours;
        try 
        { 
            hours = Convert.ToDouble(args[1]); 
        }
        catch (FormatException) { Console.WriteLine($"Error: Argument 1 must be of type double."); return null; }
        if (hours > 24)
        {
            Console.WriteLine($"Error: Argument 2 can not be greater than 24.00.");  return null;
        }

        Workday result = new(weekday, hours);
        Console.WriteLine(result.ToString());
        return result;
    }

    static WorkWeek? GetWeekData(params string[] args)
    {
        if (args.Length < 2 || args.Length > 9)
        {
            Console.WriteLine("Error: Minimum 2 arguments, Maximum 9 ( \"{nameof(GetWeekData)}\" [double] [double]...) ");
            return null;
        }

        DayType startingDay;
        try
        {
            startingDay = (DayType)Enum.Parse(typeof(DayType), args[0], true);
        }
        catch (ArgumentException)
        {
            Console.WriteLine($"Error: Argument 1 is not a valid DayType.");
            return null;
        } 

        double[] weekHours = new double[7];
        for (int i = 0; i < 7; i++)
        {
            double hours;
            try
            {
                hours = Convert.ToDouble(args[i + 1]);
            } catch (FormatException)
            {
                Console.WriteLine($"Error: Argument 2...9 must be of type double");
                return null;
            }  catch (IndexOutOfRangeException)
            {
                hours = 0;
            }
            if (hours > 24)
            {
                Console.WriteLine($"Error: Arguments 2...9 can not be greater than 24.00");
                return null;
            }

            weekHours[i] = hours;
        }
        WorkWeek result = new(startingDay, [.. weekHours]);
        Console.WriteLine($"regularHoursWorked  = {result.RegularHours} hours");
        Console.WriteLine($"overtimeHoursWorked = {result.OvertimeHours} hours");
        Console.WriteLine(result.ToString());
        return result;
    }

    static double? GetTimeDifference(params string[] args)
    {
        if (args.Length < 2) 
        { Console.WriteLine("Error: Minimum 2 arguments"); return null; }
        if (args.Length % 2 != 0) 
        { Console.WriteLine("Error: Argument count must be even"); return null; }

        string[] format = ["hh mm tt", "hh mm t", "h mm tt", "h mm t",
                            "h m tt", "h m t", "hh mm ss tt", "hh mm ss t",
                            "hh:mm tt", "hh:mm t", "h:mm tt", "h:mm t",
                            "h:m tt", "h:m t", "hh:mm:ss tt", "hh:mm:ss t",
                            "hh mmtt", "hh mmt", "h mmtt", "h mmt",
                            "h mtt", "h mt", "hh mm sstt", "hh mm sst",
                            "hh:mmtt", "hh:mmt", "h:mmtt", "h:mmt",
                            "h:mtt", "h:mt",  "hh:mm:sstt", "hh:mm:sst",
                            "h tt", "htt", "hh tt", "hhtt"];
        double hoursWorked = 0;
        for (int i = 0; i < args.Length - 1; i += 2)
        {
            TimeOnly inTime;
            TimeOnly outTime;
            try { 
                inTime = TimeOnly.ParseExact(args[i], format).RoundToMinutes(15); 
            } 
            catch (FormatException) {
                 Console.WriteLine($"Error: {args[i]} is not a valid time format"); return null; 
            }
            try {
                 outTime = TimeOnly.ParseExact(args[i + 1], format).RoundToMinutes(15); 
            } 
            catch (FormatException) {
                 Console.WriteLine($"Error: {args[i + 1]} is not a valid time format"); return null; 
            }
            TimeSpan timeDifference = outTime - inTime;
            hoursWorked += timeDifference.Hours;
            hoursWorked += (double)timeDifference.Minutes / 60;
        }
        Console.WriteLine($"Total hours: {hoursWorked}");
        return hoursWorked;
    }

    private VariableHandler variableHandler = [];

    private void CreateNewVariable(params string[] args)
    {
        string name = args[0];
        string command = args[1];
        string[] newArgs = args[2..];

        object? value = command.ToLower() switch
        {
            "time" or 
            "timedifference"  =>  GetTimeDifference(newArgs),
            "day"             =>  GetDayData(newArgs),
            "week"            =>  GetWeekData(newArgs),
            _                 =>  null
        };
        if (value == null)
        {
            Console.WriteLine("Error: Unknown Type");
            return;
        }
        variableHandler.Add(name, value);
        variableHandler.ShowVariables();
    }
    
    private void EditVariable(params string[] args)
    {
        string name = args[0];
        string command = args[1];
        string[] newArgs = args[2..];

        object? value = command.ToLower() switch
        {
            "time" or 
            "timedifference"  =>  GetTimeDifference(newArgs),
            "day"             =>  GetDayData(newArgs),
            "week"            =>  GetWeekData(newArgs),
            _                 =>  null
        };
        if (value == null)
        {
            Console.WriteLine("Error: Unknown Type.");
            return;
        }
        if (variableHandler.ContainsKey(name))
        {
            Console.WriteLine("Error: Variable {name} already exists.");
            return;
        }
        variableHandler[name] = value;
        variableHandler.ShowVariables();
    }

    private void DeleteVariable(params string[] args)
    {
        string name = args[0];
        variableHandler.Delete(name);
        variableHandler.ShowVariables();
    }

    public void RunCommand(params string[] input)
    {

        string command = input[0].ToLower();
        string[] args = input[1..];
        switch (command)
        {
            // Help
            case    "help" or 
                    "commands" or 
                    "cmds" or 
                    "cmd":                  Help(args); 
                                            break;
            // Variables
            case    "var":                  CreateNewVariable(args); 
                                            break;
            case    "edit":                 EditVariable(args); 
                                            break;
            case    "vars":                 DeleteVariable(args); 
                                            break;
            // Calculators
            case    "day":                  GetDayData(args); 
                                            break;
            case    "week":                 GetWeekData(args); 
                                            break;
            case    "timedifference" or 
                    "time":                 GetTimeDifference(args); 
                                            break;
            // Miscellaneous
            case    "exit":                 looping = false; 
                                            break;
            case    "fg" or 
                    "foregroundcolor":      ChangeConsoleForegroundColor(args); 
                                            break;
            case    "bg" or 
                    "backgroundcolor":      ChangeConsoleBackgroundColor(args); 
                                            break;
            case    "beep":                 Console.Beep(); 
                                            break;
            // Error
            default:                        Console.WriteLine("Error: Unknown Command"); break;
        }
    }

    public void Run()
    {
        Console.WriteLine("Hello Program\n");
        string input;
        string[] arguments;
        looping = true;
        while (looping)
        {
            Console.Write(":");
            input = Console.ReadLine() ?? "";
            arguments = ParseArguments(input);
            RunCommand(arguments);
        }
    }
}
}

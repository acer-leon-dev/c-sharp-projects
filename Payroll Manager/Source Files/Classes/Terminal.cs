using Utilities;

namespace PayrollManager
{
    
internal class Terminal
{
    public Terminal(string title) 
    { 
        Console.Title = title; 
    }
    
    private bool looping;

    static void PrintError(string message)
    {
        ConsoleColor original = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: " + message);
        Console.ForegroundColor = original;
    }

    static void Help()
    {
        Console.WriteLine
        (
            File.ReadAllText(Path.Join("Documents", "help-page.txt"))
        );
    }
    
    static void ChangeConsoleForegroundColor(params string[] args)
    {
        int minimumArguments = 1;
        int maximumArguments = 1;
        if (args.Length < minimumArguments || args.Length > maximumArguments)
        {
            PrintError($"Minimum {minimumArguments} arguments, Maximum {maximumArguments} arguments.");
            return;
        }
        
        if (!Enum.TryParse(args[1], true, out ConsoleColor color))
        {
            PrintError("\"{args[1]}\" is not a valid color. (System.ConsoleColor)");
            return;
        }

        Console.ForegroundColor = color;
        Console.Clear();
    }

    static void ChangeConsoleBackgroundColor(params string[] args)
    {
        int minimumArguments = 2;
        int maximumArguments = 2;
        if (args.Length < minimumArguments || args.Length > maximumArguments)
        {
            PrintError($"Minimum {minimumArguments} arguments, Maximum {maximumArguments} arguments.");
            return;
        }

        if (!Enum.TryParse(args[1], true, out ConsoleColor color))
        {
            PrintError("\"{args[1]}\" is not a valid color. (System.ConsoleColor)");
            return;
        }

        Console.BackgroundColor = color;
        Console.Clear();
    }

    static void GetDay(params string[] args)
    {
        int minimumArguments = 2;
        int maximumArguments = 2;
        if (args.Length < minimumArguments || args.Length > maximumArguments)
        {
            PrintError($"Minimum {minimumArguments} arguments, Maximum {maximumArguments} arguments.");
            return;
        }

        if (!Enum.TryParse(args[0], true, out DayType weekday))
        {
            PrintError("Argument 1 is not a valid DayType.");
            return;
        }
        
        if (!decimal.TryParse(args[1], out decimal hours))
        {
            PrintError("Argument 2 must be of type decimal.");
            return;
        }

        if (hours > 24)
        {
            PrintError("Argument 2 can not be greater than 24.00.");
            return;
        }

        Workday day = new(weekday, hours);
        Console.WriteLine(day.ToString());
        return;
    }

    static void GetWeek(params string[] args)
    {
        int minimumArguments = 1;
        int maximumArguments = 8;
        if (args.Length < minimumArguments || args.Length > maximumArguments)
        {
            PrintError($"Minimum {minimumArguments} arguments, Maximum {maximumArguments} arguments.");
            return;
        }
        
        if (!Enum.TryParse(args[0], true, out DayType startingDay))
        {
            PrintError("Argument 1 is not a valid DayType.");
            return;
        }

        decimal[] weekHours = new decimal[7];
        for (int i = 0; i < 7; i++)
        {
            decimal hours;
            try
            {
                if (!decimal.TryParse(args[i + 1], out hours))
                {
                    PrintError("Argument 2...9 must be of type decimal");
                    return;
                }
            }  catch (IndexOutOfRangeException)
            {
                hours = 0;
            }

            if (hours > 24)
            {
                PrintError("Arguments 2...9 can not be greater than 24.00");
                return;
            }

            weekHours[i] = hours;
        }
        WorkWeek result = new(startingDay, [.. weekHours]);
        Console.WriteLine($"regularHoursWorked  = {result.RegularHours} hours");
        Console.WriteLine($"overtimeHoursWorked = {result.OvertimeHours} hours");
        Console.WriteLine(result.ToString());
        return;
    }

    static void GetTimeDifference(params string[] args)
    {
        int minimumArguments = 2;
        int maximumArguments = int.MaxValue;
        if (args.Length < minimumArguments || args.Length > maximumArguments)
        {
            PrintError($"Minimum {minimumArguments} arguments, Maximum {maximumArguments} arguments.");
            return;
        }
        
        if (args.Length % 2 != 0) 
        { 
            PrintError("Argument count must be even"); 
            return; 
        }

        string[] format = ["hh mm tt", "hh mm t", "h mm tt", "h mm t",
                            "h m tt", "h m t", "hh mm ss tt", "hh mm ss t",
                            "hh:mm tt", "hh:mm t", "h:mm tt", "h:mm t",
                            "h:m tt", "h:m t", "hh:mm:ss tt", "hh:mm:ss t",
                            "hh mmtt", "hh mmt", "h mmtt", "h mmt",
                            "h mtt", "h mt", "hh mm sstt", "hh mm sst",
                            "hh:mmtt", "hh:mmt", "h:mmtt", "h:mmt",
                            "h:mtt", "h:mt",  "hh:mm:sstt", "hh:mm:sst",
                            "h tt", "htt", "hh tt", "hhtt"];
        decimal hoursWorked = 0;
        for (int i = 0; i < args.Length - 1; i += 2)
        {
            if (!TimeOnly.TryParseExact(args[i], format, out TimeOnly inTime))
            {
                PrintError($"{args[i]} is not a valid time format"); 
                return; 
            }

            if (!TimeOnly.TryParseExact(args[i + 1], format, out TimeOnly outTime))
            {
                PrintError($"{args[i + 1]} is not a valid time format"); 
                return; 
            } 
            
            TimeSpan timeDifference = outTime.RoundToMinutes(15) - inTime.RoundToMinutes(15);
            hoursWorked += timeDifference.Hours;
            hoursWorked += timeDifference.Minutes / 60;
        }
        Console.WriteLine($"Total hours: {hoursWorked}");
        return;
    }

    public void MatchCommand(params string[] input)
    {
        string command = input[0].ToLower();
        string[] args = input[1..];
        switch (command)
        {
            // Help
            case    "help" or 
                    "commands" or 
                    "cmds" or 
                    "cmd":                  Help(); 
                                            break;
            // Calculators
            case    "day":                  GetDay(args); 
                                            break;
            case    "week":                 GetWeek(args); 
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
            default:                        PrintError("Unknown Command"); break;
        }
    }

    public void Run()
    {
        Console.WriteLine("Hello Program\n");
        looping = true;
        while (looping)
        {
            Console.Write(":");
            string input = Console.ReadLine() ?? "";
            MatchCommand(Parsing.ParseCommands(input));
        }
    }
}
}

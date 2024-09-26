using System.Runtime.Serialization;

namespace PayrollManager
{
class WorkWeek
{
    public WorkWeek(DayType startingDay, params decimal[] hours)
    {
        Days = new Workday[7];

        for (int i = 0; i < 7; i++)
        {
            int weekdayNumber = (int)startingDay + i;
            if (weekdayNumber > 6) 
                weekdayNumber = i - 7 + (int)startingDay; 
            
            Days[i] = new Workday((DayType)weekdayNumber, hours[i]); 
        }

        decimal week_regular             = 0.00m;
        decimal week_overtime_under_40   = 0.00m;
        decimal week_overtime_over_40    = 0.00m;

        decimal interval = 0.25m;
        foreach (var day in Days)
        {
            decimal hours_passed_in_day      = 0.00m;
            decimal regular_in_day           = 0.00m;
            decimal day_overtime_under_40    = 0.00m;
            decimal day_overtime_over_40     = 0.00m;
            for (decimal i = 0.00m; i < 24.00m; i += interval)
            {
                if (hours_passed_in_day < day.TotalHours)
                {
                    if (week_regular < 40.00m)
                    {
                        if ( hours_passed_in_day < 8.00m)
                        {
                            regular_in_day  += interval;
                            week_regular    += interval;
                        }
                        else if ( hours_passed_in_day >= 8.00m)
                        {
                            day_overtime_under_40   += interval;
                            week_overtime_under_40  += interval;
                        }
                        hours_passed_in_day += interval;
                    }
                    else if (week_regular >= 40.00m)
                    {
                        hours_passed_in_day     += interval;
                        day_overtime_over_40    += interval;
                        week_overtime_over_40   += interval;
                        day.RegularHours        = regular_in_day;
                        day.OvertimeHours       = day_overtime_over_40;
                    }
                }
            }
        }
        OvertimeHours += week_overtime_under_40;
        OvertimeHours += week_overtime_over_40;
        if (week_regular <= 40.00m)
        {
            RegularHours = week_regular;
        }
        else if (week_regular > 40.00m)
        {
            RegularHours    = 40.00m;
            OvertimeHours   += week_regular - 40.00m;
        }
    }

    public Workday[] Days { get; }

    public decimal RegularHours { get; }
    
    public decimal OvertimeHours { get; }

    public override string ToString()
    {
        IEnumerable<string> middleDays = from day in Days[1..6]
                                        
                                         select $"{char.ToLower((Enum.GetName(day.Weekday) ?? throw new Exception(""))[0])}" +
                                                $": {day.TotalHours}";
        return $"{Enum.GetName(Days[0].Weekday)} ({Days[0].TotalHours} hours)" +
                $", {string.Join(", ", middleDays)}" + 
                $", {Enum.GetName(Days[6].Weekday)} ({Days[6].TotalHours} hours)";
    }
}
}

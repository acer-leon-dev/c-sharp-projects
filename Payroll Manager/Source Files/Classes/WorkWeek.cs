using System.Runtime.Serialization;

namespace PayrollManager
{
class WorkWeek
{
    public WorkWeek(DayType startingDay, params double[] hours)
    {
        Days = new Workday[7];

        for (int i = 0; i < 7; i++)
        {
            int weekdayNumber = (int)startingDay + i;
            if (weekdayNumber > 6) 
                weekdayNumber = i - 7 + (int)startingDay; 
            
            Days[i] = new Workday((DayType)weekdayNumber, hours[i]); 
        }

        double week_regular             = 0;
        double week_overtime_under_40   = 0;
        double week_overtime_over_40    = 0;

        float interval = 0.25f;
        foreach (var day in Days)
        {
            double hours_passed_in_day      = 0;
            double regular_in_day           = 0;
            double day_overtime_under_40    = 0;
            double day_overtime_over_40     = 0;
            for (float i = 0f; i < 24f; i += interval)
            {
                if (hours_passed_in_day < day.TotalHours)
                {
                    if (week_regular < 40)
                    {
                        if ( hours_passed_in_day < 8)
                        {
                            regular_in_day  += interval;
                            week_regular    += interval;
                        }
                        else if ( hours_passed_in_day >= 8)
                        {
                            day_overtime_under_40   += interval;
                            week_overtime_under_40  += interval;
                        }
                        hours_passed_in_day += interval;
                    }
                    else if (week_regular >= 40)
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
        if (week_regular <= 40)
        {
            RegularHours = week_regular;
        }
        else if (week_regular > 40)
        {
            RegularHours    = 40;
            OvertimeHours   += week_regular - 40;
        }
    }

    public Workday[] Days { get; }

    public double RegularHours { get; }
    
    public double OvertimeHours { get; }

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

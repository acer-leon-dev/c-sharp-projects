using PayrollManager;

namespace PayrollManager
{
class Workday
{
    public Workday(DayType weekday, decimal hours)
    {
        Weekday = weekday;
        RecalculateHours(hours);
    }

    public Workday(decimal hours)
    {
        Weekday = DayType.Any;
        RecalculateHours(hours);
    }
    
    static readonly Exception YouGoddamnFoolException = new("Exception: I'm not going to explain to you why you can't enter a number less than 0 or greater than 24 in the Workday Constructor.");

    public DayType Weekday { get; set; }

    public decimal RegularHours { get; set; }

    public decimal OvertimeHours { get; set; }

    public decimal TotalHours
    {
        get { return OvertimeHours + RegularHours; }
        set { RecalculateHours(value); }
    }

    private void RecalculateHours(decimal hours)
    {
        if (hours < 0.00m || hours > 24.00m)
            throw YouGoddamnFoolException;
        
        if (hours <= 8.00m)
        {
            RegularHours = hours;
            OvertimeHours = 0.00m;
        }
        else if (hours > 8.00m)
        {
            RegularHours = 8.00m;
            OvertimeHours = hours - 8.00m;
        }
    }

    public override string ToString() => $"{Weekday}: ({TotalHours} hours [{RegularHours} reg, {OvertimeHours} ot])";
}
}

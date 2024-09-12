using PayrollManager;

namespace PayrollManager
{
class Workday
{
    public Workday(DayType weekday, double hours)
    {
        Weekday = weekday;
        RecalculateHours(hours);
    }

    public Workday(double hours)
    {
        Weekday = DayType.Any;
        RecalculateHours(hours);
    }
    
    static readonly Exception YouGoddamnFoolException = new("Exception: I'm not going to explain to you why you can't enter a number less than 0 or greater than 24 in the Workday Constructor.");

    public DayType Weekday { get; set; }

    public double RegularHours { get; set; }

    public double OvertimeHours { get; set; }

    public double TotalHours
    {
        get { return OvertimeHours + RegularHours; }
        set { RecalculateHours(value); }
    }

    private void RecalculateHours(double hours)
    {
        if (hours < 0 || hours > 24)
            throw YouGoddamnFoolException;
        
        if (hours <= 8)
        {
            RegularHours = hours;
            OvertimeHours = 0;
        }
        else if (hours > 8)
        {
            RegularHours = 8;
            OvertimeHours = hours - 8;
        }
    }

    public override string ToString() => $"{Weekday}: ({TotalHours} hours [{RegularHours} reg, {OvertimeHours} ot])";
}
}

namespace PayrollManager
{

abstract class Employee(int ID, FullName name, GenderType gender)
{
    public int ID { get; } = ID;

    public FullName Name { get; set; } = name;

    public GenderType Gender { get; set; } = gender;

}

class HourlyEmployee(int ID, FullName name, GenderType gender, decimal rate) : Employee(ID, name, gender)
{
    public decimal Rate { get; set; } = rate;

    public decimal GetDayPay(Workday day) => Rate * day.RegularHours + Rate * 1.5m * day.OvertimeHours;

    public decimal GetWeekPay(WorkWeek week)
    {
        decimal res = 0;
        foreach (Workday day in week.Days)
            res += Rate * day.RegularHours + Rate * 1.5m * day.OvertimeHours;
        return res;
    }
}
}
